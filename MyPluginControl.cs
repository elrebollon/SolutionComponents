using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Organization;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace SolutionComponents
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;
      
        private static readonly Dictionary<int, (string componentTypeName, string entityName)> ComponentTypes = new Dictionary<int, (string, string)>
        {
            { 1, ("Entity", "entity") },
            { 2, ("Entity field", "attribute") }, //Attribute
            { 3, ("Relationship", "relationship") },
            { 4, ("Attribute Picklist Value","") },
            { 5, ("Attribute Lookup Value","") },
            { 6, ("View Attribute","") },
            { 7, ("Localized Label","") },
            { 8, ("Relationship Extra Condition","") },
            { 9, ("Option Set","optionset") },
            { 10, ("Entity Relationship","entityrelationship") },
            { 11, ("Entity Relationship Role","") },
            { 12, ("Entity Relationship Relationships","") },
            { 13, ("Managed Property","") },
            { 14, ("Entity Key","entitykey") },
            { 16, ("Privilege","") },
            { 17, ("PrivilegeObjectTypeCode","") },
            { 18, ("Index","entityindex") },
            { 20, ("Role", "role") },
            { 21, ("Role Privilege","") },
            { 22, ("Display String","") },
            { 23, ("Display String Map","") },
            { 24, ("Form","systemform") },
            { 25, ("Organization","") },
            { 26, ("View","savedquery") },
            { 29, ("Workflow", "workflow") },
            { 31, ("Report","") },
            { 32, ("Report Entity","") },
            { 33, ("Report Category","") },
            { 34, ("Report Visibility","") },
            { 35, ("Attachment","") },
            { 36, ("Email Template","") },
            { 37, ("Contract Template","") },
            { 38, ("KB Article Template","") },
            { 39, ("Mail Merge Template","") },
            { 44, ("Duplicate Rule","") },
            { 45, ("Duplicate Rule Condition","") },
            { 46, ("Entity Map","") },
            { 47, ("Attribute Map","") },
            { 48, ("Ribbon Command","") },
            { 49, ("Ribbon Context Group","") },
            { 50, ("Ribbon Customization","") },
            { 52, ("Ribbon Rule","") },
            { 53, ("Ribbon Tab To Command Map","") },
            { 55, ("Ribbon Diff","") },
            { 59, ("Charts","") }, //Saved Query Visualization
            { 60, ("Entity Form", "userquery") },
            { 61, ("Web Resource","") },
            { 62, ("Site Map","") },
            { 63, ("Connection Role","") },
            { 64, ("Complex Control","") },
            { 65, ("Hierarchy Rule","") },
            { 66, ("Custom Control","") },
            { 68, ("Custom Control Default Config","") },
            { 70, ("Field Security Profile","") },
            { 71, ("Field Permission","") },
            { 90, ("Plugin Type","") },
            { 91, ("Plugin Assembly","") },
            { 92, ("SDK Message Processing Step","") },
            { 93, ("SDK Message Processing Step Image","") },
            { 95, ("Service Endpoint","") },
            { 150, ("Routing Rule","") },
            { 151, ("Routing Rule Item","") },
            { 152, ("SLA","") },
            { 153, ("SLA Item","") },
            { 154, ("Convert Rule","") },
            { 155, ("Convert Rule Item","") },
            { 161, ("Mobile Offline Profile","") },
            { 162, ("Mobile Offline Profile Item","") },
            { 165, ("Similarity Rule","") },
            { 166, ("Data Source Mapping","") },
            { 201, ("SDKMessage","") },
            { 202, ("SDKMessageFilter","") },
            { 203, ("SdkMessagePair","") },
            { 204, ("SdkMessageRequest","") },
            { 205, ("SdkMessageRequestField","") },
            { 206, ("SdkMessageResponse","") },
            { 207, ("SdkMessageResponseField","") },
            { 210, ("WebWizard","") },
            { 208, ("Import Map","") },
            { 300, ("Canvas App","") },
            { 371, ("Connector","") },
            { 372, ("Connector","") },
            { 380, ("Environment Variable Definition","") },
            { 381, ("Environment Variable Value","") },
            { 400, ("AI Project Type","") },
            { 401, ("AI Project","") },
            { 402, ("AI Configuration","") },
            { 430, ("Entity Analytics Configuration","") },
            { 431, ("Attribute Image Configuration","") },
            { 432, ("Entity Image Configuration","") }
        };

        public MyPluginControl()
        {
            InitializeComponent();
            LoadSolutions();
        }

        private void LoadSolutions()
        {
            try
            {
                var service = this.Service;

                var query = new QueryExpression("solution")
                {
                    ColumnSet = new ColumnSet("friendlyname", "solutionid")
                };

                EntityCollection result = service.RetrieveMultiple(query);

                cbSolutions.Items.Clear();

                foreach (var entity in result.Entities)
                {
                    string solutionName = entity.GetAttributeValue<string>("friendlyname");
                    Guid solutionId = entity.Id;

                    ComboBoxItem item = new ComboBoxItem(solutionName, solutionId);
                    cbSolutions.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error while loading solutions", ex);
            }
        }

        private void cbSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSolutions.SelectedItem != null)
            {
                ComboBoxItem selectedSolution = (ComboBoxItem)cbSolutions.SelectedItem;
                Guid solutionId = selectedSolution.Id;
                LoadSolutionComponents(solutionId);
            }
        }

        private void LoadSolutionComponents(Guid solutionId)
        {
            try
            {
                var service = this.Service;

                var query = new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("componenttype", "objectid")
                };

                query.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solutionId);

                EntityCollection result = service.RetrieveMultiple(query);

                cbSolutionComponents.Items.Clear();
                cbSolutionComponents.Items.Add(new ComboBoxItem("All components", Guid.Empty));

                foreach (var entity in result.Entities)
                {
                    int componentType = entity.GetAttributeValue<int>("componenttype");
                    Guid objectId = entity.GetAttributeValue<Guid>("objectid");

                    var componentDetails = GetComponentTypeDetails(componentType);

                    string componentName = GetComponentName(service, componentDetails.entityName, objectId);
                    string relatedEntityName = null;

                    if (IsComponentRelatedToEntity(componentType))
                    {
                        relatedEntityName = GetEntityNameByComponent(service, componentDetails.entityName, objectId);
                    }

                    if (!string.IsNullOrEmpty(relatedEntityName))
                    {
                        componentName += $" (Entity: {relatedEntityName})";
                    }

                    ComboBoxItem item = new ComboBoxItem($"{componentDetails.componentTypeName}: {componentName}", objectId);

                    cbSolutionComponents.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error while loading solution components for SolutionId: {solutionId}", ex);
            }
        }

        private bool IsComponentRelatedToEntity(int componentType)
        {
            // Ejemplos: 60 (form), 65 (view), 150 (dashboard), etc.
            return componentType == 60 || componentType == 65 || componentType == 150;
        }

        private string GetEntityNameByComponent(IOrganizationService service, string entityLogicalName, Guid objectId)
        {
            try
            {
                // Cargar el componente específico y obtener su "objecttypecode"
                Entity componentEntity = service.Retrieve(entityLogicalName, objectId, new ColumnSet("objecttypecode"));

                // El "objecttypecode" es el código de la entidad a la que pertenece el componente
                if (componentEntity.Contains("objecttypecode"))
                {
                    int entityTypeCode = componentEntity.GetAttributeValue<int>("objecttypecode");

                    // Consultar para obtener el nombre lógico de la entidad a partir del código
                    return GetEntityLogicalNameByTypeCode(service, entityTypeCode);
                }

                return string.Empty; // Retornar cadena vacía si no tiene un objecttypecode
            }
            catch (Exception ex)
            {
                LogError($"Error retrieving entity name for component {objectId}", ex);
                return string.Empty; // En caso de error, retornar cadena vacía
            }
        }

        private string GetEntityLogicalNameByTypeCode(IOrganizationService service, int entityTypeCode)
        {
            return entityTypeCode.ToString();
        }

        private (string componentTypeName, string entityName) GetComponentTypeDetails(int componentType)
        {
            return ComponentTypes.ContainsKey(componentType)
                ? ComponentTypes[componentType]
                : ("Unknown", string.Empty);
        }

        private string GetComponentName(IOrganizationService service, string entityName, Guid objectId)
        {
            if (string.IsNullOrEmpty(entityName)) return "Unknown";

            Entity entity = service.Retrieve(entityName, objectId, new ColumnSet("name"));
            return entity.Contains("name") ? entity.GetAttributeValue<string>("name") : "Unnamed";
        }

        private void cbSolutionComponents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSolutionComponents.SelectedItem != null)
            {
                // Assuming the component object ID is stored in the ComboBox item (could be handled differently)
                ComboBoxItem selectedComponent = (ComboBoxItem)cbSolutions.SelectedItem;

                if (selectedComponent.Id == Guid.Empty)
                {
                    // All components
                    LoadRelatedSolutionsForAllComponents();
                }
                else
                {
                    Guid componentId = selectedComponent.Id;
                    string componentName = selectedComponent.Name;
                    LoadRelatedSolutionsForComponent(componentId, componentName);
                }

            }
        }

        private void LoadRelatedSolutionsForAllComponents()
        {
            try
            {
                var service = this.Service;
                Guid selectedSolutionId = ((ComboBoxItem)cbSolutions.SelectedItem).Id;

                // Consulta para obtener todos los componentes de la solución seleccionada
                var componentQuery = new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("componenttype", "objectid", "solutionid")
                };
                componentQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, selectedSolutionId);

                EntityCollection components = service.RetrieveMultiple(componentQuery);

                dgvRelatedSolutions.Rows.Clear();

                // Para cada componente, obtener las soluciones relacionadas
                foreach (var component in components.Entities)
                {
                    Guid componentId = component.GetAttributeValue<Guid>("objectid");
                    int componentType = component.GetAttributeValue<int>("componenttype");

                    // Obtener las soluciones relacionadas con este componente
                    var relatedSolutionsQuery = new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("solutionid")
                    };
                    relatedSolutionsQuery.Criteria.AddCondition("objectid", ConditionOperator.Equal, componentId);

                    EntityCollection relatedSolutions = service.RetrieveMultiple(relatedSolutionsQuery);

                    foreach (var solutionComponent in relatedSolutions.Entities)
                    {
                        Guid solutionId = solutionComponent.GetAttributeValue<Guid>("solutionid");
                        var solution = service.Retrieve("solution", solutionId, new ColumnSet("friendlyname", "createdon", "installedon", "ismanaged"));

                        var componentDetails = GetComponentTypeDetails(componentType);
                        string componentName = GetComponentName(service, componentDetails.entityName, componentId);
                        string solutionName = solution.GetAttributeValue<string>("friendlyname");
                        DateTime? createdOn = solution.GetAttributeValue<DateTime?>("createdon");
                        DateTime? installedOn = solution.GetAttributeValue<DateTime?>("installedon");
                        bool isManaged = solution.GetAttributeValue<bool>("ismanaged");

                        dgvRelatedSolutions.Rows.Add(componentName, solutionName, createdOn, installedOn, isManaged ? "Yes" : "No", componentName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error loading related solutions for all components", ex);
            }
        }

        private void LoadRelatedSolutionsForComponent(Guid componentId, string componentName = "")
        {
            try
            {
                var service = this.Service;

                // Query to retrieve solutions that contain the selected component
                var query = new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("solutionid")
                };

                query.Criteria.AddCondition("objectid", ConditionOperator.Equal, componentId);

                EntityCollection result = service.RetrieveMultiple(query);

                dgvRelatedSolutions.Rows.Clear();

                foreach (var entity in result.Entities)
                {
                    Guid solutionId = entity.GetAttributeValue<Guid>("solutionid");

                    // Retrieve solution details
                    var solution = service.Retrieve("solution", solutionId, new ColumnSet("friendlyname", "createdon", "installedon", "ismanaged"));

                    string solutionName = solution.GetAttributeValue<string>("friendlyname");
                    DateTime? createdOn = solution.GetAttributeValue<DateTime?>("createdon");
                    DateTime? installedOn = solution.GetAttributeValue<DateTime?>("installedon");
                    bool isManaged = solution.GetAttributeValue<bool>("ismanaged");

                    dgvRelatedSolutions.Rows.Add(componentName, solutionName, createdOn, installedOn, isManaged ? "Yes" : "No");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error while loading related solutions for Component: {componentId}", ex);
            }
        }

        // Clase para representar los elementos del ComboBox
        public class ComboBoxItem
        {
            public string Name { get; set; }
            public Guid Id { get; set; }

            public ComboBoxItem(string name, Guid id)
            {
                Name = name;
                Id = id;
            }

            // Sobrescribir el método ToString para que el ComboBox muestre el nombre
            public override string ToString()
            {
                return Name;
            }
        }
    

    /********************************/

    private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAccounts);
        }

        private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }
    }
}