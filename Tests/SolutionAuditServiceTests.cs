using SolutionComponents.Models;
using SolutionComponents.Services;
using System;
using System.Collections.Generic;
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
            Console.WriteLine("All SolutionAuditServiceTests passed successfully!");
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
    }
}
