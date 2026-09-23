using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using SolutionComponents.Models;
using SolutionComponents.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace SolutionComponents
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;
        private readonly SolutionAuditService _auditService = new SolutionAuditService();
        private List<SolutionOverlapItem> _currentOverlaps = new List<SolutionOverlapItem>();
        private List<EntityMetadataSummary> _loadedEntities = new List<EntityMetadataSummary>();
        private bool _entitiesLoaded = false;

        public MyPluginControl()
        {
            InitializeComponent();
            PopulateComponentTypesDropDown();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
                LogWarning("Settings not found => a new default settings file has been created.");
            }
            else
            {
                LogInfo("Settings loaded successfully.");
            }

            if (cbComponentTypes.Items.Count == 0)
            {
                PopulateComponentTypesDropDown();
            }

            if (Service != null)
            {
                LoadSolutionsAsync();
            }
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection updated: {0}", detail.WebApplicationUrl);
            }

            _entitiesLoaded = false;
            _loadedEntities.Clear();

            if (Service != null)
            {
                if (rbSolution.Checked)
                {
                    LoadSolutionsAsync();
                }
                else
                {
                    HandleComponentTypeChanged();
                }
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (rbSolution.Checked)
            {
                ExecuteMethod(LoadSolutionsAsync);
            }
            else
            {
                ExecuteMethod(HandleComponentTypeChanged);
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSolution.Checked)
            {
                pnSolution.Visible = true;
                pnComponent.Visible = false;
                if (cbSolutions.Items.Count == 0 && Service != null)
                {
                    LoadSolutionsAsync();
                }
            }
            else if (rbComponent.Checked)
            {
                pnSolution.Visible = false;
                pnComponent.Visible = true;
                if (cbComponentTypes.SelectedIndex < 0 && cbComponentTypes.Items.Count > 0)
                {
                    cbComponentTypes.SelectedIndex = 0;
                }
            }
        }

        #region Mode: By Solution (REC-1)

        private void LoadSolutionsAsync()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions from Dataverse...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetSolutions(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError("Error while retrieving solutions", args.Error);
                        MessageBox.Show($"Failed to load solutions: {args.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var solutions = args.Result as List<SolutionSummary>;
                    cbSolutions.Items.Clear();
                    cbSolutionComponents.Items.Clear();
                    dgvRelatedSolutions.Rows.Clear();

                    if (solutions != null && solutions.Count > 0)
                    {
                        foreach (var sol in solutions)
                        {
                            cbSolutions.Items.Add(sol);
                        }
                        lblStatus.Text = $"{solutions.Count} solution(s) loaded.";
                    }
                    else
                    {
                        lblStatus.Text = "No solutions found.";
                    }
                }
            });
        }

        private void cbSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSolutions.SelectedItem is SolutionSummary selectedSolution)
            {
                LoadSolutionComponentsAndOverlapsAsync(selectedSolution.Id);
            }
        }

        private void LoadSolutionComponentsAndOverlapsAsync(Guid solutionId)
        {
            var excludedPublishers = mySettings?.ExcludedPublishers ?? new List<string>();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Auditing solution component overlaps...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetOverlappingSolutions(Service, solutionId, excludedPublishers);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError($"Error auditing overlaps for solution {solutionId}", args.Error);
                        MessageBox.Show($"Failed to audit solution overlaps: {args.Error.Message}", "Audit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _currentOverlaps = args.Result as List<SolutionOverlapItem> ?? new List<SolutionOverlapItem>();
                    PopulateComponentFilterComboBox();
                    DisplayOverlapsInGrid(_currentOverlaps);

                    lblStatus.Text = $"{_currentOverlaps.Count} overlap(s) detected.";
                }
            });
        }

        private void PopulateComponentFilterComboBox()
        {
            cbSolutionComponents.Items.Clear();
            cbSolutionComponents.Items.Add("All components");

            var distinctComponents = _currentOverlaps
                .Select(o => o.ComponentName)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(n => n, StringComparer.CurrentCultureIgnoreCase);

            foreach (var compName in distinctComponents)
            {
                cbSolutionComponents.Items.Add(compName);
            }

            if (cbSolutionComponents.Items.Count > 0)
            {
                cbSolutionComponents.SelectedIndex = 0;
            }
        }

        private void cbSolutionComponents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSolutionComponents.SelectedItem == null) return;

            string selected = cbSolutionComponents.SelectedItem.ToString();
            if (selected == "All components" || string.IsNullOrEmpty(selected))
            {
                DisplayOverlapsInGrid(_currentOverlaps);
            }
            else
            {
                var filtered = _currentOverlaps.Where(o => string.Equals(o.ComponentName, selected, StringComparison.CurrentCultureIgnoreCase)).ToList();
                DisplayOverlapsInGrid(filtered);
            }
        }

        private void DisplayOverlapsInGrid(List<SolutionOverlapItem> items)
        {
            dgvRelatedSolutions.Rows.Clear();

            if (items == null || items.Count == 0)
                return;

            foreach (var item in items)
            {
                dgvRelatedSolutions.Rows.Add(
                    item.ComponentName,
                    item.ComponentType,
                    item.RelatedSolutionName,
                    item.Version,
                    item.ModifiedBy,
                    item.CreatedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.ModifiedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.PublisherName,
                    item.ManagedDisplay
                );
            }
        }

        #endregion

        #region Mode: By Component (REC-2)

        private void PopulateComponentTypesDropDown()
        {
            cbComponentTypes.Items.Clear();

            var availableTypes = SolutionAuditService.ComponentTypes
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value.ComponentTypeName))
                .GroupBy(kvp => !string.IsNullOrEmpty(kvp.Value.EntityName)
                    ? $"{kvp.Value.ComponentTypeName} ({kvp.Value.EntityName})"
                    : kvp.Value.ComponentTypeName, StringComparer.CurrentCultureIgnoreCase)
                .Select(g => new ComboBoxTypeItem(g.Key, g.First().Key))
                .OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            foreach (var item in availableTypes)
            {
                cbComponentTypes.Items.Add(item);
            }

            if (cbComponentTypes.Items.Count > 0 && cbComponentTypes.SelectedIndex < 0)
            {
                cbComponentTypes.SelectedIndex = 0;
            }
        }

        private void cbComponentTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleComponentTypeChanged();
        }

        private void HandleComponentTypeChanged()
        {
            if (cbComponentTypes.SelectedItem is ComboBoxTypeItem selectedItem)
            {
                int compType = selectedItem.TypeCode;
                bool isEntityDependent = SolutionAuditService.IsEntityDependentComponent(compType);

                cbComponents.Items.Clear();
                dgvComponentHistory.Rows.Clear();

                if (compType == 29) // Workflow / Cloud Flow
                {
                    lblEntity.Text = "Process Category:";
                    lblEntity.Enabled = true;
                    cbEntities.Enabled = true;

                    cbEntities.Items.Clear();
                    cbEntities.Items.Add(new ProcessCategoryItem(null, "[All process types]"));
                    cbEntities.Items.Add(new ProcessCategoryItem(5, "Cloud Flow (Power Automate)"));
                    cbEntities.Items.Add(new ProcessCategoryItem(0, "Classic Workflow"));
                    cbEntities.Items.Add(new ProcessCategoryItem(4, "Business Process Flow (BPF)"));
                    cbEntities.Items.Add(new ProcessCategoryItem(3, "Action"));
                    cbEntities.Items.Add(new ProcessCategoryItem(2, "Business Rule"));
                    cbEntities.Items.Add(new ProcessCategoryItem(6, "Desktop Flow"));
                    cbEntities.Items.Add(new ProcessCategoryItem(7, "AI Flow"));
                    cbEntities.Items.Add(new ProcessCategoryItem(1, "Dialog"));

                    cbEntities.SelectedIndex = 0;
                }
                else if (isEntityDependent)
                {
                    lblEntity.Text = "Select Entity / Table:";
                    lblEntity.Enabled = true;
                    cbEntities.Enabled = true;

                    if (_entitiesLoaded && (cbEntities.Items.Count == 0 || cbEntities.Items[0] is ProcessCategoryItem))
                    {
                        cbEntities.Items.Clear();
                        foreach (var entity in _loadedEntities)
                        {
                            cbEntities.Items.Add(entity);
                        }
                        if (cbEntities.Items.Count > 0)
                        {
                            cbEntities.SelectedIndex = 0;
                        }
                    }
                    else if (!_entitiesLoaded && Service != null)
                    {
                        LoadEntitiesAsync(() =>
                        {
                            if (cbEntities.SelectedItem is EntityMetadataSummary loadedEnt)
                            {
                                LoadComponentsForTypeAndEntityAsync(compType, loadedEnt.LogicalName);
                            }
                        });
                        return;
                    }

                    if (cbEntities.SelectedItem is EntityMetadataSummary ent)
                    {
                        LoadComponentsForTypeAndEntityAsync(compType, ent.LogicalName);
                    }
                    else
                    {
                        lblComponentStatus.Text = "Please select an entity / table.";
                    }
                }
                else
                {
                    lblEntity.Text = "Select Entity / Table:";
                    cbEntities.Enabled = false;
                    lblEntity.Enabled = false;
                    cbEntities.SelectedIndex = -1;

                    LoadComponentsForTypeAndEntityAsync(compType, null);
                }
            }
        }

        private void LoadEntitiesAsync(Action onComplete = null)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving entities / tables from Dataverse...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetEntities(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError("Error retrieving entities", args.Error);
                        return;
                    }

                    _loadedEntities = args.Result as List<EntityMetadataSummary> ?? new List<EntityMetadataSummary>();
                    _entitiesLoaded = true;

                    cbEntities.Items.Clear();
                    foreach (var entity in _loadedEntities)
                    {
                        cbEntities.Items.Add(entity);
                    }

                    if (cbEntities.Items.Count > 0)
                    {
                        cbEntities.SelectedIndex = 0;
                    }

                    onComplete?.Invoke();
                }
            });
        }

        private void cbEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbComponentTypes.SelectedItem is ComboBoxTypeItem selectedType)
            {
                if (selectedType.TypeCode == 29)
                {
                    int? category = (cbEntities.SelectedItem as ProcessCategoryItem)?.Category;
                    LoadWorkflowsAsync(category);
                }
                else if (cbEntities.SelectedItem is EntityMetadataSummary selectedEntity)
                {
                    LoadComponentsForTypeAndEntityAsync(selectedType.TypeCode, selectedEntity.LogicalName);
                }
            }
        }

        private void LoadWorkflowsAsync(int? category)
        {
            if (Service == null) return;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading workflows / cloud flows...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetWorkflowsByCategory(Service, category);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError("Error retrieving workflows", args.Error);
                        MessageBox.Show($"Failed to load workflows: {args.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var components = args.Result as List<ComponentItem> ?? new List<ComponentItem>();
                    cbComponents.Items.Clear();
                    dgvComponentHistory.Rows.Clear();

                    foreach (var comp in components)
                    {
                        cbComponents.Items.Add(comp);
                    }

                    lblComponentStatus.Text = $"{components.Count} workflow(s) found.";
                    if (components.Count > 0)
                    {
                        cbComponents.SelectedIndex = 0;
                    }
                }
            });
        }

        private void LoadComponentsForTypeAndEntityAsync(int componentType, string entityLogicalName)
        {
            if (Service == null) return;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Loading components for type {componentType}...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetComponentsByType(Service, componentType, entityLogicalName);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError("Error retrieving components", args.Error);
                        MessageBox.Show($"Failed to load components: {args.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var components = args.Result as List<ComponentItem> ?? new List<ComponentItem>();
                    cbComponents.Items.Clear();
                    dgvComponentHistory.Rows.Clear();

                    foreach (var comp in components)
                    {
                        cbComponents.Items.Add(comp);
                    }

                    lblComponentStatus.Text = $"{components.Count} component(s) found.";
                    if (components.Count > 0)
                    {
                        cbComponents.SelectedIndex = 0;
                    }
                }
            });
        }

        private void cbComponents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbComponents.SelectedItem is ComponentItem selectedComponent)
            {
                LoadComponentHistoryAsync(selectedComponent.Id);
            }
        }

        private void LoadComponentHistoryAsync(Guid objectId)
        {
            if (Service == null || objectId == Guid.Empty) return;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Reconstructing component solution history...",
                Work = (worker, args) =>
                {
                    args.Result = _auditService.GetSolutionsForComponent(Service, objectId);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        LogError($"Error querying solutions for component {objectId}", args.Error);
                        MessageBox.Show($"Failed to retrieve component history: {args.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var history = args.Result as List<ComponentHistoryItem> ?? new List<ComponentHistoryItem>();
                    DisplayComponentHistoryInGrid(history);
                    lblComponentStatus.Text = $"Component found in {history.Count} solution(s).";
                }
            });
        }

        private void DisplayComponentHistoryInGrid(List<ComponentHistoryItem> history)
        {
            dgvComponentHistory.Rows.Clear();

            if (history == null || history.Count == 0)
                return;

            foreach (var item in history)
            {
                dgvComponentHistory.Rows.Add(
                    item.SolutionName,
                    item.Version,
                    item.ModifiedBy,
                    item.CreatedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.ModifiedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.PublisherName,
                    item.ManagedDisplay
                );
            }
        }

        public class ComboBoxTypeItem
        {
            public string Name { get; }
            public int TypeCode { get; }

            public ComboBoxTypeItem(string name, int typeCode)
            {
                Name = name;
                TypeCode = typeCode;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        #region ComboBox AutoComplete & Search Helpers

        private static bool TryCommitComboBoxSelection(ComboBox comboBox)
        {
            if (comboBox == null || comboBox.Items.Count == 0) return false;

            if (comboBox.SelectedItem != null && string.Equals(comboBox.Text, comboBox.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            string text = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(text)) return false;

            // 1. Exact match
            foreach (var item in comboBox.Items)
            {
                if (string.Equals(item.ToString(), text, StringComparison.CurrentCultureIgnoreCase))
                {
                    comboBox.SelectedItem = item;
                    return true;
                }
            }

            // 2. Prefix match
            foreach (var item in comboBox.Items)
            {
                if (item.ToString().StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                {
                    comboBox.SelectedItem = item;
                    return true;
                }
            }

            // 3. Substring match
            foreach (var item in comboBox.Items)
            {
                if (item.ToString().IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    comboBox.SelectedItem = item;
                    return true;
                }
            }

            return false;
        }

        private void cbSolutions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (TryCommitComboBoxSelection(cbSolutions))
                {
                    cbSolutions_SelectedIndexChanged(sender, EventArgs.Empty);
                }
            }
        }

        private void cbSolutions_Leave(object sender, EventArgs e)
        {
            if (TryCommitComboBoxSelection(cbSolutions))
            {
                cbSolutions_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        private void cbSolutionComponents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (TryCommitComboBoxSelection(cbSolutionComponents))
                {
                    cbSolutionComponents_SelectedIndexChanged(sender, EventArgs.Empty);
                }
            }
        }

        private void cbSolutionComponents_Leave(object sender, EventArgs e)
        {
            if (TryCommitComboBoxSelection(cbSolutionComponents))
            {
                cbSolutionComponents_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        private void cbComponentTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (TryCommitComboBoxSelection(cbComponentTypes))
                {
                    cbComponentTypes_SelectedIndexChanged(sender, EventArgs.Empty);
                }
            }
        }

        private void cbComponentTypes_Leave(object sender, EventArgs e)
        {
            if (TryCommitComboBoxSelection(cbComponentTypes))
            {
                cbComponentTypes_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        private void cbEntities_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (TryCommitComboBoxSelection(cbEntities))
                {
                    cbEntities_SelectedIndexChanged(sender, EventArgs.Empty);
                }
            }
        }

        private void cbEntities_Leave(object sender, EventArgs e)
        {
            if (TryCommitComboBoxSelection(cbEntities))
            {
                cbEntities_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        private void cbComponents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (TryCommitComboBoxSelection(cbComponents))
                {
                    cbComponents_SelectedIndexChanged(sender, EventArgs.Empty);
                }
            }
        }

        private void cbComponents_Leave(object sender, EventArgs e)
        {
            if (TryCommitComboBoxSelection(cbComponents))
            {
                cbComponents_SelectedIndexChanged(sender, EventArgs.Empty);
            }
        }

        #endregion

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            if (mySettings != null)
            {
                SettingsManager.Instance.Save(GetType(), mySettings);
            }
        }
    }
}