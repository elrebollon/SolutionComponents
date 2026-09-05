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

            if (Service != null)
            {
                LoadSolutionsAsync();
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadSolutionsAsync);
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbComponent.Checked)
            {
                lblStatus.Text = "Component search hierarchy will be available in the next release.";
            }
            else
            {
                lblStatus.Text = string.Empty;
            }
        }

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
                    item.CreatedOn?.ToString("yyyy-MM-dd HH:mm"),
                    item.InstalledOn?.ToString("yyyy-MM-dd HH:mm")
                );
            }
        }

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            if (mySettings != null)
            {
                SettingsManager.Instance.Save(GetType(), mySettings);
            }
        }
    }
}