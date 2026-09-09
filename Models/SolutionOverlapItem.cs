using System;

namespace SolutionComponents.Models
{
    public class SolutionOverlapItem
    {
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public Guid ComponentId { get; set; }
        public string RelatedSolutionName { get; set; }
        public string RelatedSolutionUniqueName { get; set; }
        public string Version { get; set; }
        public string PublisherName { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsManaged { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? InstalledOn { get; set; }

        public string ManagedDisplay => IsManaged ? "Yes" : "No";
    }

    public class SolutionSummary
    {
        public Guid Id { get; set; }
        public string FriendlyName { get; set; }
        public string UniqueName { get; set; }
        public string Version { get; set; }
        public string PublisherName { get; set; }
        public bool IsManaged { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(FriendlyName) ? UniqueName : $"{FriendlyName} ({UniqueName})";
        }
    }
}
