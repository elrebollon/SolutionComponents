using System;

namespace SolutionComponents.Models
{
    public class ComponentHistoryItem
    {
        public string SolutionName { get; set; }
        public string SolutionUniqueName { get; set; }
        public string Version { get; set; }
        public string PublisherName { get; set; }
        public bool IsManaged { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? InstalledOn { get; set; }

        public string ManagedDisplay => IsManaged ? "Yes" : "No";
    }

    public class EntityMetadataSummary
    {
        public string LogicalName { get; set; }
        public string DisplayName { get; set; }
        public int? ObjectTypeCode { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(DisplayName) ? LogicalName : $"{DisplayName} ({LogicalName})";
        }
    }

    public class ComponentItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ComponentType { get; set; }
        public string ComponentTypeName { get; set; }
        public string EntityLogicalName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
