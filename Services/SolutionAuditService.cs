using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using SolutionComponents.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolutionComponents.Services
{
    public class SolutionAuditService
    {
        public static readonly Dictionary<int, ComponentTypeMetadata> ComponentTypes = new Dictionary<int, ComponentTypeMetadata>
        {
            { 1, new ComponentTypeMetadata(1, "Entity", "entity", "entityid", "name", new[] { "logicalname" }, true) },
            { 2, new ComponentTypeMetadata(2, "Entity field", "attribute", "attributeid", "name", new[] { "logicalname" }, true, "entityid") },
            { 3, new ComponentTypeMetadata(3, "Relationship", "relationship", "relationshipid", "name", new[] { "schemaname" }, true) },
            { 4, new ComponentTypeMetadata(4, "Attribute Picklist Value", "attributepicklistvalue", "attributepicklistvalueid", "value", null, true) },
            { 5, new ComponentTypeMetadata(5, "Attribute Lookup Value", "attributelookupvalue", "attributelookupvalueid", "value", null, true) },
            { 6, new ComponentTypeMetadata(6, "View Attribute", "viewattribute", "viewattributeid", "name", null, true) },
            { 7, new ComponentTypeMetadata(7, "Localized Label", "localizedlabel", "localizedlabelid", "label", null, true) },
            { 8, new ComponentTypeMetadata(8, "Relationship Extra Condition", "relationshipextracondition", "relationshipextraconditionid", "name", null, true) },
            { 9, new ComponentTypeMetadata(9, "Option Set / Choice", "optionset", "optionsetid", "name", new[] { "displayname" }, true) },
            { 10, new ComponentTypeMetadata(10, "Entity Relationship", "entityrelationship", "entityrelationshipid", "schemaname", null, true) },
            { 11, new ComponentTypeMetadata(11, "Entity Relationship Role", "entityrelationshiprole", "entityrelationshiproleid", "name", null, true) },
            { 12, new ComponentTypeMetadata(12, "Entity Relationship Relationships", "entityrelationshiprelationships", "entityrelationshiprelationshipsid", "name", null, true) },
            { 13, new ComponentTypeMetadata(13, "Managed Property", "managedproperty", "managedpropertyid", "logicalname", null, true) },
            { 14, new ComponentTypeMetadata(14, "Entity Key", "entitykey", "entitykeyid", "logicalname", new[] { "schemaname" }, true) },
            { 15, new ComponentTypeMetadata(15, "Relationship Role", "relationshiprole", "relationshiproleid", "name") },
            { 16, new ComponentTypeMetadata(16, "Privilege", "privilege", "privilegeid", "name") },
            { 17, new ComponentTypeMetadata(17, "PrivilegeObjectTypeCode", "privilegeobjecttypecode", "privilegeobjecttypecodeid", "privilegeobjecttypecodeid") },
            { 18, new ComponentTypeMetadata(18, "Index", "entityindex", "indexid", "name", null, true) },
            { 20, new ComponentTypeMetadata(20, "Role", "role", "roleid", "name") },
            { 21, new ComponentTypeMetadata(21, "Role Privilege", "roleprivileges", "roleprivilegeid", "roleprivilegeid") },
            { 22, new ComponentTypeMetadata(22, "Display String", "displaystring", "displaystringid", "customdisplaystring", new[] { "formatparameters" }) },
            { 23, new ComponentTypeMetadata(23, "Display String Map", "displaystringmap", "displaystringmapid", "displaystringmapid") },
            { 24, new ComponentTypeMetadata(24, "Form", "systemform", "formid", "name", new[] { "objecttypecode" }) },
            { 25, new ComponentTypeMetadata(25, "Organization", "organization", "organizationid", "name") },
            { 26, new ComponentTypeMetadata(26, "View", "savedquery", "savedqueryid", "name", new[] { "returnedtypecode" }) },
            { 29, new ComponentTypeMetadata(29, "Workflow / Cloud Flow", "workflow", "workflowid", "name") },
            { 31, new ComponentTypeMetadata(31, "Report", "report", "reportid", "name") },
            { 32, new ComponentTypeMetadata(32, "Report Entity", "reportentity", "reportentityid", "reportentityid") },
            { 33, new ComponentTypeMetadata(33, "Report Category", "reportcategory", "reportcategoryid", "name") },
            { 34, new ComponentTypeMetadata(34, "Report Visibility", "reportvisibility", "reportvisibilityid", "reportvisibilityid") },
            { 35, new ComponentTypeMetadata(35, "Attachment", "attachment", "attachmentid", "filename", new[] { "subject" }) },
            { 36, new ComponentTypeMetadata(36, "Email Template", "template", "templateid", "title", new[] { "subject" }) },
            { 37, new ComponentTypeMetadata(37, "Contract Template", "contracttemplate", "contracttemplateid", "name") },
            { 38, new ComponentTypeMetadata(38, "KB Article Template", "kbarticletemplate", "kbarticletemplateid", "title") },
            { 39, new ComponentTypeMetadata(39, "Mail Merge Template", "mailmergetemplate", "mailmergetemplateid", "name") },
            { 44, new ComponentTypeMetadata(44, "Duplicate Rule", "duplicaterule", "duplicateruleid", "name") },
            { 45, new ComponentTypeMetadata(45, "Duplicate Rule Condition", "duplicaterulecondition", "duplicateruleconditionid", "duplicateruleconditionid") },
            { 46, new ComponentTypeMetadata(46, "Entity Map", "entitymap", "entitymapid", "sourceentityname", new[] { "targetentityname" }) },
            { 47, new ComponentTypeMetadata(47, "Attribute Map", "attributemap", "attributemapid", "sourceattributename", new[] { "targetattributename" }) },
            { 48, new ComponentTypeMetadata(48, "Ribbon Command", "ribboncommand", "ribboncommandid", "command") },
            { 49, new ComponentTypeMetadata(49, "Ribbon Context Group", "ribboncontextgroup", "ribboncontextgroupid", "contextgroupid") },
            { 50, new ComponentTypeMetadata(50, "Ribbon Customization", "ribboncustomization", "ribboncustomizationid", "entity") },
            { 52, new ComponentTypeMetadata(52, "Ribbon Rule", "ribbonrule", "ribbonruleid", "ruleid") },
            { 53, new ComponentTypeMetadata(53, "Ribbon Tab To Command Map", "ribbontabtocommandmap", "ribbontabtocommandmapid", "command") },
            { 55, new ComponentTypeMetadata(55, "Ribbon Diff", "ribbondiff", "ribbondiffid", "diffid") },
            { 59, new ComponentTypeMetadata(59, "Charts", "savedqueryvisualization", "savedqueryvisualizationid", "name", new[] { "primaryentitytypecode" }) },
            { 60, new ComponentTypeMetadata(60, "System Form", "systemform", "formid", "name", new[] { "objecttypecode" }) },
            { 61, new ComponentTypeMetadata(61, "Web Resource", "webresource", "webresourceid", "name", new[] { "displayname" }) },
            { 62, new ComponentTypeMetadata(62, "Site Map", "sitemap", "sitemapid", "sitemapname", new[] { "sitemapnameunique" }) },
            { 63, new ComponentTypeMetadata(63, "Connection Role", "connectionrole", "connectionroleid", "name") },
            { 64, new ComponentTypeMetadata(64, "Complex Control", "complexcontrol", "complexcontrolid", "name") },
            { 65, new ComponentTypeMetadata(65, "Hierarchy Rule", "hierarchyrule", "hierarchyruleid", "name", new[] { "primaryentityname" }) },
            { 66, new ComponentTypeMetadata(66, "Custom Control", "customcontrol", "customcontrolid", "name", new[] { "manifest" }) },
            { 68, new ComponentTypeMetadata(68, "Custom Control Default Config", "customcontroldefaultconfig", "customcontroldefaultconfigid", "customcontrolid") },
            { 70, new ComponentTypeMetadata(70, "Field Security Profile", "fieldsecurityprofile", "fieldsecurityprofileid", "name") },
            { 71, new ComponentTypeMetadata(71, "Field Permission", "fieldpermission", "fieldpermissionid", "attributelogicalname", new[] { "entityname" }) },
            { 72, new ComponentTypeMetadata(72, "Form Parameter", "formparameter", "formparameterid", "name") },
            { 80, new ComponentTypeMetadata(80, "Model-Driven App", "appmodule", "appmoduleid", "name", new[] { "uniquename" }) },
            { 81, new ComponentTypeMetadata(81, "App Module Component", "appmodulecomponent", "appmodulecomponentid", "appmodulecomponentid") },
            { 90, new ComponentTypeMetadata(90, "Plugin Type", "plugintype", "plugintypeid", "name", new[] { "typename" }) },
            { 91, new ComponentTypeMetadata(91, "Plugin Assembly", "pluginassembly", "pluginassemblyid", "name") },
            { 92, new ComponentTypeMetadata(92, "SDK Message Processing Step", "sdkmessageprocessingstep", "sdkmessageprocessingstepid", "name") },
            { 93, new ComponentTypeMetadata(93, "SDK Message Processing Step Image", "sdkmessageprocessingstepimage", "sdkmessageprocessingstepimageid", "name", new[] { "entityalias" }) },
            { 94, new ComponentTypeMetadata(94, "Service Endpoint Filter", "serviceendpointfilter", "serviceendpointfilterid", "name") },
            { 95, new ComponentTypeMetadata(95, "Service Endpoint", "serviceendpoint", "serviceendpointid", "name") },
            { 150, new ComponentTypeMetadata(150, "Routing Rule", "routingrule", "routingruleid", "name") },
            { 151, new ComponentTypeMetadata(151, "Routing Rule Item", "routingruleitem", "routingruleitemid", "name") },
            { 152, new ComponentTypeMetadata(152, "SLA", "sla", "slaid", "name") },
            { 153, new ComponentTypeMetadata(153, "SLA Item", "slaitem", "slaitemid", "name") },
            { 154, new ComponentTypeMetadata(154, "Convert Rule", "convertrule", "convertruleid", "name") },
            { 155, new ComponentTypeMetadata(155, "Convert Rule Item", "convertruleitem", "convertruleitemid", "name") },
            { 161, new ComponentTypeMetadata(161, "Mobile Offline Profile", "mobileofflineprofile", "mobileofflineprofileid", "name") },
            { 162, new ComponentTypeMetadata(162, "Mobile Offline Profile Item", "mobileofflineprofileitem", "mobileofflineprofileitemid", "name") },
            { 165, new ComponentTypeMetadata(165, "Similarity Rule", "similarityrule", "similarityruleid", "name") },
            { 166, new ComponentTypeMetadata(166, "Data Source Mapping", "datasourcemapping", "datasourcemappingid", "datasourcemappingid") },
            { 201, new ComponentTypeMetadata(201, "SDK Message", "sdkmessage", "sdkmessageid", "name") },
            { 202, new ComponentTypeMetadata(202, "SDK Message Filter", "sdkmessagefilter", "sdkmessagefilterid", "primaryobjecttypecode") },
            { 203, new ComponentTypeMetadata(203, "SDK Message Pair", "sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid") },
            { 204, new ComponentTypeMetadata(204, "SDK Message Request", "sdkmessagerequest", "sdkmessagerequestid", "name", new[] { "primaryobjecttypecode" }) },
            { 205, new ComponentTypeMetadata(205, "SDK Message Request Field", "sdkmessagerequestfield", "sdkmessagerequestfieldid", "name") },
            { 206, new ComponentTypeMetadata(206, "SDK Message Response", "sdkmessageresponse", "sdkmessageresponseid", "sdkmessageresponseid") },
            { 207, new ComponentTypeMetadata(207, "SDK Message Response Field", "sdkmessageresponsefield", "sdkmessageresponsefieldid", "name") },
            { 208, new ComponentTypeMetadata(208, "Import Map", "importmap", "importmapid", "name") },
            { 210, new ComponentTypeMetadata(210, "WebWizard", "webwizard", "webwizardid", "name") },
            { 300, new ComponentTypeMetadata(300, "Canvas App", "canvasapp", "canvasappid", "displayname", new[] { "name" }) },
            { 371, new ComponentTypeMetadata(371, "Connector", "connector", "connectorid", "displayname", new[] { "name" }) },
            { 372, new ComponentTypeMetadata(372, "Connection Reference", "connectionreference", "connectionreferenceid", "connectionreferencedisplayname", new[] { "connectionreferencename" }) },
            { 380, new ComponentTypeMetadata(380, "Environment Variable Definition", "environmentvariabledefinition", "environmentvariabledefinitionid", "displayname", new[] { "schemaname" }) },
            { 381, new ComponentTypeMetadata(381, "Environment Variable Value", "environmentvariablevalue", "environmentvariablevalueid", "value", null, false, "environmentvariabledefinitionid") },
            { 400, new ComponentTypeMetadata(400, "AI Project Type", "msdyn_aiprojecttype", "msdyn_aiprojecttypeid", "msdyn_name") },
            { 401, new ComponentTypeMetadata(401, "AI Project", "msdyn_aiproject", "msdyn_aiprojectid", "msdyn_name") },
            { 402, new ComponentTypeMetadata(402, "AI Configuration", "msdyn_aiconfiguration", "msdyn_aiconfigurationid", "msdyn_name") },
            { 430, new ComponentTypeMetadata(430, "Entity Analytics Configuration", "entityanalyticsconfig", "entityanalyticsconfigid", "entityanalyticsconfigid") },
            { 431, new ComponentTypeMetadata(431, "Attribute Image Configuration", "attributeimageconfig", "attributeimageconfigid", "attributeimageconfigid") },
            { 432, new ComponentTypeMetadata(432, "Entity Image Configuration", "entityimageconfig", "entityimageconfigid", "entityimageconfigid") },
            { 973, new ComponentTypeMetadata(973, "Copilot / Bot", "bot", "botid", "name", new[] { "schemaname" }) },
            { 974, new ComponentTypeMetadata(974, "Bot Component", "botcomponent", "botcomponentid", "name", new[] { "schemaname" }) },
            { 10020, new ComponentTypeMetadata(10020, "Custom API", "customapi", "customapiid", "displayname", new[] { "uniquename", "name" }) },
            { 10000, new ComponentTypeMetadata(10000, "Web Resource Dependency", "webresourcedependency", "webresourcedependencyid", "webresourcedependencyid") },
            { 10021, new ComponentTypeMetadata(10021, "Custom API Request Parameter", "customapirequestparameter", "customapirequestparameterid", "displayname", new[] { "uniquename", "name" }) },
            { 10022, new ComponentTypeMetadata(10022, "Custom API Response Property", "customapiresponseproperty", "customapiresponsepropertyid", "displayname", new[] { "uniquename", "name" }) },
            { 10040, new ComponentTypeMetadata(10040, "Dataflow", "msdyn_dataflow", "msdyn_dataflowid", "msdyn_name") },
            { 10060, new ComponentTypeMetadata(10060, "Catalog", "catalog", "catalogid", "displayname", new[] { "uniquename", "name" }) },
            { 10061, new ComponentTypeMetadata(10061, "Catalog Assignment", "catalogassignment", "catalogassignmentid", "name") },
            { 10080, new ComponentTypeMetadata(10080, "Plugin Package", "pluginpackage", "pluginpackageid", "name") },
            { 10090, new ComponentTypeMetadata(10090, "Desktop Flow Binary", "workflow", "workflowid", "name") },
            { 10091, new ComponentTypeMetadata(10091, "Plugin Package", "pluginpackage", "pluginpackageid", "name") },
            { 10097, new ComponentTypeMetadata(10097, "Desktop Flow", "workflow", "workflowid", "name") },
            { 10098, new ComponentTypeMetadata(10098, "Connection Reference", "connectionreference", "connectionreferenceid", "connectionreferencedisplayname", new[] { "connectionreferencename" }) },
            { 10110, new ComponentTypeMetadata(10110, "Power Pages Site", "powerpagesite", "powerpagesiteid", "name") },
            { 10111, new ComponentTypeMetadata(10111, "Power Pages Component", "powerpagescomponent", "powerpagescomponentid", "name") },
            { 10200, new ComponentTypeMetadata(10200, "AI Model / Prompt", "msdyn_aimodel", "msdyn_aimodelid", "msdyn_name") },
            { 10201, new ComponentTypeMetadata(10201, "AI Builder File", "msdyn_aifile", "msdyn_aifileid", "msdyn_name") },
            { 10202, new ComponentTypeMetadata(10202, "AI Builder File Document", "msdyn_aifiledocument", "msdyn_aifiledocumentid", "msdyn_name") }
        };

        public static readonly Dictionary<int, string> WorkflowCategories = new Dictionary<int, string>
        {
            { 0, "Classic Workflow" },
            { 1, "Dialog" },
            { 2, "Business Rule" },
            { 3, "Action" },
            { 4, "Business Process Flow (BPF)" },
            { 5, "Cloud Flow (Power Automate)" },
            { 6, "Desktop Flow" },
            { 7, "AI Flow" }
        };

        public static string FormatWorkflowName(string name, int category)
        {
            if (string.IsNullOrEmpty(name)) return name;

            string prefix;
            switch (category)
            {
                case 0: prefix = "[Classic Workflow]"; break;
                case 1: prefix = "[Dialog]"; break;
                case 2: prefix = "[Business Rule]"; break;
                case 3: prefix = "[Action]"; break;
                case 4: prefix = "[BPF]"; break;
                case 5: prefix = "[Cloud Flow]"; break;
                case 6: prefix = "[Desktop Flow]"; break;
                case 7: prefix = "[AI Flow]"; break;
                default: prefix = $"[Process {category}]"; break;
            }

            if (name.StartsWith(prefix + " ", StringComparison.OrdinalIgnoreCase) || name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return name;

            return $"{prefix} {name}";
        }

        public static bool IsEntityDependentComponent(int componentType)
        {
            // Tipos dependientes de una tabla/entidad (Forms, Views, Attributes, Relationships, Keys, Charts, Maps, Rules, etc.)
            return componentType == 2   // Attribute / Column
                || componentType == 3   // Relationship (1:N, N:1)
                || componentType == 4   // Attribute Picklist Value
                || componentType == 5   // Attribute Lookup Value
                || componentType == 6   // View Attribute
                || componentType == 7   // Localized Label
                || componentType == 8   // Relationship Extra Condition
                || componentType == 10  // Entity Relationship (N:N)
                || componentType == 11  // Entity Relationship Role
                || componentType == 12  // Entity Relationship Relationships
                || componentType == 14  // Entity Key / Alternate Key
                || componentType == 18  // Index
                || componentType == 22  // Display String
                || componentType == 23  // Display String Map
                || componentType == 24  // Form (systemform)
                || componentType == 26  // View (savedquery)
                || componentType == 46  // Entity Map
                || componentType == 47  // Attribute Map
                || componentType == 50  // Ribbon Customization
                || componentType == 59  // Charts (savedqueryvisualization)
                || componentType == 60  // System Form (systemform)
                || componentType == 65  // Hierarchy Rule (hierarchyrule)
                || componentType == 68  // Custom Control Default Config
                || componentType == 71  // Field Permission
                || componentType == 430 // Entity Analytics Configuration
                || componentType == 431 // Attribute Image Configuration
                || componentType == 432; // Entity Image Configuration
        }

        public List<SolutionSummary> GetSolutions(IOrganizationService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var query = new QueryExpression("solution")
            {
                ColumnSet = new ColumnSet("solutionid", "friendlyname", "uniquename", "version", "ismanaged", "publisherid"),
                Orders = { new OrderExpression("friendlyname", OrderType.Ascending) }
            };

            var linkPublisher = query.AddLink("publisher", "publisherid", "publisherid", JoinOperator.LeftOuter);
            linkPublisher.Columns = new ColumnSet("friendlyname");
            linkPublisher.EntityAlias = "pub";

            var result = service.RetrieveMultiple(query);
            var list = new List<SolutionSummary>();

            foreach (var entity in result.Entities)
            {
                string publisherName = null;
                if (entity.Contains("pub.friendlyname"))
                {
                    var aliased = entity.GetAttributeValue<AliasedValue>("pub.friendlyname");
                    if (aliased != null && aliased.Value != null)
                        publisherName = aliased.Value.ToString();
                }
                if (string.IsNullOrWhiteSpace(publisherName) && entity.FormattedValues != null && entity.FormattedValues.TryGetValue("publisherid", out string formattedPub) && !string.IsNullOrWhiteSpace(formattedPub))
                {
                    publisherName = formattedPub;
                }
                if (string.IsNullOrWhiteSpace(publisherName))
                {
                    publisherName = entity.GetAttributeValue<EntityReference>("publisherid")?.Name;
                }

                list.Add(new SolutionSummary
                {
                    Id = entity.Id,
                    FriendlyName = entity.GetAttributeValue<string>("friendlyname"),
                    UniqueName = entity.GetAttributeValue<string>("uniquename"),
                    Version = entity.GetAttributeValue<string>("version"),
                    PublisherName = publisherName ?? "Unknown",
                    IsManaged = entity.GetAttributeValue<bool>("ismanaged")
                });
            }

            return list.OrderBy(s => s.ToString(), StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        private List<EntityMetadataSummary> _entitiesCache;

        public List<EntityMetadataSummary> GetEntities(IOrganizationService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (_entitiesCache != null && _entitiesCache.Count > 0)
                return _entitiesCache;

            try
            {
                RetrieveAllEntitiesResponse response = null;
                try
                {
                    var request = new RetrieveAllEntitiesRequest
                    {
                        EntityFilters = EntityFilters.Entity,
                        RetrieveAsIfPublished = true
                    };
                    response = (RetrieveAllEntitiesResponse)service.Execute(request);
                }
                catch
                {
                    var request = new RetrieveAllEntitiesRequest
                    {
                        EntityFilters = EntityFilters.Entity,
                        RetrieveAsIfPublished = false
                    };
                    response = (RetrieveAllEntitiesResponse)service.Execute(request);
                }

                if (response?.EntityMetadata != null && response.EntityMetadata.Length > 0)
                {
                    var list = new List<EntityMetadataSummary>();
                    foreach (var meta in response.EntityMetadata)
                    {
                        string displayName = meta.DisplayName?.UserLocalizedLabel?.Label;
                        if (string.IsNullOrWhiteSpace(displayName) && meta.DisplayName?.LocalizedLabels != null && meta.DisplayName.LocalizedLabels.Count > 0)
                        {
                            displayName = meta.DisplayName.LocalizedLabels[0].Label;
                        }
                        if (string.IsNullOrWhiteSpace(displayName))
                        {
                            displayName = meta.SchemaName ?? meta.LogicalName;
                        }

                        list.Add(new EntityMetadataSummary
                        {
                            MetadataId = meta.MetadataId,
                            LogicalName = meta.LogicalName,
                            DisplayName = displayName,
                            ObjectTypeCode = meta.ObjectTypeCode
                        });
                    }

                    _entitiesCache = list.OrderBy(e => e.ToString()).ToList();
                    return _entitiesCache;
                }
            }
            catch
            {
                // Fallback paginado completo mediante consulta a la entidad 'entity' si la llamada de metadatos falla
            }

            try
            {
                var list = new List<EntityMetadataSummary>();
                var query = new QueryExpression("entity")
                {
                    ColumnSet = new ColumnSet("name", "logicalname", "physicalname", "objecttypecode"),
                    Orders = { new OrderExpression("name", OrderType.Ascending) },
                    PageInfo = new PagingInfo { Count = 5000, PageNumber = 1 }
                };

                while (true)
                {
                    var result = service.RetrieveMultiple(query);
                    if (result?.Entities != null && result.Entities.Count > 0)
                    {
                        foreach (var e in result.Entities)
                        {
                            string logName = e.GetAttributeValue<string>("logicalname") ?? e.GetAttributeValue<string>("name");
                            string dispName = e.GetAttributeValue<string>("name") ?? logName;
                            list.Add(new EntityMetadataSummary
                            {
                                MetadataId = e.Id,
                                LogicalName = logName,
                                DisplayName = dispName,
                                ObjectTypeCode = e.GetAttributeValue<int?>("objecttypecode")
                            });
                        }

                        if (result.MoreRecords)
                        {
                            query.PageInfo.PageNumber++;
                            query.PageInfo.PagingCookie = result.PagingCookie;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                _entitiesCache = list.OrderBy(e => e.ToString()).ToList();
                return _entitiesCache;
            }
            catch
            {
                return new List<EntityMetadataSummary>();
            }
        }

        public static readonly Dictionary<int, string> GlobalComponentTypeLabels = new Dictionary<int, string>();

        public static void EnsureGlobalComponentTypeLabels(IOrganizationService service)
        {
            if (GlobalComponentTypeLabels.Count > 0 || service == null) return;
            try
            {
                var req = new RetrieveOptionSetRequest { Name = "componenttype" };
                var resp = (RetrieveOptionSetResponse)service.Execute(req);
                if (resp?.OptionSetMetadata is OptionSetMetadata osMeta && osMeta.Options != null)
                {
                    foreach (var opt in osMeta.Options)
                    {
                        if (opt.Value.HasValue && !string.IsNullOrWhiteSpace(opt.Label?.UserLocalizedLabel?.Label))
                        {
                            GlobalComponentTypeLabels[opt.Value.Value] = opt.Label.UserLocalizedLabel.Label;
                        }
                    }
                }
            }
            catch { }
        }

        public (string ComponentTypeName, string EntityName) GetComponentTypeDetails(int componentType, IOrganizationService service = null)
        {
            if (ComponentTypes.ContainsKey(componentType))
            {
                return (ComponentTypes[componentType].TypeName, ComponentTypes[componentType].EntityLogicalName);
            }

            if (GlobalComponentTypeLabels.TryGetValue(componentType, out string cachedLabel))
            {
                return (cachedLabel, string.Empty);
            }

            if (service != null)
            {
                EnsureGlobalComponentTypeLabels(service);
                if (GlobalComponentTypeLabels.TryGetValue(componentType, out string foundLabel))
                {
                    return (foundLabel, string.Empty);
                }
            }

            return ("Unknown", string.Empty);
        }

        public ComponentTypeMetadata GetComponentTypeMetadata(int componentType)
        {
            return ComponentTypes.ContainsKey(componentType)
                ? ComponentTypes[componentType]
                : null;
        }

        private Dictionary<Guid, (string DisplayName, string LogicalName)> _entityMetadataCache = null;
        private Dictionary<Guid, string> _attributeMetadataCache = null;
        private Dictionary<Guid, string> _relationshipMetadataCache = null;

        private void EnsureMetadataCaches(IOrganizationService service)
        {
            if (_entityMetadataCache != null) return;
            _entityMetadataCache = new Dictionary<Guid, (string, string)>();
            _attributeMetadataCache = new Dictionary<Guid, string>();
            _relationshipMetadataCache = new Dictionary<Guid, string>();

            try
            {
                var req = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity,
                    RetrieveAsIfPublished = true
                };
                var resp = (RetrieveAllEntitiesResponse)service.Execute(req);
                if (resp?.EntityMetadata != null)
                {
                    foreach (var meta in resp.EntityMetadata)
                    {
                        if (meta.MetadataId.HasValue)
                        {
                            string disp = meta.DisplayName?.UserLocalizedLabel?.Label ?? meta.LogicalName;
                            _entityMetadataCache[meta.MetadataId.Value] = (disp, meta.LogicalName);
                        }
                    }
                }
            }
            catch
            {
                // Fallback
            }
        }

        public string GetComponentName(IOrganizationService service, string entityLogicalName, Guid objectId, int componentType = 0)
        {
            if (service == null || objectId == Guid.Empty)
                return "Unknown";

            // 1. Entity / Table (componentType 1)
            if (componentType == 1 || entityLogicalName == "entity")
            {
                EnsureMetadataCaches(service);
                if (_entityMetadataCache != null && _entityMetadataCache.TryGetValue(objectId, out var entInfo))
                {
                    return $"{entInfo.DisplayName} ({entInfo.LogicalName})";
                }
                try
                {
                    var req = new RetrieveEntityRequest
                    {
                        MetadataId = objectId,
                        EntityFilters = EntityFilters.Entity,
                        RetrieveAsIfPublished = true
                    };
                    var resp = (RetrieveEntityResponse)service.Execute(req);
                    if (resp?.EntityMetadata != null)
                    {
                        string disp = resp.EntityMetadata.DisplayName?.UserLocalizedLabel?.Label ?? resp.EntityMetadata.LogicalName;
                        string log = resp.EntityMetadata.LogicalName;
                        if (_entityMetadataCache != null) _entityMetadataCache[objectId] = (disp, log);
                        return $"{disp} ({log})";
                    }
                }
                catch { }
            }

            // 2. Entity field / Attribute (componentType 2)
            if (componentType == 2 || entityLogicalName == "attribute")
            {
                if (_attributeMetadataCache != null && _attributeMetadataCache.TryGetValue(objectId, out var cachedAttr))
                {
                    return cachedAttr;
                }
                if (!string.IsNullOrEmpty(entityLogicalName) && entityLogicalName != "attribute")
                {
                    try
                    {
                        var req = new RetrieveAttributeRequest
                        {
                            EntityLogicalName = entityLogicalName,
                            MetadataId = objectId,
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveAttributeResponse)service.Execute(req);
                        if (resp?.AttributeMetadata != null)
                        {
                            string disp = resp.AttributeMetadata.DisplayName?.UserLocalizedLabel?.Label;
                            string logName = resp.AttributeMetadata.LogicalName;
                            string entityName = resp.AttributeMetadata.EntityLogicalName ?? entityLogicalName;
                            string resolved = string.IsNullOrEmpty(disp) ? logName : $"{disp} ({logName})";
                            if (!string.IsNullOrEmpty(entityName))
                            {
                                resolved = $"{resolved} [{entityName}]";
                            }
                            if (_attributeMetadataCache != null) _attributeMetadataCache[objectId] = resolved;
                            return resolved;
                        }
                    }
                    catch { }
                }
            }

            // 3. Relationships (componentType 3 o 10)
            if (componentType == 3 || componentType == 10 || entityLogicalName == "relationship" || entityLogicalName == "entityrelationship")
            {
                if (_relationshipMetadataCache != null && _relationshipMetadataCache.TryGetValue(objectId, out var cachedRel))
                {
                    return cachedRel;
                }
                try
                {
                    var req = new RetrieveRelationshipRequest
                    {
                        MetadataId = objectId,
                        RetrieveAsIfPublished = true
                    };
                    var resp = (RetrieveRelationshipResponse)service.Execute(req);
                    if (resp?.RelationshipMetadata != null)
                    {
                        string schemaName = resp.RelationshipMetadata.SchemaName;
                        if (!string.IsNullOrEmpty(schemaName))
                        {
                            if (_relationshipMetadataCache != null) _relationshipMetadataCache[objectId] = schemaName;
                            return schemaName;
                        }
                    }
                }
                catch { }
            }

            // 4. OptionSet / Choice (componentType 9)
            if (componentType == 9 || entityLogicalName == "optionset")
            {
                try
                {
                    var req = new RetrieveOptionSetRequest
                    {
                        MetadataId = objectId,
                        RetrieveAsIfPublished = true
                    };
                    var resp = (RetrieveOptionSetResponse)service.Execute(req);
                    if (resp?.OptionSetMetadata != null)
                    {
                        string disp = resp.OptionSetMetadata.DisplayName?.UserLocalizedLabel?.Label ?? resp.OptionSetMetadata.Name;
                        return disp;
                    }
                }
                catch { }
            }

            // 5. Entity Key / Alternate Key (componentType 14)
            if (componentType == 14 || entityLogicalName == "entitykey")
            {
                try
                {
                    var req = new RetrieveEntityKeyRequest
                    {
                        MetadataId = objectId,
                        RetrieveAsIfPublished = true
                    };
                    var resp = (RetrieveEntityKeyResponse)service.Execute(req);
                    if (resp?.EntityKeyMetadata != null)
                    {
                        string disp = resp.EntityKeyMetadata.DisplayName?.UserLocalizedLabel?.Label ?? resp.EntityKeyMetadata.SchemaName ?? resp.EntityKeyMetadata.LogicalName;
                        string keyCols = resp.EntityKeyMetadata.KeyAttributes != null ? string.Join(", ", resp.EntityKeyMetadata.KeyAttributes) : "";
                        string entName = resp.EntityKeyMetadata.EntityLogicalName;
                        string res = !string.IsNullOrEmpty(keyCols) ? $"{disp} [{keyCols}]" : disp;
                        return !string.IsNullOrEmpty(entName) ? $"{res} [{entName}]" : res;
                    }
                }
                catch { }
            }

            // 6. Form / Entity Form (componentType 24, 60)
            if (componentType == 24 || componentType == 60 || entityLogicalName == "systemform")
            {
                try
                {
                    Entity form = service.Retrieve("systemform", objectId, new ColumnSet("name", "objecttypecode"));
                    if (form != null)
                    {
                        string name = form.GetAttributeValue<string>("name");
                        string otc = form.GetAttributeValue<string>("objecttypecode");
                        return !string.IsNullOrEmpty(otc) ? $"{name} ({otc})" : name;
                    }
                }
                catch { }
            }

            // 7. View (componentType 26)
            if (componentType == 26 || entityLogicalName == "savedquery")
            {
                try
                {
                    Entity view = service.Retrieve("savedquery", objectId, new ColumnSet("name", "returnedtypecode"));
                    if (view != null)
                    {
                        string name = view.GetAttributeValue<string>("name");
                        string rtc = view.GetAttributeValue<string>("returnedtypecode");
                        return !string.IsNullOrEmpty(rtc) ? $"{name} ({rtc})" : name;
                    }
                }
                catch { }
            }

            // 8. Charts / Visualizations (componentType 59)
            if (componentType == 59 || entityLogicalName == "savedqueryvisualization")
            {
                try
                {
                    Entity chart = service.Retrieve("savedqueryvisualization", objectId, new ColumnSet("name", "primaryentitytypecode"));
                    if (chart != null)
                    {
                        string name = chart.GetAttributeValue<string>("name");
                        string petc = chart.GetAttributeValue<string>("primaryentitytypecode");
                        return !string.IsNullOrEmpty(petc) ? $"{name} ({petc})" : name;
                    }
                }
                catch { }
            }

            // 9. Hierarchy Rule (componentType 65)
            if (componentType == 65 || entityLogicalName == "hierarchyrule")
            {
                try
                {
                    Entity rule = service.Retrieve("hierarchyrule", objectId, new ColumnSet("name", "primaryentityname"));
                    if (rule != null)
                    {
                        string name = rule.GetAttributeValue<string>("name");
                        string pen = rule.GetAttributeValue<string>("primaryentityname");
                        return !string.IsNullOrEmpty(pen) ? $"{name} ({pen})" : name;
                    }
                }
                catch { }
            }

            // 10. Entity Map (componentType 46)
            if (componentType == 46 || entityLogicalName == "entitymap")
            {
                try
                {
                    Entity em = service.Retrieve("entitymap", objectId, new ColumnSet("sourceentityname", "targetentityname"));
                    if (em != null)
                    {
                        string src = em.GetAttributeValue<string>("sourceentityname");
                        string tgt = em.GetAttributeValue<string>("targetentityname");
                        return $"{src} -> {tgt}";
                    }
                }
                catch { }
            }

            // 11. Attribute Map (componentType 47)
            if (componentType == 47 || entityLogicalName == "attributemap")
            {
                try
                {
                    Entity am = service.Retrieve("attributemap", objectId, new ColumnSet("sourceattributename", "targetattributename"));
                    if (am != null)
                    {
                        string src = am.GetAttributeValue<string>("sourceattributename");
                        string tgt = am.GetAttributeValue<string>("targetattributename");
                        return $"{src} -> {tgt}";
                    }
                }
                catch { }
            }

            // 12. Field Permission (componentType 71)
            if (componentType == 71 || entityLogicalName == "fieldpermission")
            {
                try
                {
                    Entity fp = service.Retrieve("fieldpermission", objectId, new ColumnSet("entityname", "attributelogicalname"));
                    if (fp != null)
                    {
                        string ent = fp.GetAttributeValue<string>("entityname");
                        string attr = fp.GetAttributeValue<string>("attributelogicalname");
                        return $"{attr} [{ent}]";
                    }
                }
                catch { }
            }

            // 13. Deterministic metadata resolution & CRM Table lookup
            var meta = GetComponentTypeMetadata(componentType);
            string targetEntity = !string.IsNullOrEmpty(entityLogicalName) 
                ? entityLogicalName 
                : (meta?.EntityLogicalName ?? GetComponentTypeDetails(componentType).EntityName);

            if (!string.IsNullOrEmpty(targetEntity) && targetEntity != "entity" && targetEntity != "attribute" && targetEntity != "relationship" && targetEntity != "entityrelationship" && targetEntity != "entitykey")
            {
                try
                {
                    Entity entity = service.Retrieve(targetEntity, objectId, new ColumnSet(true));
                    if (entity != null)
                    {
                        if (targetEntity == "workflow" || componentType == 29)
                        {
                            string wfName = entity.GetAttributeValue<string>("name");
                            int cat = entity.Contains("category") && entity["category"] is OptionSetValue osv ? osv.Value : 0;
                            return FormatWorkflowName(wfName ?? $"Workflow {objectId}", cat);
                        }

                        // 1. Try PrimaryNameAttribute from metadata
                        if (meta != null && !string.IsNullOrEmpty(meta.PrimaryNameAttribute) && entity.Contains(meta.PrimaryNameAttribute))
                        {
                            var val = entity[meta.PrimaryNameAttribute];
                            if (val is string str && !string.IsNullOrWhiteSpace(str))
                                return str;
                            if (val is EntityReference er && !string.IsNullOrWhiteSpace(er.Name))
                                return er.Name;
                        }

                        // 2. Try FallbackNameAttributes from metadata
                        if (meta != null && meta.FallbackNameAttributes != null)
                        {
                            foreach (var attr in meta.FallbackNameAttributes)
                            {
                                if (entity.Contains(attr))
                                {
                                    var val = entity[attr];
                                    if (val is string str && !string.IsNullOrWhiteSpace(str))
                                        return str;
                                    if (val is EntityReference er && !string.IsNullOrWhiteSpace(er.Name))
                                        return er.Name;
                                }
                            }
                        }

                        // 3. Try ParentEntityAttribute if present
                        if (meta != null && !string.IsNullOrEmpty(meta.ParentEntityAttribute) && entity.Contains(meta.ParentEntityAttribute))
                        {
                            var val = entity[meta.ParentEntityAttribute];
                            if (val is EntityReference er && !string.IsNullOrWhiteSpace(er.Name))
                                return er.Name;
                        }

                        // 4. Common standard attributes
                        foreach (var attr in new[] { "displayname", "name", "title", "subject", "friendlyname", "schemaname", "uniquename", "typename", "connectionreferencedisplayname", "connectionreferencename", "sitemapname", "sitemapnameunique", "filename", "msdyn_name", "value" })
                        {
                            if (entity.Contains(attr))
                            {
                                var val = entity[attr];
                                if (val is string strVal && !string.IsNullOrWhiteSpace(strVal))
                                {
                                    return strVal;
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            // 14. Universal fallback: Query msdyn_solutioncomponentsummary by objectid
            try
            {
                var sq = new QueryExpression("msdyn_solutioncomponentsummary")
                {
                    ColumnSet = new ColumnSet(
                        "msdyn_displayname",
                        "msdyn_name",
                        "msdyn_schemaname",
                        "msdyn_componentlogicalname",
                        "msdyn_primaryentityname",
                        "msdyn_componenttypename",
                        "msdyn_componenttype"
                    ),
                    TopCount = 1
                };
                sq.Criteria.AddCondition("msdyn_objectid", ConditionOperator.Equal, objectId);
                var sRes = service.RetrieveMultiple(sq);
                if (sRes != null && sRes.Entities != null && sRes.Entities.Count > 0)
                {
                    var s = sRes.Entities[0];
                    string n = s.GetAttributeValue<string>("msdyn_displayname")
                        ?? s.GetAttributeValue<string>("msdyn_name")
                        ?? s.GetAttributeValue<string>("msdyn_schemaname")
                        ?? s.GetAttributeValue<string>("msdyn_componentlogicalname");

                    if (!string.IsNullOrWhiteSpace(n))
                    {
                        string pEnt = s.GetAttributeValue<string>("msdyn_primaryentityname");
                        int cType = GetOptionSetAttribute(s, "msdyn_componenttype");
                        if (cType == 2 && !string.IsNullOrWhiteSpace(pEnt))
                        {
                            return $"{n} [{pEnt}]";
                        }
                        if (cType == 29)
                        {
                            int cat = s.Contains("msdyn_workflowcategory") && s["msdyn_workflowcategory"] is OptionSetValue osv ? osv.Value : 0;
                            return FormatWorkflowName(n, cat);
                        }
                        return n;
                    }
                }
            }
            catch { }

            LogUnresolvedComponent(objectId, componentType, targetEntity);
            return $"Unnamed ({objectId})";
        }

        private static void LogUnresolvedComponent(Guid objectId, int componentType, string entityName)
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".runtime");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string filePath = Path.Combine(dir, "unresolved_components.log");
                string logLine = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss UTC}] ObjectId={objectId} | ComponentType={componentType} | EntityLogicalName={entityName}{Environment.NewLine}";
                File.AppendAllText(filePath, logLine);
            }
            catch { }
        }

        public List<ComponentItem> GetComponentsByType(IOrganizationService service, int componentType, string entityLogicalName = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var details = GetComponentTypeDetails(componentType);
            var list = new List<ComponentItem>();

            try
            {
                if (componentType == 1) // Entity / Table: Cargar todas las tablas con formato homogéneo
                {
                    var allEntities = GetEntities(service);
                    foreach (var ent in allEntities)
                    {
                        list.Add(new ComponentItem
                        {
                            Id = ent.MetadataId ?? Guid.Empty,
                            Name = ent.ToString(),
                            ComponentType = 1,
                            ComponentTypeName = "Entity",
                            EntityLogicalName = ent.LogicalName
                        });
                    }
                    return list.OrderBy(c => c.Name).ToList();
                }
                else if (componentType == 2) // Entity field (attribute)
                {
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        var req = new RetrieveEntityRequest
                        {
                            LogicalName = entityLogicalName,
                            EntityFilters = EntityFilters.Attributes,
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveEntityResponse)service.Execute(req);
                        if (resp?.EntityMetadata?.Attributes != null)
                        {
                            foreach (var attr in resp.EntityMetadata.Attributes.OrderBy(a => a.DisplayName?.UserLocalizedLabel?.Label ?? a.LogicalName))
                            {
                                string displayName = attr.DisplayName?.UserLocalizedLabel?.Label;
                                string name = string.IsNullOrEmpty(displayName)
                                    ? attr.LogicalName
                                    : $"{displayName} ({attr.LogicalName})";

                                list.Add(new ComponentItem
                                {
                                    Id = attr.MetadataId ?? Guid.Empty,
                                    Name = name,
                                    ComponentType = 2,
                                    ComponentTypeName = "Entity field",
                                    EntityLogicalName = entityLogicalName
                                });
                            }
                        }
                    }
                }
                else if (componentType == 3) // Relationship (1:N, N:1)
                {
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        var req = new RetrieveEntityRequest
                        {
                            LogicalName = entityLogicalName,
                            EntityFilters = EntityFilters.Relationships,
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveEntityResponse)service.Execute(req);
                        var seenIds = new HashSet<Guid>();

                        if (resp?.EntityMetadata?.OneToManyRelationships != null)
                        {
                            foreach (var rel in resp.EntityMetadata.OneToManyRelationships.OrderBy(r => r.SchemaName))
                            {
                                Guid relId = rel.MetadataId ?? Guid.Empty;
                                if (relId != Guid.Empty && !seenIds.Add(relId)) continue;

                                string name = $"{rel.SchemaName} (1:N: {rel.ReferencedEntity} -> {rel.ReferencingEntity}.{rel.ReferencingAttribute})";
                                list.Add(new ComponentItem
                                {
                                    Id = relId,
                                    Name = name,
                                    ComponentType = 3,
                                    ComponentTypeName = "Relationship",
                                    EntityLogicalName = entityLogicalName
                                });
                            }
                        }

                        if (resp?.EntityMetadata?.ManyToOneRelationships != null)
                        {
                            foreach (var rel in resp.EntityMetadata.ManyToOneRelationships.OrderBy(r => r.SchemaName))
                            {
                                Guid relId = rel.MetadataId ?? Guid.Empty;
                                if (relId != Guid.Empty && !seenIds.Add(relId)) continue;

                                string name = $"{rel.SchemaName} (N:1: {rel.ReferencingEntity}.{rel.ReferencingAttribute} -> {rel.ReferencedEntity})";
                                list.Add(new ComponentItem
                                {
                                    Id = relId,
                                    Name = name,
                                    ComponentType = 3,
                                    ComponentTypeName = "Relationship",
                                    EntityLogicalName = entityLogicalName
                                });
                            }
                        }
                    }
                }
                else if (componentType == 10) // Entity Relationship (N:N)
                {
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        var req = new RetrieveEntityRequest
                        {
                            LogicalName = entityLogicalName,
                            EntityFilters = EntityFilters.Relationships,
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveEntityResponse)service.Execute(req);
                        if (resp?.EntityMetadata?.ManyToManyRelationships != null)
                        {
                            foreach (var rel in resp.EntityMetadata.ManyToManyRelationships.OrderBy(r => r.SchemaName))
                            {
                                string name = $"{rel.SchemaName} (N:N: {rel.Entity1LogicalName} <-> {rel.Entity2LogicalName})";
                                list.Add(new ComponentItem
                                {
                                    Id = rel.MetadataId ?? Guid.Empty,
                                    Name = name,
                                    ComponentType = 10,
                                    ComponentTypeName = "Entity Relationship",
                                    EntityLogicalName = entityLogicalName
                                });
                            }
                        }
                    }
                }
                else if (componentType == 14) // Entity Key / Alternate Key
                {
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        var req = new RetrieveEntityRequest
                        {
                            LogicalName = entityLogicalName,
                            EntityFilters = EntityFilters.Entity,
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveEntityResponse)service.Execute(req);
                        if (resp?.EntityMetadata?.Keys != null)
                        {
                            foreach (var key in resp.EntityMetadata.Keys.OrderBy(k => k.SchemaName ?? k.LogicalName))
                            {
                                string disp = key.DisplayName?.UserLocalizedLabel?.Label ?? key.SchemaName ?? key.LogicalName;
                                string keyCols = key.KeyAttributes != null ? string.Join(", ", key.KeyAttributes) : "";
                                string name = !string.IsNullOrEmpty(keyCols) ? $"{disp} [{keyCols}]" : disp;

                                list.Add(new ComponentItem
                                {
                                    Id = key.MetadataId ?? Guid.Empty,
                                    Name = name,
                                    ComponentType = 14,
                                    ComponentTypeName = "Entity Key",
                                    EntityLogicalName = entityLogicalName
                                });
                            }
                        }
                    }
                }
                else if (componentType == 24 || componentType == 60) // Form (systemform)
                {
                    var query = new QueryExpression("systemform")
                    {
                        ColumnSet = new ColumnSet("formid", "name", "objecttypecode", "type"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        query.Criteria.AddCondition("objecttypecode", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = e.GetAttributeValue<string>("name") ?? "Unnamed Form";
                        string otc = e.GetAttributeValue<string>("objecttypecode");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = !string.IsNullOrEmpty(otc) ? $"{name} ({otc})" : name,
                            ComponentType = componentType,
                            ComponentTypeName = details.ComponentTypeName,
                            EntityLogicalName = otc
                        });
                    }
                }
                else if (componentType == 26) // View (savedquery)
                {
                    var query = new QueryExpression("savedquery")
                    {
                        ColumnSet = new ColumnSet("savedqueryid", "name", "returnedtypecode"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        query.Criteria.AddCondition("returnedtypecode", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = e.GetAttributeValue<string>("name") ?? "Unnamed View";
                        string rtc = e.GetAttributeValue<string>("returnedtypecode");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = !string.IsNullOrEmpty(rtc) ? $"{name} ({rtc})" : name,
                            ComponentType = 26,
                            ComponentTypeName = "View",
                            EntityLogicalName = rtc
                        });
                    }
                }
                else if (componentType == 59) // Charts (savedqueryvisualization)
                {
                    var query = new QueryExpression("savedqueryvisualization")
                    {
                        ColumnSet = new ColumnSet("savedqueryvisualizationid", "name", "primaryentitytypecode"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        query.Criteria.AddCondition("primaryentitytypecode", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = e.GetAttributeValue<string>("name") ?? "Unnamed Chart";
                        string petc = e.GetAttributeValue<string>("primaryentitytypecode");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = !string.IsNullOrEmpty(petc) ? $"{name} ({petc})" : name,
                            ComponentType = 59,
                            ComponentTypeName = "Charts",
                            EntityLogicalName = petc
                        });
                    }
                }
                else if (componentType == 65) // Hierarchy Rule (hierarchyrule)
                {
                    var query = new QueryExpression("hierarchyrule")
                    {
                        ColumnSet = new ColumnSet("hierarchyruleid", "name", "primaryentityname"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        query.Criteria.AddCondition("primaryentityname", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = e.GetAttributeValue<string>("name") ?? "Unnamed Hierarchy Rule";
                        string pen = e.GetAttributeValue<string>("primaryentityname");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = !string.IsNullOrEmpty(pen) ? $"{name} ({pen})" : name,
                            ComponentType = 65,
                            ComponentTypeName = "Hierarchy Rule",
                            EntityLogicalName = pen
                        });
                    }
                }
                else if (componentType == 46) // Entity Map (entitymap)
                {
                    var query = new QueryExpression("entitymap")
                    {
                        ColumnSet = new ColumnSet("entitymapid", "sourceentityname", "targetentityname"),
                        Orders = { new OrderExpression("sourceentityname", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        var filter = query.Criteria.AddFilter(LogicalOperator.Or);
                        filter.AddCondition("sourceentityname", ConditionOperator.Equal, entityLogicalName);
                        filter.AddCondition("targetentityname", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string src = e.GetAttributeValue<string>("sourceentityname");
                        string tgt = e.GetAttributeValue<string>("targetentityname");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = $"{src} -> {tgt}",
                            ComponentType = 46,
                            ComponentTypeName = "Entity Map",
                            EntityLogicalName = entityLogicalName
                        });
                    }
                }
                else if (componentType == 47) // Attribute Map (attributemap)
                {
                    var query = new QueryExpression("attributemap")
                    {
                        ColumnSet = new ColumnSet("attributemapid", "sourceattributename", "targetattributename", "entitymapid"),
                        TopCount = 500
                    };
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string src = e.GetAttributeValue<string>("sourceattributename");
                        string tgt = e.GetAttributeValue<string>("targetattributename");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = $"{src} -> {tgt}",
                            ComponentType = 47,
                            ComponentTypeName = "Attribute Map",
                            EntityLogicalName = entityLogicalName
                        });
                    }
                }
                else if (componentType == 71) // Field Permission (fieldpermission)
                {
                    var query = new QueryExpression("fieldpermission")
                    {
                        ColumnSet = new ColumnSet("fieldpermissionid", "entityname", "attributelogicalname"),
                        Orders = { new OrderExpression("attributelogicalname", OrderType.Ascending) }
                    };
                    if (!string.IsNullOrEmpty(entityLogicalName))
                    {
                        query.Criteria.AddCondition("entityname", ConditionOperator.Equal, entityLogicalName);
                    }
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string ent = e.GetAttributeValue<string>("entityname");
                        string attr = e.GetAttributeValue<string>("attributelogicalname");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = $"{attr} ({ent})",
                            ComponentType = 71,
                            ComponentTypeName = "Field Permission",
                            EntityLogicalName = ent
                        });
                    }
                }
                else if (componentType == 9) // Option Set / Choice (global optionsets)
                {
                    try
                    {
                        var req = new RetrieveAllOptionSetsRequest
                        {
                            RetrieveAsIfPublished = true
                        };
                        var resp = (RetrieveAllOptionSetsResponse)service.Execute(req);
                        if (resp?.OptionSetMetadata != null)
                        {
                            foreach (var os in resp.OptionSetMetadata.OrderBy(o => o.DisplayName?.UserLocalizedLabel?.Label ?? o.Name))
                            {
                                string disp = os.DisplayName?.UserLocalizedLabel?.Label;
                                string name = string.IsNullOrEmpty(disp) ? os.Name : $"{disp} ({os.Name})";
                                list.Add(new ComponentItem
                                {
                                    Id = os.MetadataId ?? Guid.Empty,
                                    Name = name,
                                    ComponentType = 9,
                                    ComponentTypeName = "Option Set / Choice",
                                    EntityLogicalName = null
                                });
                            }
                        }
                    }
                    catch { }
                }
                else if (componentType == 29) // Workflow / Cloud Flow
                {
                    int? catFilter = null;
                    if (!string.IsNullOrEmpty(entityLogicalName) && int.TryParse(entityLogicalName, out int parsedCat))
                    {
                        catFilter = parsedCat;
                    }
                    return GetWorkflowsByCategory(service, catFilter);
                }
                else if (componentType == 20) // Role (Filtrar por unidad de negocio principal y deduplicar por nombre)
                {
                    var query = new QueryExpression("role")
                    {
                        ColumnSet = new ColumnSet("roleid", "name", "businessunitid"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };

                    // Filtrar por la unidad de negocio principal (donde parentbusinessunitid es null)
                    var linkBu = query.AddLink("businessunit", "businessunitid", "businessunitid", JoinOperator.Inner);
                    linkBu.LinkCriteria.AddCondition("parentbusinessunitid", ConditionOperator.Null);

                    var result = service.RetrieveMultiple(query);
                    var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var e in result.Entities)
                    {
                        string roleName = e.GetAttributeValue<string>("name");
                        if (string.IsNullOrEmpty(roleName)) continue;

                        if (seenNames.Add(roleName))
                        {
                            list.Add(new ComponentItem
                            {
                                Id = e.Id,
                                Name = roleName,
                                ComponentType = 20,
                                ComponentTypeName = "Role",
                                EntityLogicalName = null
                            });
                        }
                    }
                }
                else if (componentType == 61) // Web Resource
                {
                    var query = new QueryExpression("webresource")
                    {
                        ColumnSet = new ColumnSet("webresourceid", "name", "displayname"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = e.GetAttributeValue<string>("displayname") ?? e.GetAttributeValue<string>("name");
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = name ?? "Unnamed Web Resource",
                            ComponentType = 61,
                            ComponentTypeName = "Web Resource",
                            EntityLogicalName = null
                        });
                    }
                }
                else if (!string.IsNullOrEmpty(details.EntityName))
                {
                    var query = new QueryExpression(details.EntityName)
                    {
                        ColumnSet = new ColumnSet(true),
                        TopCount = 500
                    };
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = null;
                        foreach (var attr in new[] { "title", "subject", "displayname", "name", "friendlyname", "schemaname", "uniquename", "typename", "connectionreferencedisplayname", "connectionreferencename", "filename" })
                        {
                            if (e.Contains(attr))
                            {
                                name = e.GetAttributeValue<string>(attr);
                                if (!string.IsNullOrEmpty(name)) break;
                            }
                        }

                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = name ?? $"Item {e.Id}",
                            ComponentType = componentType,
                            ComponentTypeName = details.ComponentTypeName,
                            EntityLogicalName = entityLogicalName
                        });
                    }
                }
            }
            catch
            {
                // Fallback generic
            }

            return list.OrderBy(c => c.Name ?? string.Empty, StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        public List<ComponentItem> GetWorkflowsByCategory(IOrganizationService service, int? category = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var query = new QueryExpression("workflow")
            {
                ColumnSet = new ColumnSet("workflowid", "name", "category"),
                Orders = { new OrderExpression("name", OrderType.Ascending) }
            };
            query.Criteria.AddCondition("type", ConditionOperator.Equal, 1); // Definition

            if (category.HasValue)
            {
                query.Criteria.AddCondition("category", ConditionOperator.Equal, category.Value);
            }

            var result = service.RetrieveMultiple(query);
            var list = new List<ComponentItem>();

            if (result != null && result.Entities != null)
            {
                foreach (var e in result.Entities)
                {
                    string rawName = e.GetAttributeValue<string>("name") ?? "Unnamed Workflow";
                    int cat = e.Contains("category") && e["category"] is OptionSetValue osv ? osv.Value : 0;
                    string formattedName = category.HasValue ? rawName : FormatWorkflowName(rawName, cat);

                    list.Add(new ComponentItem
                    {
                        Id = e.Id,
                        Name = formattedName,
                        ComponentType = 29,
                        ComponentTypeName = "Workflow / Cloud Flow",
                        EntityLogicalName = null
                    });
                }
            }

            return list.OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        private static Guid GetGuidAttribute(Entity entity, string attributeName)
        {
            if (entity == null || !entity.Contains(attributeName)) return Guid.Empty;
            var val = entity[attributeName];
            if (val is Guid g) return g;
            if (val is EntityReference er) return er.Id;
            if (val is string s && Guid.TryParse(s, out Guid parsed)) return parsed;
            return Guid.Empty;
        }

        private static int GetOptionSetAttribute(Entity entity, string attributeName)
        {
            if (entity == null || !entity.Contains(attributeName)) return 0;
            var val = entity[attributeName];
            if (val is OptionSetValue osv) return osv.Value;
            if (val is int i) return i;
            return 0;
        }

        private readonly Dictionary<Guid, string> _userCache = new Dictionary<Guid, string>();

        public string GetModifiedByName(IOrganizationService service, Entity sol, Dictionary<Guid, string> localUserCache = null)
        {
            if (sol == null) return "System";

            // 1. Check FormattedValues for modifiedby
            if (sol.FormattedValues != null && sol.FormattedValues.TryGetValue("modifiedby", out string formattedModBy) && !string.IsNullOrWhiteSpace(formattedModBy))
            {
                return formattedModBy;
            }

            // 2. Check AliasedValue from usr.fullname
            if (sol.Contains("usr.fullname"))
            {
                var aliased = sol.GetAttributeValue<AliasedValue>("usr.fullname");
                if (aliased != null && aliased.Value != null)
                {
                    string val = aliased.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(val))
                        return val;
                }
            }

            // 3. Check EntityReference Name
            var modByRef = sol.GetAttributeValue<EntityReference>("modifiedby");
            if (modByRef != null)
            {
                if (!string.IsNullOrWhiteSpace(modByRef.Name))
                    return modByRef.Name;

                // 4. Resolve via cache or service.Retrieve from systemuser
                if (modByRef.Id != Guid.Empty)
                {
                    if (localUserCache != null && localUserCache.TryGetValue(modByRef.Id, out string cachedName) && !string.IsNullOrWhiteSpace(cachedName))
                    {
                        return cachedName;
                    }
                    if (_userCache.TryGetValue(modByRef.Id, out string globalCachedName) && !string.IsNullOrWhiteSpace(globalCachedName))
                    {
                        return globalCachedName;
                    }

                    if (service != null)
                    {
                        try
                        {
                            var userEntity = service.Retrieve(modByRef.LogicalName ?? "systemuser", modByRef.Id, new ColumnSet("fullname"));
                            if (userEntity != null && userEntity.Contains("fullname"))
                            {
                                string fullName = userEntity.GetAttributeValue<string>("fullname");
                                if (!string.IsNullOrWhiteSpace(fullName))
                                {
                                    if (localUserCache != null) localUserCache[modByRef.Id] = fullName;
                                    _userCache[modByRef.Id] = fullName;
                                    return fullName;
                                }
                            }
                        }
                        catch
                        {
                            // Fallback if user cannot be retrieved
                        }
                    }
                }
            }

            // 5. Check createdby formatted value or reference as secondary fallback
            if (sol.FormattedValues != null && sol.FormattedValues.TryGetValue("createdby", out string formattedCreatedBy) && !string.IsNullOrWhiteSpace(formattedCreatedBy))
            {
                return formattedCreatedBy;
            }

            var createdByRef = sol.GetAttributeValue<EntityReference>("createdby");
            if (createdByRef != null && !string.IsNullOrWhiteSpace(createdByRef.Name))
            {
                return createdByRef.Name;
            }

            return "System";
        }

        public List<ComponentHistoryItem> GetSolutionsForComponent(IOrganizationService service, Guid objectId)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (objectId == Guid.Empty) return new List<ComponentHistoryItem>();

            var query = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("solutionid")
            };
            query.Criteria.AddCondition("objectid", ConditionOperator.Equal, objectId);

            var result = service.RetrieveMultiple(query);
            if (result == null || result.Entities.Count == 0)
                return new List<ComponentHistoryItem>();

            var history = new List<ComponentHistoryItem>();
            var userCache = new Dictionary<Guid, string>();

            foreach (var entity in result.Entities)
            {
                Guid solutionId = GetGuidAttribute(entity, "solutionid");

                try
                {
                    var solQuery = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "modifiedon", "installedon", "publisherid", "modifiedby", "createdby")
                    };
                    solQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solutionId);

                    var linkPub = solQuery.AddLink("publisher", "publisherid", "publisherid", JoinOperator.LeftOuter);
                    linkPub.Columns = new ColumnSet("friendlyname");
                    linkPub.EntityAlias = "pub";

                    var linkUser = solQuery.AddLink("systemuser", "modifiedby", "systemuserid", JoinOperator.LeftOuter);
                    linkUser.Columns = new ColumnSet("fullname");
                    linkUser.EntityAlias = "usr";

                    var solRecords = service.RetrieveMultiple(solQuery);
                    if (solRecords != null && solRecords.Entities.Count > 0)
                    {
                        var sol = solRecords.Entities[0];
                        string pubName = null;
                        if (sol.Contains("pub.friendlyname"))
                        {
                            var aliasedPub = sol.GetAttributeValue<AliasedValue>("pub.friendlyname");
                            if (aliasedPub != null && aliasedPub.Value != null)
                            {
                                pubName = aliasedPub.Value.ToString();
                            }
                        }
                        if (string.IsNullOrWhiteSpace(pubName) && sol.FormattedValues != null && sol.FormattedValues.TryGetValue("publisherid", out string formattedPub) && !string.IsNullOrWhiteSpace(formattedPub))
                        {
                            pubName = formattedPub;
                        }
                        if (string.IsNullOrWhiteSpace(pubName))
                        {
                            pubName = sol.GetAttributeValue<EntityReference>("publisherid")?.Name ?? "Unknown";
                        }

                        string modUser = GetModifiedByName(service, sol, userCache);

                        history.Add(new ComponentHistoryItem
                        {
                            SolutionName = sol.GetAttributeValue<string>("friendlyname") ?? sol.GetAttributeValue<string>("uniquename"),
                            SolutionUniqueName = sol.GetAttributeValue<string>("uniquename"),
                            Version = sol.GetAttributeValue<string>("version"),
                            PublisherName = pubName ?? "Unknown",
                            ModifiedBy = modUser ?? "System",
                            IsManaged = sol.GetAttributeValue<bool>("ismanaged"),
                            CreatedOn = sol.GetAttributeValue<DateTime?>("createdon"),
                            ModifiedOn = sol.GetAttributeValue<DateTime?>("modifiedon"),
                            InstalledOn = sol.GetAttributeValue<DateTime?>("installedon")
                        });
                    }
                }
                catch
                {
                    // Ignore single solution errors
                }
            }

            // Ordenar cronológicamente (más recientes primero)
            return history
                .OrderByDescending(h => h.InstalledOn ?? h.ModifiedOn ?? h.CreatedOn ?? DateTime.MinValue)
                .ThenByDescending(h => h.Version)
                .ToList();
        }

        public List<SolutionOverlapItem> GetOverlappingSolutions(
            IOrganizationService service,
            Guid selectedSolutionId,
            IEnumerable<string> excludedPublishers = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (selectedSolutionId == Guid.Empty) return new List<SolutionOverlapItem>();

            var compQuery = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("componenttype", "objectid", "solutionid"),
                PageInfo = new PagingInfo { PageNumber = 1, PagingCookie = null }
            };
            compQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, selectedSolutionId);

            var solutionComponents = new List<Entity>();
            while (true)
            {
                var resp = service.RetrieveMultiple(compQuery);
                if (resp?.Entities != null)
                {
                    solutionComponents.AddRange(resp.Entities);
                }
                if (resp != null && resp.MoreRecords)
                {
                    compQuery.PageInfo.PageNumber++;
                    compQuery.PageInfo.PagingCookie = resp.PagingCookie;
                }
                else
                {
                    break;
                }
            }

            if (solutionComponents.Count == 0)
                return new List<SolutionOverlapItem>();

            var overlaps = new List<SolutionOverlapItem>();
            var compNameCache = new Dictionary<Guid, string>();
            var compTypeCache = new Dictionary<Guid, string>();
            var userCache = new Dictionary<Guid, string>();

            EnsureGlobalComponentTypeLabels(service);

            // Pre-populate component names using msdyn_solutioncomponentsummary if available in this Dataverse environment
            try
            {
                var summaryQuery = new QueryExpression("msdyn_solutioncomponentsummary")
                {
                    ColumnSet = new ColumnSet(
                        "msdyn_objectid",
                        "msdyn_displayname",
                        "msdyn_name",
                        "msdyn_schemaname",
                        "msdyn_componentlogicalname",
                        "msdyn_componenttype",
                        "msdyn_componenttypename",
                        "msdyn_primaryentityname",
                        "msdyn_workflowcategory"
                    ),
                    PageInfo = new PagingInfo { PageNumber = 1, PagingCookie = null }
                };
                summaryQuery.Criteria.AddCondition("msdyn_solutionid", ConditionOperator.Equal, selectedSolutionId);

                while (true)
                {
                    var summaryResults = service.RetrieveMultiple(summaryQuery);
                    if (summaryResults != null && summaryResults.Entities != null)
                    {
                        foreach (var summary in summaryResults.Entities)
                        {
                            Guid objId = GetGuidAttribute(summary, "msdyn_objectid");
                            if (objId != Guid.Empty)
                            {
                                string disp = summary.GetAttributeValue<string>("msdyn_displayname");
                                string rawName = summary.GetAttributeValue<string>("msdyn_name");
                                string schemaName = summary.GetAttributeValue<string>("msdyn_schemaname");
                                string logicalName = summary.GetAttributeValue<string>("msdyn_componentlogicalname");
                                string finalName = !string.IsNullOrWhiteSpace(disp) ? disp
                                    : (!string.IsNullOrWhiteSpace(rawName) ? rawName
                                    : (!string.IsNullOrWhiteSpace(schemaName) ? schemaName
                                    : logicalName));

                                int typeCode = GetOptionSetAttribute(summary, "msdyn_componenttype");
                                string typeName = summary.GetAttributeValue<string>("msdyn_componenttypename");
                                string primaryEnt = summary.GetAttributeValue<string>("msdyn_primaryentityname");

                                if (!string.IsNullOrWhiteSpace(typeName))
                                {
                                    compTypeCache[objId] = typeName;
                                    if (typeCode > 0)
                                    {
                                        GlobalComponentTypeLabels[typeCode] = typeName;
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(finalName))
                                {
                                    if (typeCode == 2 && !string.IsNullOrWhiteSpace(primaryEnt))
                                    {
                                        finalName = $"{finalName} [{primaryEnt}]";
                                    }
                                    else if (typeCode == 29)
                                    {
                                        int cat = summary.Contains("msdyn_workflowcategory") && summary["msdyn_workflowcategory"] is OptionSetValue osv ? osv.Value : 0;
                                        finalName = FormatWorkflowName(finalName, cat);
                                    }
                                    compNameCache[objId] = finalName;
                                }
                            }
                        }
                    }

                    if (summaryResults != null && summaryResults.MoreRecords)
                    {
                        summaryQuery.PageInfo.PageNumber++;
                        summaryQuery.PageInfo.PagingCookie = summaryResults.PagingCookie;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {
                // Environment does not support msdyn_solutioncomponentsummary; fallback to individual metadata resolution (R5)
            }

            foreach (var comp in solutionComponents)
            {
                Guid objectId = GetGuidAttribute(comp, "objectid");
                int compType = GetOptionSetAttribute(comp, "componenttype");

                var details = GetComponentTypeDetails(compType, service);
                if (!compNameCache.TryGetValue(objectId, out string compName))
                {
                    compName = GetComponentName(service, details.EntityName, objectId, compType);
                    compNameCache[objectId] = compName;
                }

                string displayCompType = details.ComponentTypeName;
                if (string.IsNullOrEmpty(displayCompType) || displayCompType == "Unknown")
                {
                    if (compTypeCache.TryGetValue(objectId, out string summaryType) && !string.IsNullOrWhiteSpace(summaryType))
                    {
                        displayCompType = summaryType;
                    }
                    else if (GlobalComponentTypeLabels.TryGetValue(compType, out string globalLabel) && !string.IsNullOrWhiteSpace(globalLabel))
                    {
                        displayCompType = globalLabel;
                    }
                    else
                    {
                        displayCompType = $"Type {compType}";
                    }
                }

                var relatedQuery = new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("solutionid")
                };
                relatedQuery.Criteria.AddCondition("objectid", ConditionOperator.Equal, objectId);
                relatedQuery.Criteria.AddCondition("solutionid", ConditionOperator.NotEqual, selectedSolutionId);

                var relatedResult = service.RetrieveMultiple(relatedQuery);
                if (relatedResult == null || relatedResult.Entities.Count == 0)
                    continue;

                foreach (var relComp in relatedResult.Entities)
                {
                    Guid otherSolutionId = GetGuidAttribute(relComp, "solutionid");

                    try
                    {
                        var solQuery = new QueryExpression("solution")
                        {
                            ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "modifiedon", "installedon", "publisherid", "modifiedby", "createdby")
                        };
                        solQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, otherSolutionId);

                        var linkPub = solQuery.AddLink("publisher", "publisherid", "publisherid", JoinOperator.LeftOuter);
                        linkPub.Columns = new ColumnSet("friendlyname");
                        linkPub.EntityAlias = "pub";

                        var linkUser = solQuery.AddLink("systemuser", "modifiedby", "systemuserid", JoinOperator.LeftOuter);
                        linkUser.Columns = new ColumnSet("fullname");
                        linkUser.EntityAlias = "usr";

                        var solRecords = service.RetrieveMultiple(solQuery);
                        if (solRecords != null && solRecords.Entities.Count > 0)
                        {
                            var sol = solRecords.Entities[0];
                            string pubName = null;
                            if (sol.Contains("pub.friendlyname"))
                            {
                                var aliasedPub = sol.GetAttributeValue<AliasedValue>("pub.friendlyname");
                                if (aliasedPub != null && aliasedPub.Value != null)
                                {
                                    pubName = aliasedPub.Value.ToString();
                                }
                            }
                            if (string.IsNullOrWhiteSpace(pubName) && sol.FormattedValues != null && sol.FormattedValues.TryGetValue("publisherid", out string formattedPub) && !string.IsNullOrWhiteSpace(formattedPub))
                            {
                                pubName = formattedPub;
                            }
                            if (string.IsNullOrWhiteSpace(pubName))
                            {
                                pubName = sol.GetAttributeValue<EntityReference>("publisherid")?.Name ?? "Unknown";
                            }

                            string modUser = GetModifiedByName(service, sol, userCache);

                            overlaps.Add(new SolutionOverlapItem
                            {
                                ComponentName = compName,
                                ComponentType = displayCompType,
                                ComponentId = objectId,
                                RelatedSolutionName = sol.GetAttributeValue<string>("friendlyname") ?? sol.GetAttributeValue<string>("uniquename"),
                                RelatedSolutionUniqueName = sol.GetAttributeValue<string>("uniquename"),
                                Version = sol.GetAttributeValue<string>("version"),
                                PublisherName = pubName ?? "Unknown",
                                ModifiedBy = modUser ?? "System",
                                IsManaged = sol.GetAttributeValue<bool>("ismanaged"),
                                CreatedOn = sol.GetAttributeValue<DateTime?>("createdon"),
                                ModifiedOn = sol.GetAttributeValue<DateTime?>("modifiedon"),
                                InstalledOn = sol.GetAttributeValue<DateTime?>("installedon")
                            });
                        }
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }

            return FilterByExcludedPublishers(overlaps, excludedPublishers);
        }

        public static List<SolutionOverlapItem> FilterByExcludedPublishers(
            IEnumerable<SolutionOverlapItem> items,
            IEnumerable<string> excludedPublishers)
        {
            if (items == null) return new List<SolutionOverlapItem>();
            if (excludedPublishers == null) return items.ToList();

            var excludedSet = new HashSet<string>(
                excludedPublishers.Where(p => !string.IsNullOrWhiteSpace(p)),
                StringComparer.OrdinalIgnoreCase
            );

            if (excludedSet.Count == 0) return items.ToList();

            return items.Where(item =>
                string.IsNullOrEmpty(item.PublisherName) || !excludedSet.Contains(item.PublisherName.Trim())
            ).ToList();
        }
    }
}
