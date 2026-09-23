using System;
using System.Linq;

namespace SolutionComponents.Models
{
    public class ComponentTypeMetadata
    {
        public int TypeCode { get; set; }
        public string TypeName { get; set; }
        public string EntityLogicalName { get; set; }
        public string ComponentTypeName => TypeName;
        public string EntityName => EntityLogicalName;
        public string PrimaryKeyAttribute { get; set; }
        public string PrimaryNameAttribute { get; set; }
        public string[] FallbackNameAttributes { get; set; }
        public bool IsMetadataComponent { get; set; }
        public string ParentEntityAttribute { get; set; }

        public ComponentTypeMetadata()
        {
            FallbackNameAttributes = new string[0];
            EntityLogicalName = string.Empty;
            PrimaryKeyAttribute = string.Empty;
            PrimaryNameAttribute = string.Empty;
            ParentEntityAttribute = string.Empty;
        }

        public ComponentTypeMetadata(
            int typeCode,
            string typeName,
            string entityLogicalName,
            string primaryKeyAttribute,
            string primaryNameAttribute,
            string[] fallbackNameAttributes = null,
            bool isMetadataComponent = false,
            string parentEntityAttribute = null)
        {
            TypeCode = typeCode;
            TypeName = typeName;
            EntityLogicalName = entityLogicalName ?? string.Empty;
            PrimaryKeyAttribute = primaryKeyAttribute ?? string.Empty;
            PrimaryNameAttribute = primaryNameAttribute ?? string.Empty;
            FallbackNameAttributes = fallbackNameAttributes ?? new string[0];
            IsMetadataComponent = isMetadataComponent;
            ParentEntityAttribute = parentEntityAttribute ?? string.Empty;
        }

        public override string ToString()
        {
            return TypeName ?? EntityLogicalName ?? TypeCode.ToString();
        }
    }

    public class ProcessCategoryItem
    {
        public int? Category { get; set; }
        public string DisplayName { get; set; }

        public ProcessCategoryItem(int? category, string displayName)
        {
            Category = category;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
