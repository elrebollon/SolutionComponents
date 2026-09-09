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

            PopulateComponentTypesDropDown();

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
            var excludedPublishers = mySettings?.ExcludedPublishers ?? new List<string> { "MicrosoftCorporation", "Microsoft" };

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
                .Distinct()
                .OrderBy(n => n);

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
                var filtered = _currentOverlaps.Where(o => o.ComponentName == selected).ToList();
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
                    item.PublisherName,
                    item.ManagedDisplay,
                    item.ModifiedBy,
                    item.CreatedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.InstalledOn?.ToString("yyyy-MM-dd HH:mm")
                );
            }
        }

        #endregion

        #region Mode: By Component (REC-2)

        private void PopulateComponentTypesDropDown()
        {
            cbComponentTypes.Items.Clear();

            var supportedTypes = new[]
            {
                new { Type = 24, Name = "Form (systemform)" },
                new { Type = 26, Name = "View (savedquery)" },
                new { Type = 29, Name = "Workflow / Cloud Flow (workflow)" },
                new { Type = 20, Name = "Role (role)" },
                new { Type = 61, Name = "Web Resource (webresource)" },
                new { Type = 300, Name = "Canvas App (canvasapp)" },
                new { Type = 380, Name = "Environment Variable (environmentvariabledefinition)" },
                new { Type = 1, Name = "Entity / Table (entity)" },
                new { Type = 9, Name = "Option Set (optionset)" },
                new { Type = 62, Name = "Site Map (sitemap)" },
                new { Type = 91, Name = "Plugin Assembly (pluginassembly)" },
                new { Type = 92, Name = "SDK Message Step (sdkmessageprocessingstep)" }
            };

            foreach (var item in supportedTypes)
            {
                cbComponentTypes.Items.Add(new ComboBoxTypeItem(item.Name, item.Type));
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

                if (isEntityDependent)
                {
                    cbEntities.Enabled = true;
                    lblEntity.Enabled = true;

                    if (!_entitiesLoaded && Service != null)
                    {
                        LoadEntitiesAsync(() =>
                        {
                            if (cbEntities.SelectedItem is EntityMetadataSummary ent)
                            {
                                LoadComponentsForTypeAndEntityAsync(compType, ent.LogicalName);
                            }
                        });
                    }
                    else if (cbEntities.SelectedItem is EntityMetadataSummary ent)
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
            if (cbComponentTypes.SelectedItem is ComboBoxTypeItem selectedType && cbEntities.SelectedItem is EntityMetadataSummary selectedEntity)
            {
                LoadComponentsForTypeAndEntityAsync(selectedType.TypeCode, selectedEntity.LogicalName);
            }
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
                    item.PublisherName,
                    item.ManagedDisplay,
                    item.ModifiedBy,
                    item.CreatedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.InstalledOn?.ToString("yyyy-MM-dd HH:mm")
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

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            if (mySettings != null)
            {
                SettingsManager.Instance.Save(GetType(), mySettings);
            }
        }
    }
}