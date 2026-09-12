using Microsoft.Xrm.Sdk;
using SolutionComponents.Models;
using SolutionComponents.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

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
    }
}
