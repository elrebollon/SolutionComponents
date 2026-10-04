using Microsoft.Xrm.Sdk;
using SolutionComponents.Models;
using SolutionComponents.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using XrmToolBox.Extensibility.Interfaces;

namespace SolutionComponents.Tests
{
    public class SolutionAuditServiceTests
    {
        public static void RunAllTests()
        {
            Test_FilterByExcludedPublishers_RemovesMatchingPublishers();
            Test_FilterByExcludedPublishers_CaseInsensitive();
            Test_FilterByExcludedPublishers_NullOrEmptyListReturnsAll();
            Test_GetComponentTypeDetails_KnownAndUnknownTypes();
            Test_GetComponentTypeDetails_ModernDataverseTypes();
            Test_GetComponentTypeDetails_EntityComponents();
            Test_IsEntityDependentComponent_IdentifiesCorrectTypes();
            Test_ComponentHistoryOrdering();
            Test_MefMetadata_ValidIconsAndProperties();
            Test_ModifiedByFieldSupport();
            Test_GetModifiedByName_ResolutionOrder();
            Test_SolutionSummaryOrdering_CaseInsensitive();
            Test_ComponentItemOrdering_CaseInsensitive();
            Test_ComboBoxConfiguration_SuggestAppendAndDropDown();
            Test_ComponentTypeMetadata_StructuredCatalog();
            Test_ProcessSubtypes_WorkflowCategories();
            Test_GetWorkflowsByCategory_FilteringAndPrefixing();
            Test_GetComponentName_DeterministicMetadataResolution();
            Test_GetOverlappingSolutions_WithSummaryAndFallback();
            Test_ResolveComponentTypes_WithGlobalLabelsAndSchemaNames();
            Test_MyPluginControl_ProcessCategoryDropdownSwitching();
            Test_PluginControl_Implements_IGitHubPlugin_And_IHelpPlugin();
            Console.WriteLine("All SolutionAuditServiceTests passed successfully!");
        }

        public static void Test_GetComponentTypeDetails_EntityComponents()
        {
            var service = new SolutionAuditService();

            var attr = service.GetComponentTypeDetails(2);
            if (attr.ComponentTypeName != "Entity field" || attr.EntityName != "attribute")
                throw new Exception($"Expected Entity field/attribute for type 2, got {attr.ComponentTypeName}/{attr.EntityName}");

            var rel = service.GetComponentTypeDetails(3);
            if (rel.ComponentTypeName != "Relationship" || rel.EntityName != "relationship")
                throw new Exception($"Expected Relationship/relationship for type 3, got {rel.ComponentTypeName}/{rel.EntityName}");

            var entRel = service.GetComponentTypeDetails(10);
            if (entRel.ComponentTypeName != "Entity Relationship" || entRel.EntityName != "entityrelationship")
                throw new Exception($"Expected Entity Relationship/entityrelationship for type 10, got {entRel.ComponentTypeName}/{entRel.EntityName}");

            var key = service.GetComponentTypeDetails(14);
            if (key.ComponentTypeName != "Entity Key" || key.EntityName != "entitykey")
                throw new Exception($"Expected Entity Key/entitykey for type 14, got {key.ComponentTypeName}/{key.EntityName}");

            var chart = service.GetComponentTypeDetails(59);
            if (chart.ComponentTypeName != "Charts" || chart.EntityName != "savedqueryvisualization")
                throw new Exception($"Expected Charts/savedqueryvisualization for type 59, got {chart.ComponentTypeName}/{chart.EntityName}");

            var hRule = service.GetComponentTypeDetails(65);
            if (hRule.ComponentTypeName != "Hierarchy Rule" || hRule.EntityName != "hierarchyrule")
                throw new Exception($"Expected Hierarchy Rule/hierarchyrule for type 65, got {hRule.ComponentTypeName}/{hRule.EntityName}");

            var entMap = service.GetComponentTypeDetails(46);
            if (entMap.ComponentTypeName != "Entity Map" || entMap.EntityName != "entitymap")
                throw new Exception($"Expected Entity Map/entitymap for type 46, got {entMap.ComponentTypeName}/{entMap.EntityName}");

            var attrMap = service.GetComponentTypeDetails(47);
            if (attrMap.ComponentTypeName != "Attribute Map" || attrMap.EntityName != "attributemap")
                throw new Exception($"Expected Attribute Map/attributemap for type 47, got {attrMap.ComponentTypeName}/{attrMap.EntityName}");

            var fPerm = service.GetComponentTypeDetails(71);
            if (fPerm.ComponentTypeName != "Field Permission" || fPerm.EntityName != "fieldpermission")
                throw new Exception($"Expected Field Permission/fieldpermission for type 71, got {fPerm.ComponentTypeName}/{fPerm.EntityName}");
        }

        public static void Test_IsEntityDependentComponent_IdentifiesCorrectTypes()
        {
            // Dependientes de entidad (R-4, G-2)
            if (!SolutionAuditService.IsEntityDependentComponent(2)) // Attribute/Field
                throw new Exception("Attribute (2) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(3)) // Relationship
                throw new Exception("Relationship (3) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(10)) // Entity Relationship (N:N)
                throw new Exception("Entity Relationship (10) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(14)) // Entity Key
                throw new Exception("Entity Key (14) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(24)) // Form
                throw new Exception("Form (24) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(26)) // View
                throw new Exception("View (26) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(46)) // Entity Map
                throw new Exception("Entity Map (46) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(47)) // Attribute Map
                throw new Exception("Attribute Map (47) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(59)) // Chart
                throw new Exception("Chart (59) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(60)) // System Form
                throw new Exception("System Form (60) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(65)) // Hierarchy Rule
                throw new Exception("Hierarchy Rule (65) must be entity dependent");

            if (!SolutionAuditService.IsEntityDependentComponent(71)) // Field Permission
                throw new Exception("Field Permission (71) must be entity dependent");

            // Globales (R-4, G-2)
            if (SolutionAuditService.IsEntityDependentComponent(20)) // Role
                throw new Exception("Role (20) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(29)) // Workflow / Cloud Flow
                throw new Exception("Workflow (29) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(61)) // Web Resource
                throw new Exception("Web Resource (61) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(80)) // Model-Driven App
                throw new Exception("Model-Driven App (80) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(300)) // Canvas App
                throw new Exception("Canvas App (300) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(380)) // Environment Variable
                throw new Exception("Environment Variable (380) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(973)) // Bot
                throw new Exception("Bot (973) must be global");

            if (SolutionAuditService.IsEntityDependentComponent(10020)) // Custom API
                throw new Exception("Custom API (10020) must be global");
        }

        public static void Test_GetComponentTypeDetails_ModernDataverseTypes()
        {
            var service = new SolutionAuditService();

            var appModule = service.GetComponentTypeDetails(80);
            if (appModule.ComponentTypeName != "Model-Driven App" || appModule.EntityName != "appmodule")
                throw new Exception($"Expected Model-Driven App/appmodule for type 80, got {appModule.ComponentTypeName}/{appModule.EntityName}");

            var connRef = service.GetComponentTypeDetails(10098);
            if (connRef.ComponentTypeName != "Connection Reference")
                throw new Exception($"Expected Connection Reference for type 10098, got {connRef.ComponentTypeName}");

            var customApi = service.GetComponentTypeDetails(10020);
            if (customApi.ComponentTypeName != "Custom API")
                throw new Exception($"Expected Custom API for type 10020, got {customApi.ComponentTypeName}");

            var bot = service.GetComponentTypeDetails(973);
            if (bot.ComponentTypeName != "Copilot / Bot")
                throw new Exception($"Expected Copilot / Bot for type 973, got {bot.ComponentTypeName}");

            var stagedOptionSet = service.GetComponentTypeDetails(10027);
            if (stagedOptionSet.ComponentTypeName != "Staged Option Set" || stagedOptionSet.EntityName != "stagedoptionset")
                throw new Exception($"Expected Staged Option Set/stagedoptionset for type 10027, got {stagedOptionSet.ComponentTypeName}/{stagedOptionSet.EntityName}");
        }

        public static void Test_ModifiedByFieldSupport()
        {
            var item = new SolutionOverlapItem { ModifiedBy = "Alberto Perez" };
            if (item.ModifiedBy != "Alberto Perez")
                throw new Exception("SolutionOverlapItem.ModifiedBy not set");

            var hist = new ComponentHistoryItem { ModifiedBy = "Alberto Perez" };
            if (hist.ModifiedBy != "Alberto Perez")
                throw new Exception("ComponentHistoryItem.ModifiedBy not set");
        }

        public static void Test_GetModifiedByName_ResolutionOrder()
        {
            var service = new SolutionAuditService();

            // Case 1: FormattedValues takes precedence
            var sol1 = new Entity("solution");
            sol1.FormattedValues["modifiedby"] = "Formatted User";
            sol1.Attributes["usr.fullname"] = new AliasedValue("usr", "fullname", "Aliased User");
            if (service.GetModifiedByName(null, sol1) != "Formatted User")
                throw new Exception("FormattedValues should take precedence");

            // Case 2: AliasedValue usr.fullname when FormattedValues not present
            var sol2 = new Entity("solution");
            sol2.Attributes["usr.fullname"] = new AliasedValue("usr", "fullname", "Aliased User");
            if (service.GetModifiedByName(null, sol2) != "Aliased User")
                throw new Exception("AliasedValue usr.fullname should be used");

            // Case 3: EntityReference.Name when AliasedValue is null/missing
            var sol3 = new Entity("solution");
            sol3.Attributes["usr.fullname"] = new AliasedValue("usr", "fullname", null);
            sol3.Attributes["modifiedby"] = new EntityReference("systemuser", Guid.NewGuid()) { Name = "Reference User" };
            if (service.GetModifiedByName(null, sol3) != "Reference User")
                throw new Exception("EntityReference.Name should be used when aliased value is null");

            // Case 4: Cache lookup by Guid
            var userId = Guid.NewGuid();
            var sol4 = new Entity("solution");
            sol4.Attributes["modifiedby"] = new EntityReference("systemuser", userId);
            var cache = new Dictionary<Guid, string> { { userId, "Cached User" } };
            if (service.GetModifiedByName(null, sol4, cache) != "Cached User")
                throw new Exception("Cached user should be used");

            // Case 5: Fallback to System
            var sol5 = new Entity("solution");
            if (service.GetModifiedByName(null, sol5) != "System")
                throw new Exception("Fallback should be System");
        }

        public static void Test_FilterByExcludedPublishers_RemovesMatchingPublishers()
        {
            var items = new List<SolutionOverlapItem>
            {
                new SolutionOverlapItem { ComponentName = "Account Form", PublisherName = "MicrosoftCorporation" },
                new SolutionOverlapItem { ComponentName = "Custom Role", PublisherName = "InternalDev" },
                new SolutionOverlapItem { ComponentName = "Contact View", PublisherName = "PartnerCorp" }
            };

            var excluded = new List<string> { "MicrosoftCorporation" };

            var filtered = SolutionAuditService.FilterByExcludedPublishers(items, excluded);

            if (filtered.Count != 2)
                throw new Exception($"Expected 2 items, got {filtered.Count}");

            if (filtered.Any(x => x.PublisherName == "MicrosoftCorporation"))
                throw new Exception("MicrosoftCorporation should have been filtered out");
        }

        public static void Test_FilterByExcludedPublishers_CaseInsensitive()
        {
            var items = new List<SolutionOverlapItem>
            {
                new SolutionOverlapItem { ComponentName = "Lead Entity", PublisherName = "microsoftcorporation" },
                new SolutionOverlapItem { ComponentName = "Custom Flow", PublisherName = "MyCompany" }
            };

            var excluded = new List<string> { "MICROSOFTCORPORATION" };

            var filtered = SolutionAuditService.FilterByExcludedPublishers(items, excluded);

            if (filtered.Count != 1)
                throw new Exception($"Expected 1 item, got {filtered.Count}");

            if (filtered[0].PublisherName != "MyCompany")
                throw new Exception($"Expected MyCompany, got {filtered[0].PublisherName}");
        }

        public static void Test_FilterByExcludedPublishers_NullOrEmptyListReturnsAll()
        {
            var items = new List<SolutionOverlapItem>
            {
                new SolutionOverlapItem { ComponentName = "Item 1", PublisherName = "PubA" },
                new SolutionOverlapItem { ComponentName = "Item 2", PublisherName = "PubB" }
            };

            var filteredWithNull = SolutionAuditService.FilterByExcludedPublishers(items, null);
            if (filteredWithNull.Count != 2)
                throw new Exception("Null excluded list should return all items");

            var filteredWithEmpty = SolutionAuditService.FilterByExcludedPublishers(items, new List<string>());
            if (filteredWithEmpty.Count != 2)
                throw new Exception("Empty excluded list should return all items");
        }

        public static void Test_GetComponentTypeDetails_KnownAndUnknownTypes()
        {
            var service = new SolutionAuditService();

            var entityType = service.GetComponentTypeDetails(1);
            if (entityType.ComponentTypeName != "Entity" || entityType.EntityName != "entity")
                throw new Exception($"Expected Entity/entity, got {entityType.ComponentTypeName}/{entityType.EntityName}");

            var formType = service.GetComponentTypeDetails(24);
            if (formType.ComponentTypeName != "Form" || formType.EntityName != "systemform")
                throw new Exception($"Expected Form/systemform, got {formType.ComponentTypeName}/{formType.EntityName}");

            var unknownType = service.GetComponentTypeDetails(9999);
            if (unknownType.ComponentTypeName != "Unknown")
                throw new Exception($"Expected Unknown, got {unknownType.ComponentTypeName}");
        }



        public static void Test_ComponentHistoryOrdering()
        {
            var history = new List<ComponentHistoryItem>
            {
                new ComponentHistoryItem { SolutionName = "Release 2025", InstalledOn = new DateTime(2025, 1, 1), Version = "1.0.0.0" },
                new ComponentHistoryItem { SolutionName = "Release 2026", InstalledOn = new DateTime(2026, 3, 1), Version = "2.0.0.0" },
                new ComponentHistoryItem { SolutionName = "Release 2024", InstalledOn = new DateTime(2024, 6, 1), Version = "0.9.0.0" }
            };

            var ordered = history
                .OrderByDescending(h => h.InstalledOn ?? h.CreatedOn ?? DateTime.MinValue)
                .ThenByDescending(h => h.Version)
                .ToList();

            if (ordered[0].SolutionName != "Release 2026")
                throw new Exception($"Expected Release 2026 as first item, got {ordered[0].SolutionName}");

            if (ordered[2].SolutionName != "Release 2024")
                throw new Exception($"Expected Release 2024 as last item, got {ordered[2].SolutionName}");
        }

        public static void Test_MefMetadata_ValidIconsAndProperties()
        {
            var attributes = typeof(MyPlugin).GetCustomAttributes(typeof(ExportMetadataAttribute), false)
                .Cast<ExportMetadataAttribute>()
                .ToDictionary(a => a.Name, a => a.Value);

            if (!attributes.ContainsKey("SmallImageBase64") || string.IsNullOrEmpty(attributes["SmallImageBase64"] as string))
                throw new Exception("SmallImageBase64 MEF metadata must not be null or empty");

            if (!attributes.ContainsKey("BigImageBase64") || string.IsNullOrEmpty(attributes["BigImageBase64"] as string))
                throw new Exception("BigImageBase64 MEF metadata must not be null or empty");

            if (!attributes.ContainsKey("Name") || (string)attributes["Name"] != "Solution Component Inspector")
                throw new Exception("Name MEF metadata must be 'Solution Component Inspector'");
        }

        public static void Test_SolutionSummaryOrdering_CaseInsensitive()
        {
            var solutions = new List<SolutionSummary>
            {
                new SolutionSummary { FriendlyName = "zebra solution", UniqueName = "zebra" },
                new SolutionSummary { FriendlyName = "Alpha Solution", UniqueName = "alpha" },
                new SolutionSummary { FriendlyName = "beta solution", UniqueName = "beta" },
                new SolutionSummary { FriendlyName = "", UniqueName = "gamma_no_friendly" }
            };

            var sorted = solutions.OrderBy(s => s.ToString(), StringComparer.CurrentCultureIgnoreCase).ToList();

            if (sorted[0].FriendlyName != "Alpha Solution")
                throw new Exception($"Expected 'Alpha Solution' first, got '{sorted[0].FriendlyName}'");

            if (sorted[1].FriendlyName != "beta solution")
                throw new Exception($"Expected 'beta solution' second, got '{sorted[1].FriendlyName}'");

            if (sorted[2].UniqueName != "gamma_no_friendly")
                throw new Exception($"Expected 'gamma_no_friendly' third, got '{sorted[2].UniqueName}'");

            if (sorted[3].FriendlyName != "zebra solution")
                throw new Exception($"Expected 'zebra solution' fourth, got '{sorted[3].FriendlyName}'");
        }

        public static void Test_ComponentItemOrdering_CaseInsensitive()
        {
            var components = new List<ComponentItem>
            {
                new ComponentItem { Id = Guid.NewGuid(), Name = "zip_code" },
                new ComponentItem { Id = Guid.NewGuid(), Name = "Account" },
                new ComponentItem { Id = Guid.NewGuid(), Name = "account_number" },
                new ComponentItem { Id = Guid.NewGuid(), Name = "Contact" }
            };

            var sorted = components.OrderBy(c => c.Name ?? string.Empty, StringComparer.CurrentCultureIgnoreCase).ToList();

            if (sorted[0].Name != "Account")
                throw new Exception($"Expected 'Account' first, got '{sorted[0].Name}'");

            if (sorted[1].Name != "account_number")
                throw new Exception($"Expected 'account_number' second, got '{sorted[1].Name}'");

            if (sorted[2].Name != "Contact")
                throw new Exception($"Expected 'Contact' third, got '{sorted[2].Name}'");

            if (sorted[3].Name != "zip_code")
                throw new Exception($"Expected 'zip_code' fourth, got '{sorted[3].Name}'");
        }

        public static void Test_ComboBoxConfiguration_SuggestAppendAndDropDown()
        {
            using (var control = new MyPluginControl())
            {
                var combos = new[]
                {
                    new { Name = "cbSolutions", Control = control.GetType().GetField("cbSolutions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox },
                    new { Name = "cbSolutionComponents", Control = control.GetType().GetField("cbSolutionComponents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox },
                    new { Name = "cbComponentTypes", Control = control.GetType().GetField("cbComponentTypes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox },
                    new { Name = "cbEntities", Control = control.GetType().GetField("cbEntities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox },
                    new { Name = "cbComponents", Control = control.GetType().GetField("cbComponents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox }
                };

                foreach (var item in combos)
                {
                    if (item.Control == null)
                        throw new Exception($"ComboBox {item.Name} not found in MyPluginControl");

                    if (item.Control.DropDownStyle != System.Windows.Forms.ComboBoxStyle.DropDown)
                        throw new Exception($"ComboBox {item.Name} must have DropDownStyle = DropDown, got {item.Control.DropDownStyle}");

                    if (item.Control.AutoCompleteMode != System.Windows.Forms.AutoCompleteMode.SuggestAppend)
                        throw new Exception($"ComboBox {item.Name} must have AutoCompleteMode = SuggestAppend, got {item.Control.AutoCompleteMode}");

                    if (item.Control.AutoCompleteSource != System.Windows.Forms.AutoCompleteSource.ListItems)
                        throw new Exception($"ComboBox {item.Name} must have AutoCompleteSource = ListItems, got {item.Control.AutoCompleteSource}");
                }
            }
        }

<<<<<<< HEAD
        public static void Test_ComponentTypeMetadata_StructuredCatalog()
        {
            var service = new SolutionAuditService();

            // 1. Site Map (62): Debe tener tabla sitemap, PK sitemapid, nombre sitemapname y fallback sitemapnameunique
            var sitemap = service.GetComponentTypeMetadata(62);
            if (sitemap == null || sitemap.EntityLogicalName != "sitemap" || sitemap.PrimaryKeyAttribute != "sitemapid" || sitemap.PrimaryNameAttribute != "sitemapname")
                throw new Exception($"Expected Site Map structured metadata, got {sitemap?.EntityLogicalName}/{sitemap?.PrimaryKeyAttribute}/{sitemap?.PrimaryNameAttribute}");
            if (sitemap.FallbackNameAttributes == null || !sitemap.FallbackNameAttributes.Contains("sitemapnameunique"))
                throw new Exception("Site Map must contain sitemapnameunique in FallbackNameAttributes");

            // 2. Workflow / Cloud Flow (29): Debe tener tabla workflow, PK workflowid, nombre name
            var wf = service.GetComponentTypeMetadata(29);
            if (wf == null || wf.EntityLogicalName != "workflow" || wf.PrimaryKeyAttribute != "workflowid" || wf.PrimaryNameAttribute != "name")
                throw new Exception($"Expected Workflow structured metadata, got {wf?.EntityLogicalName}/{wf?.PrimaryKeyAttribute}/{wf?.PrimaryNameAttribute}");

            // 3. Environment Variable Definition (380): PK environmentvariabledefinitionid, nombre displayname, fallback schemaname
            var envDef = service.GetComponentTypeMetadata(380);
            if (envDef == null || envDef.EntityLogicalName != "environmentvariabledefinition" || envDef.PrimaryKeyAttribute != "environmentvariabledefinitionid" || envDef.PrimaryNameAttribute != "displayname")
                throw new Exception($"Expected Env Var Def metadata, got {envDef?.EntityLogicalName}/{envDef?.PrimaryKeyAttribute}/{envDef?.PrimaryNameAttribute}");

            // 4. Environment Variable Value (381): PK environmentvariablevalueid, nombre value, parent lookup environmentvariabledefinitionid
            var envVal = service.GetComponentTypeMetadata(381);
            if (envVal == null || envVal.EntityLogicalName != "environmentvariablevalue" || envVal.PrimaryKeyAttribute != "environmentvariablevalueid" || envVal.PrimaryNameAttribute != "value" || envVal.ParentEntityAttribute != "environmentvariabledefinitionid")
                throw new Exception($"Expected Env Var Value metadata with parent lookup, got {envVal?.EntityLogicalName}/{envVal?.ParentEntityAttribute}");

            // 5. AI Model (10200): PK msdyn_aimodelid, nombre msdyn_name
            var aiModel = service.GetComponentTypeMetadata(10200);
            if (aiModel == null || aiModel.EntityLogicalName != "msdyn_aimodel" || aiModel.PrimaryKeyAttribute != "msdyn_aimodelid" || aiModel.PrimaryNameAttribute != "msdyn_name")
                throw new Exception($"Expected AI Model metadata, got {aiModel?.EntityLogicalName}/{aiModel?.PrimaryKeyAttribute}/{aiModel?.PrimaryNameAttribute}");

            // 6. Canvas App (300): PK canvasappid, nombre displayname, fallback name
            var canvasApp = service.GetComponentTypeMetadata(300);
            if (canvasApp == null || canvasApp.EntityLogicalName != "canvasapp" || canvasApp.PrimaryKeyAttribute != "canvasappid" || canvasApp.PrimaryNameAttribute != "displayname")
                throw new Exception($"Expected Canvas App metadata with displayname primary, got {canvasApp?.EntityLogicalName}/{canvasApp?.PrimaryNameAttribute}");

            // 7. Model-Driven App (80): PK appmoduleid, nombre name
            var modelApp = service.GetComponentTypeMetadata(80);
            if (modelApp == null || modelApp.EntityLogicalName != "appmodule" || modelApp.PrimaryKeyAttribute != "appmoduleid" || modelApp.PrimaryNameAttribute != "name")
                throw new Exception($"Expected Model-Driven App metadata, got {modelApp?.EntityLogicalName}/{modelApp?.PrimaryNameAttribute}");
        }

        public static void Test_ProcessSubtypes_WorkflowCategories()
        {
            // 1. Debe existir catálogo de workflow_category con los 8 subtipos canónicos de Dataverse
            var categories = SolutionAuditService.WorkflowCategories;
            if (categories == null || categories.Count < 8)
                throw new Exception($"Expected at least 8 workflow categories, got {categories?.Count}");

            if (!categories.ContainsKey(0) || categories[0] != "Classic Workflow")
                throw new Exception("Expected category 0 = Classic Workflow");
            if (!categories.ContainsKey(1) || categories[1] != "Dialog")
                throw new Exception("Expected category 1 = Dialog");
            if (!categories.ContainsKey(2) || categories[2] != "Business Rule")
                throw new Exception("Expected category 2 = Business Rule");
            if (!categories.ContainsKey(3) || categories[3] != "Action")
                throw new Exception("Expected category 3 = Action");
            if (!categories.ContainsKey(4) || categories[4] != "Business Process Flow (BPF)")
                throw new Exception("Expected category 4 = Business Process Flow (BPF)");
            if (!categories.ContainsKey(5) || categories[5] != "Cloud Flow (Power Automate)")
                throw new Exception("Expected category 5 = Cloud Flow (Power Automate)");
            if (!categories.ContainsKey(6) || categories[6] != "Desktop Flow")
                throw new Exception("Expected category 6 = Desktop Flow");
            if (!categories.ContainsKey(7) || categories[7] != "AI Flow")
                throw new Exception("Expected category 7 = AI Flow");

            // 2. Formateo de nombre de workflow con su prefijo de subtipo
            string cloudFlow = SolutionAuditService.FormatWorkflowName("Order Notification", 5);
            if (cloudFlow != "[Cloud Flow] Order Notification")
                throw new Exception($"Expected '[Cloud Flow] Order Notification', got '{cloudFlow}'");

            string bpf = SolutionAuditService.FormatWorkflowName("Lead to Opportunity", 4);
            if (bpf != "[BPF] Lead to Opportunity")
                throw new Exception($"Expected '[BPF] Lead to Opportunity', got '{bpf}'");

            string action = SolutionAuditService.FormatWorkflowName("CalculateDiscount", 3);
            if (action != "[Action] CalculateDiscount")
                throw new Exception($"Expected '[Action] CalculateDiscount', got '{action}'");

            string classicWf = SolutionAuditService.FormatWorkflowName("Auto Close Ticket", 0);
            if (classicWf != "[Classic Workflow] Auto Close Ticket")
                throw new Exception($"Expected '[Classic Workflow] Auto Close Ticket', got '{classicWf}'");

            string bizRule = SolutionAuditService.FormatWorkflowName("Set Phone Required", 2);
            if (bizRule != "[Business Rule] Set Phone Required")
                throw new Exception($"Expected '[Business Rule] Set Phone Required', got '{bizRule}'");

            string desktopFlow = SolutionAuditService.FormatWorkflowName("SAP Invoicing", 6);
            if (desktopFlow != "[Desktop Flow] SAP Invoicing")
                throw new Exception($"Expected '[Desktop Flow] SAP Invoicing', got '{desktopFlow}'");

            string aiFlow = SolutionAuditService.FormatWorkflowName("Summarize Case", 7);
            if (aiFlow != "[AI Flow] Summarize Case")
                throw new Exception($"Expected '[AI Flow] Summarize Case', got '{aiFlow}'");

            string dialog = SolutionAuditService.FormatWorkflowName("Customer Survey", 1);
            if (dialog != "[Dialog] Customer Survey")
                throw new Exception($"Expected '[Dialog] Customer Survey', got '{dialog}'");
        }

        public static void Test_GetWorkflowsByCategory_FilteringAndPrefixing()
        {
            var auditService = new SolutionAuditService();
            var wfId1 = Guid.NewGuid();
            var wfId2 = Guid.NewGuid();
            var wfId3 = Guid.NewGuid();

            var fakeService = new FakeOrganizationService
            {
                OnRetrieveMultiple = (query) =>
                {
                    var qe = query as Microsoft.Xrm.Sdk.Query.QueryExpression;
                    var coll = new EntityCollection();

                    int? filterCat = null;
                    if (qe != null && qe.Criteria != null)
                    {
                        foreach (var cond in qe.Criteria.Conditions)
                        {
                            if (cond.AttributeName == "category")
                            {
                                filterCat = Convert.ToInt32(cond.Values[0]);
                            }
                        }
                    }

                    if (!filterCat.HasValue || filterCat.Value == 5)
                    {
                        var e1 = new Entity("workflow", wfId1);
                        e1["name"] = "Automated Order Processing";
                        e1["category"] = new OptionSetValue(5);
                        coll.Entities.Add(e1);
                    }

                    if (!filterCat.HasValue || filterCat.Value == 4)
                    {
                        var e2 = new Entity("workflow", wfId2);
                        e2["name"] = "Lead Qualification";
                        e2["category"] = new OptionSetValue(4);
                        coll.Entities.Add(e2);
                    }

                    if (!filterCat.HasValue || filterCat.Value == 0)
                    {
                        var e3 = new Entity("workflow", wfId3);
                        e3["name"] = "Classic Notification";
                        e3["category"] = new OptionSetValue(0);
                        coll.Entities.Add(e3);
                    }

                    return coll;
                }
            };

            // 1. All workflows: prefixing applies to all
            var allWfs = auditService.GetWorkflowsByCategory(fakeService, null);
            if (allWfs.Count != 3)
                throw new Exception($"Expected 3 workflows, got {allWfs.Count}");
            if (!allWfs.Any(w => w.Name == "[Cloud Flow] Automated Order Processing"))
                throw new Exception("Missing prefixed Cloud Flow");
            if (!allWfs.Any(w => w.Name == "[BPF] Lead Qualification"))
                throw new Exception("Missing prefixed BPF");
            if (!allWfs.Any(w => w.Name == "[Classic Workflow] Classic Notification"))
                throw new Exception("Missing prefixed Classic Workflow");

            // 2. Filtered by category 5 (Cloud Flow) -> Redundant prefix omitted
            var cloudFlows = auditService.GetWorkflowsByCategory(fakeService, 5);
            if (cloudFlows.Count != 1 || cloudFlows[0].Name != "Automated Order Processing")
                throw new Exception($"Filtering by category 5 failed, got '{cloudFlows[0]?.Name}'");

            // 3. Filtered by category 4 (BPF) -> Redundant prefix omitted
            var bpfs = auditService.GetWorkflowsByCategory(fakeService, 4);
            if (bpfs.Count != 1 || bpfs[0].Name != "Lead Qualification")
                throw new Exception($"Filtering by category 4 failed, got '{bpfs[0]?.Name}'");
        }

        public static void Test_GetComponentName_DeterministicMetadataResolution()
        {
            var auditService = new SolutionAuditService();
            var fakeService = new FakeOrganizationService
            {
                OnRetrieve = (entityName, targetId, cols) =>
                {
                    if (entityName == "sitemap")
                    {
                        var e = new Entity("sitemap", targetId);
                        e["sitemapname"] = "Customer Service App Sitemap";
                        return e;
                    }
                    if (entityName == "canvasapp")
                    {
                        var e = new Entity("canvasapp", targetId);
                        e["displayname"] = "Warehouse Inventory App";
                        return e;
                    }
                    if (entityName == "msdyn_aimodel")
                    {
                        var e = new Entity("msdyn_aimodel", targetId);
                        e["msdyn_name"] = "Document Processing AI";
                        return e;
                    }
                    if (entityName == "environmentvariabledefinition")
                    {
                        var e = new Entity("environmentvariabledefinition", targetId);
                        e["displayname"] = "Azure Function Key";
                        return e;
                    }
                    if (entityName == "environmentvariablevalue")
                    {
                        var e = new Entity("environmentvariablevalue", targetId);
                        e["value"] = "https://myapi.azurewebsites.net";
                        return e;
                    }
                    if (entityName == "workflow")
                    {
                        var e = new Entity("workflow", targetId);
                        e["name"] = "Sync Orders to ERP";
                        e["category"] = new OptionSetValue(5); // Cloud Flow
                        return e;
                    }
                    if (entityName == "stagedoptionset")
                    {
                        var e = new Entity("stagedoptionset", targetId);
                        e["name"] = "Lead Stage Options";
                        return e;
                    }
                    return null;
                }
            };

            var id = Guid.NewGuid();

            // Sitemap (62)
            string sitemapName = auditService.GetComponentName(fakeService, "sitemap", id, 62);
            if (sitemapName != "Customer Service App Sitemap")
                throw new Exception($"Expected 'Customer Service App Sitemap', got '{sitemapName}'");

            // Canvas App (300)
            string canvasName = auditService.GetComponentName(fakeService, "canvasapp", id, 300);
            if (canvasName != "Warehouse Inventory App")
                throw new Exception($"Expected 'Warehouse Inventory App', got '{canvasName}'");

            // AI Model (10200)
            string aiModelName = auditService.GetComponentName(fakeService, "msdyn_aimodel", id, 10200);
            if (aiModelName != "Document Processing AI")
                throw new Exception($"Expected 'Document Processing AI', got '{aiModelName}'");

            // Environment Variable Definition (380)
            string envDefName = auditService.GetComponentName(fakeService, "environmentvariabledefinition", id, 380);
            if (envDefName != "Azure Function Key")
                throw new Exception($"Expected 'Azure Function Key', got '{envDefName}'");

            // Environment Variable Value (381)
            string envValName = auditService.GetComponentName(fakeService, "environmentvariablevalue", id, 381);
            if (envValName != "https://myapi.azurewebsites.net")
                throw new Exception($"Expected 'https://myapi.azurewebsites.net', got '{envValName}'");

            // Workflow / Cloud Flow (29) with subtype prefix
            string wfName = auditService.GetComponentName(fakeService, "workflow", id, 29);
            if (wfName != "[Cloud Flow] Sync Orders to ERP")
                throw new Exception($"Expected '[Cloud Flow] Sync Orders to ERP', got '{wfName}'");

            // Staged Option Set (10027)
            string stagedName = auditService.GetComponentName(fakeService, "stagedoptionset", id, 10027);
            if (stagedName != "Lead Stage Options")
                throw new Exception($"Expected 'Lead Stage Options', got '{stagedName}'");
        }

        public static void Test_GetOverlappingSolutions_WithSummaryAndFallback()
        {
            var auditService = new SolutionAuditService();
            var solId = Guid.NewGuid();
            var compId1 = Guid.NewGuid();
            var otherSolId = Guid.NewGuid();

            // Case 1: Environment with msdyn_solutioncomponentsummary
            var fakeServiceWithSummary = new FakeOrganizationService
            {
                OnRetrieveMultiple = (query) =>
                {
                    var qe = query as Microsoft.Xrm.Sdk.Query.QueryExpression;
                    var coll = new EntityCollection();

                    if (qe != null && qe.EntityName == "msdyn_solutioncomponentsummary")
                    {
                        var summary = new Entity("msdyn_solutioncomponentsummary");
                        summary["msdyn_objectid"] = compId1;
                        summary["msdyn_displayname"] = "Send Welcome Email";
                        summary["msdyn_componenttype"] = new OptionSetValue(29);
                        summary["msdyn_workflowcategory"] = new OptionSetValue(5); // Cloud Flow
                        coll.Entities.Add(summary);
                        return coll;
                    }

                    if (qe != null && qe.EntityName == "solutioncomponent")
                    {
                        bool isMain = qe.Criteria.Conditions.Any(c => c.AttributeName == "solutionid" && c.Operator == Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal);
                        if (isMain)
                        {
                            var sc = new Entity("solutioncomponent");
                            sc["objectid"] = compId1;
                            sc["componenttype"] = new OptionSetValue(29);
                            sc["solutionid"] = solId;
                            coll.Entities.Add(sc);
                            return coll;
                        }
                        else
                        {
                            var sc = new Entity("solutioncomponent");
                            sc["objectid"] = compId1;
                            sc["solutionid"] = otherSolId;
                            coll.Entities.Add(sc);
                            return coll;
                        }
                    }

                    if (qe != null && qe.EntityName == "solution")
                    {
                        var sol = new Entity("solution", otherSolId);
                        sol["friendlyname"] = "Core System";
                        sol["uniquename"] = "CoreSystem";
                        sol["version"] = "1.0.0.0";
                        sol["ismanaged"] = false;
                        coll.Entities.Add(sol);
                        return coll;
                    }

                    return coll;
                }
            };

            var overlapsWithSummary = auditService.GetOverlappingSolutions(fakeServiceWithSummary, solId);
            if (overlapsWithSummary.Count != 1)
                throw new Exception($"Expected 1 overlap, got {overlapsWithSummary.Count}");
            if (overlapsWithSummary[0].ComponentName != "[Cloud Flow] Send Welcome Email")
                throw new Exception($"Expected '[Cloud Flow] Send Welcome Email', got '{overlapsWithSummary[0].ComponentName}'");

            // Case 2: Environment where msdyn_solutioncomponentsummary throws (e.g. not installed / older version)
            var fakeServiceFallback = new FakeOrganizationService
            {
                OnRetrieveMultiple = (query) =>
                {
                    var qe = query as Microsoft.Xrm.Sdk.Query.QueryExpression;
                    if (qe != null && qe.EntityName == "msdyn_solutioncomponentsummary")
                    {
                        throw new InvalidOperationException("Entity msdyn_solutioncomponentsummary does not exist");
                    }

                    var coll = new EntityCollection();
                    if (qe != null && qe.EntityName == "solutioncomponent")
                    {
                        bool isMain = qe.Criteria.Conditions.Any(c => c.AttributeName == "solutionid" && c.Operator == Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal);
                        if (isMain)
                        {
                            var sc = new Entity("solutioncomponent");
                            sc["objectid"] = compId1;
                            sc["componenttype"] = new OptionSetValue(62); // Sitemap
                            sc["solutionid"] = solId;
                            coll.Entities.Add(sc);
                            return coll;
                        }
                        else
                        {
                            var sc = new Entity("solutioncomponent");
                            sc["objectid"] = compId1;
                            sc["solutionid"] = otherSolId;
                            coll.Entities.Add(sc);
                            return coll;
                        }
                    }

                    if (qe != null && qe.EntityName == "solution")
                    {
                        var sol = new Entity("solution", otherSolId);
                        sol["friendlyname"] = "Base App";
                        sol["uniquename"] = "BaseApp";
                        sol["version"] = "2.0.0.0";
                        sol["ismanaged"] = true;
                        coll.Entities.Add(sol);
                        return coll;
                    }

                    return coll;
                },
                OnRetrieve = (entityName, id, cols) =>
                {
                    if (entityName == "sitemap")
                    {
                        var sm = new Entity("sitemap", id);
                        sm["sitemapname"] = "Fallback Navigation Map";
                        return sm;
                    }
                    return null;
                }
            };

            var overlapsFallback = auditService.GetOverlappingSolutions(fakeServiceFallback, solId);
            if (overlapsFallback.Count != 1)
                throw new Exception($"Expected 1 fallback overlap, got {overlapsFallback.Count}");
            if (overlapsFallback[0].ComponentName != "Fallback Navigation Map")
                throw new Exception($"Expected 'Fallback Navigation Map', got '{overlapsFallback[0].ComponentName}'");
        }

        public static void Test_ResolveComponentTypes_WithGlobalLabelsAndSchemaNames()
        {
            var auditService = new SolutionAuditService();
            var solId = Guid.NewGuid();
            var attrId = Guid.NewGuid();
            var modernId = Guid.NewGuid();
            var otherSolId = Guid.NewGuid();

            var fakeService = new FakeOrganizationService
            {
                OnRetrieveMultiple = (query) =>
                {
                    var qe = query as Microsoft.Xrm.Sdk.Query.QueryExpression;
                    var coll = new EntityCollection();

                    if (qe != null && qe.EntityName == "msdyn_solutioncomponentsummary")
                    {
                        var s1 = new Entity("msdyn_solutioncomponentsummary");
                        s1["msdyn_objectid"] = attrId;
                        s1["msdyn_schemaname"] = "new_customerid";
                        s1["msdyn_componenttype"] = new OptionSetValue(2);
                        s1["msdyn_primaryentityname"] = "account";
                        s1["msdyn_componenttypename"] = "Entity field";
                        coll.Entities.Add(s1);

                        var s2 = new Entity("msdyn_solutioncomponentsummary");
                        s2["msdyn_objectid"] = modernId;
                        s2["msdyn_name"] = "My Custom Connector Item";
                        s2["msdyn_componenttype"] = new OptionSetValue(99999);
                        s2["msdyn_componenttypename"] = "Special Cloud Connector";
                        coll.Entities.Add(s2);

                        return coll;
                    }

                    if (qe != null && qe.EntityName == "solutioncomponent")
                    {
                        bool isMain = qe.Criteria.Conditions.Any(c => c.AttributeName == "solutionid" && c.Operator == Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal);
                        if (isMain)
                        {
                            var sc1 = new Entity("solutioncomponent");
                            sc1["objectid"] = attrId;
                            sc1["componenttype"] = new OptionSetValue(2);
                            sc1["solutionid"] = solId;
                            coll.Entities.Add(sc1);

                            var sc2 = new Entity("solutioncomponent");
                            sc2["objectid"] = modernId;
                            sc2["componenttype"] = new OptionSetValue(99999);
                            sc2["solutionid"] = solId;
                            coll.Entities.Add(sc2);

                            return coll;
                        }
                        else
                        {
                            var sc = new Entity("solutioncomponent");
                            sc["objectid"] = (Guid)qe.Criteria.Conditions.First(c => c.AttributeName == "objectid").Values[0];
                            sc["solutionid"] = otherSolId;
                            coll.Entities.Add(sc);
                            return coll;
                        }
                    }

                    if (qe != null && qe.EntityName == "solution")
                    {
                        var sol = new Entity("solution", otherSolId);
                        sol["friendlyname"] = "Other Sol";
                        sol["uniquename"] = "OtherSol";
                        sol["version"] = "1.0.0.0";
                        coll.Entities.Add(sol);
                        return coll;
                    }

                    return coll;
                }
            };

            var overlaps = auditService.GetOverlappingSolutions(fakeService, solId);
            if (overlaps.Count != 2)
                throw new Exception($"Expected 2 overlaps, got {overlaps.Count}");

            var attrOverlap = overlaps.FirstOrDefault(o => o.ComponentId == attrId);
            if (attrOverlap == null || attrOverlap.ComponentName != "new_customerid [account]")
                throw new Exception($"Expected attribute 'new_customerid [account]', got '{attrOverlap?.ComponentName}'");

            var modernOverlap = overlaps.FirstOrDefault(o => o.ComponentId == modernId);
            if (modernOverlap == null || modernOverlap.ComponentType != "Special Cloud Connector")
                throw new Exception($"Expected type 'Special Cloud Connector', got '{modernOverlap?.ComponentType}'");
        }

        public static void Test_MyPluginControl_ProcessCategoryDropdownSwitching()
        {
            using (var control = new MyPluginControl())
            {
                var cbTypes = control.GetType().GetField("cbComponentTypes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox;
                var cbEntities = control.GetType().GetField("cbEntities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.ComboBox;
                var lblEntity = control.GetType().GetField("lblEntity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(control) as System.Windows.Forms.Label;

                if (cbTypes == null || cbEntities == null || lblEntity == null)
                    throw new Exception("Controls not found via reflection");

                // 1. Select Workflow / Cloud Flow (type 29)
                int wfIndex = -1;
                for (int i = 0; i < cbTypes.Items.Count; i++)
                {
                    if (cbTypes.Items[i] is MyPluginControl.ComboBoxTypeItem item && item.TypeCode == 29)
                    {
                        wfIndex = i;
                        break;
                    }
                }

                if (wfIndex < 0)
                    throw new Exception("Workflow / Cloud Flow (29) not found in cbComponentTypes");

                cbTypes.SelectedIndex = wfIndex;

                if (lblEntity.Text != "Process Category:")
                    throw new Exception($"Expected lblEntity.Text = 'Process Category:', got '{lblEntity.Text}'");
                if (!cbEntities.Enabled)
                    throw new Exception("cbEntities should be enabled for Workflow / Cloud Flow");
                if (cbEntities.Items.Count != 9)
                    throw new Exception($"Expected 9 process categories, got {cbEntities.Items.Count}");
                if (cbEntities.Items[0].ToString() != "[All process types]")
                    throw new Exception($"Expected '[All process types]', got '{cbEntities.Items[0]}'");
                if (cbEntities.Items[1].ToString() != "Cloud Flow (Power Automate)")
                    throw new Exception($"Expected 'Cloud Flow (Power Automate)', got '{cbEntities.Items[1]}'");

                // 2. Select Entity field (type 2 - entity dependent)
                int attrIndex = -1;
                for (int i = 0; i < cbTypes.Items.Count; i++)
                {
                    if (cbTypes.Items[i] is MyPluginControl.ComboBoxTypeItem item && item.TypeCode == 2)
                    {
                        attrIndex = i;
                        break;
                    }
                }
                if (attrIndex >= 0)
                {
                    cbTypes.SelectedIndex = attrIndex;
                    if (lblEntity.Text != "Select Entity / Table:")
                        throw new Exception($"Expected lblEntity.Text = 'Select Entity / Table:', got '{lblEntity.Text}'");
                }
            }
        }

        public class FakeOrganizationService : IOrganizationService
        {
            public Func<OrganizationRequest, OrganizationResponse> OnExecute { get; set; }
            public Func<string, Guid, Microsoft.Xrm.Sdk.Query.ColumnSet, Entity> OnRetrieve { get; set; }
            public Func<Microsoft.Xrm.Sdk.Query.QueryBase, EntityCollection> OnRetrieveMultiple { get; set; }

            public OrganizationResponse Execute(OrganizationRequest request) => OnExecute != null ? OnExecute(request) : null;
            public Entity Retrieve(string entityName, Guid id, Microsoft.Xrm.Sdk.Query.ColumnSet columnSet) => OnRetrieve != null ? OnRetrieve(entityName, id, columnSet) : null;
            public EntityCollection RetrieveMultiple(Microsoft.Xrm.Sdk.Query.QueryBase query) => OnRetrieveMultiple != null ? OnRetrieveMultiple(query) : new EntityCollection();
            public Guid Create(Entity entity) => Guid.Empty;
            public void Update(Entity entity) { }
            public void Delete(string entityName, Guid id) { }
            public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities) { }
            public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities) { }
        }

        public static void Test_PluginControl_Implements_IGitHubPlugin_And_IHelpPlugin()
        {
            using (var control = new MyPluginControl())
            {
                if (!(control is IGitHubPlugin githubPlugin))
                    throw new Exception("MyPluginControl must implement IGitHubPlugin");

                if (githubPlugin.UserName != "elrebollon")
                    throw new Exception($"Expected IGitHubPlugin.UserName 'elrebollon', got '{githubPlugin.UserName}'");

                if (githubPlugin.RepositoryName != "SolutionComponents")
                    throw new Exception($"Expected IGitHubPlugin.RepositoryName 'SolutionComponents', got '{githubPlugin.RepositoryName}'");

                if (!(control is IHelpPlugin helpPlugin))
                    throw new Exception("MyPluginControl must implement IHelpPlugin");

                if (string.IsNullOrWhiteSpace(helpPlugin.HelpUrl) || !helpPlugin.HelpUrl.Contains("github.com/elrebollon/SolutionComponents"))
                    throw new Exception($"Expected valid IHelpPlugin.HelpUrl pointing to GitHub repository, got '{helpPlugin.HelpUrl}'");
            }
        }
    }
}
