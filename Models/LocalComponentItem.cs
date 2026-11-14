using System;

namespace SolutionComponents.Models
{
    public class LocalComponentItem
    {
        public int ComponentType { get; set; }
        public Guid ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(DisplayName))
            {
                return !string.IsNullOrWhiteSpace(SchemaName) && !string.Equals(DisplayName, SchemaName, StringComparison.OrdinalIgnoreCase)
                    ? $"{DisplayName} ({SchemaName})"
                    : DisplayName;
            }

            if (!string.IsNullOrWhiteSpace(SchemaName))
            {
                return SchemaName;
            }

            return ObjectId != Guid.Empty ? ObjectId.ToString() : "Unknown";
        }
    }
}
