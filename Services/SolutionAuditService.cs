using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using SolutionComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionComponents.Services
{
    public class SolutionAuditService
    {
        public static readonly Dictionary<int, (string ComponentTypeName, string EntityName)> ComponentTypes = new Dictionary<int, (string, string)>
        {
            { 1, ("Entity", "entity") },
            { 2, ("Entity field", "attribute") },
            { 3, ("Relationship", "relationship") },
            { 4, ("Attribute Picklist Value", "") },
            { 5, ("Attribute Lookup Value", "") },
            { 6, ("View Attribute", "") },
            { 7, ("Localized Label", "") },
            { 8, ("Relationship Extra Condition", "") },
            { 9, ("Option Set", "optionset") },
            { 10, ("Entity Relationship", "entityrelationship") },
            { 11, ("Entity Relationship Role", "") },
            { 12, ("Entity Relationship Relationships", "") },
            { 13, ("Managed Property", "") },
            { 14, ("Entity Key", "entitykey") },
            { 16, ("Privilege", "") },
            { 17, ("PrivilegeObjectTypeCode", "") },
            { 18, ("Index", "entityindex") },
            { 20, ("Role", "role") },
            { 21, ("Role Privilege", "") },
            { 22, ("Display String", "") },
            { 23, ("Display String Map", "") },
            { 24, ("Form", "systemform") },
            { 25, ("Organization", "") },
            { 26, ("View", "savedquery") },
            { 29, ("Workflow / Cloud Flow", "workflow") },
            { 31, ("Report", "") },
            { 32, ("Report Entity", "") },
            { 33, ("Report Category", "") },
            { 34, ("Report Visibility", "") },
            { 35, ("Attachment", "") },
            { 36, ("Email Template", "") },
            { 37, ("Contract Template", "") },
            { 38, ("KB Article Template", "") },
            { 39, ("Mail Merge Template", "") },
            { 44, ("Duplicate Rule", "") },
            { 45, ("Duplicate Rule Condition", "") },
            { 46, ("Entity Map", "") },
            { 47, ("Attribute Map", "") },
            { 48, ("Ribbon Command", "") },
            { 49, ("Ribbon Context Group", "") },
            { 50, ("Ribbon Customization", "") },
            { 52, ("Ribbon Rule", "") },
            { 53, ("Ribbon Tab To Command Map", "") },
            { 55, ("Ribbon Diff", "") },
            { 59, ("Charts", "") },
            { 60, ("Entity Form", "userquery") },
            { 61, ("Web Resource", "webresource") },
            { 62, ("Site Map", "sitemap") },
            { 63, ("Connection Role", "") },
            { 64, ("Complex Control", "") },
            { 65, ("Hierarchy Rule", "") },
            { 66, ("Custom Control", "") },
            { 68, ("Custom Control Default Config", "") },
            { 70, ("Field Security Profile", "") },
            { 71, ("Field Permission", "") },
            { 90, ("Plugin Type", "plugintype") },
            { 91, ("Plugin Assembly", "pluginassembly") },
            { 92, ("SDK Message Processing Step", "sdkmessageprocessingstep") },
            { 93, ("SDK Message Processing Step Image", "sdkmessageprocessingstepimage") },
            { 95, ("Service Endpoint", "serviceendpoint") },
            { 150, ("Routing Rule", "") },
            { 151, ("Routing Rule Item", "") },
            { 152, ("SLA", "") },
            { 153, ("SLA Item", "") },
            { 154, ("Convert Rule", "") },
            { 155, ("Convert Rule Item", "") },
            { 161, ("Mobile Offline Profile", "") },
            { 162, ("Mobile Offline Profile Item", "") },
            { 165, ("Similarity Rule", "") },
            { 166, ("Data Source Mapping", "") },
            { 201, ("SDKMessage", "") },
            { 202, ("SDKMessageFilter", "") },
            { 203, ("SdkMessagePair", "") },
            { 204, ("SdkMessageRequest", "") },
            { 205, ("SdkMessageRequestField", "") },
            { 206, ("SdkMessageResponse", "") },
            { 207, ("SdkMessageResponseField", "") },
            { 208, ("Import Map", "") },
            { 210, ("WebWizard", "") },
            { 300, ("Canvas App", "canvasapp") },
            { 371, ("Connector", "") },
            { 372, ("Connector", "") },
            { 380, ("Environment Variable Definition", "environmentvariabledefinition") },
            { 381, ("Environment Variable Value", "environmentvariablevalue") },
            { 400, ("AI Project Type", "") },
            { 401, ("AI Project", "") },
            { 402, ("AI Configuration", "") },
            { 430, ("Entity Analytics Configuration", "") },
            { 431, ("Attribute Image Configuration", "") },
            { 432, ("Entity Image Configuration", "") }
        };

        public static bool IsEntityDependentComponent(int componentType)
        {
            // Tipos dependientes de una tabla/entidad (Forms, Views, Attributes, Relationships, Keys, Charts, etc.)
            return componentType == 2   // Attribute
                || componentType == 3   // Relationship
                || componentType == 10  // Entity Relationship
                || componentType == 14  // Entity Key
                || componentType == 18  // Index
                || componentType == 24  // Form (systemform)
                || componentType == 26  // View (savedquery)
                || componentType == 59  // Charts
                || componentType == 60  // Entity Form (userquery)
                || componentType == 65; // Hierarchy Rule
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
            linkPublisher.Columns = new ColumnSet("friendlyname", "customprefix");
            linkPublisher.EntityAlias = "pub";

            var result = service.RetrieveMultiple(query);
            var list = new List<SolutionSummary>();

            foreach (var entity in result.Entities)
            {
                string publisherName = entity.Contains("pub.friendlyname")
                    ? entity.GetAttributeValue<AliasedValue>("pub.friendlyname")?.Value as string
                    : null;

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

            return list;
        }

        public List<EntityMetadataSummary> GetEntities(IOrganizationService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            try
            {
                var request = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity,
                    RetrieveAsIfPublished = true
                };

                var response = (RetrieveAllEntitiesResponse)service.Execute(request);
                var list = new List<EntityMetadataSummary>();

                foreach (var meta in response.EntityMetadata)
                {
                    string displayName = meta.DisplayName?.UserLocalizedLabel?.Label ?? meta.LogicalName;
                    list.Add(new EntityMetadataSummary
                    {
                        LogicalName = meta.LogicalName,
                        DisplayName = displayName,
                        ObjectTypeCode = meta.ObjectTypeCode
                    });
                }

                return list.OrderBy(e => e.DisplayName).ToList();
            }
            catch
            {
                // Fallback ligero mediante consulta a la entidad 'entity' si la llamada de metadatos falla
                var query = new QueryExpression("entity")
                {
                    ColumnSet = new ColumnSet("name", "logicalname", "objecttypecode"),
                    Orders = { new OrderExpression("name", OrderType.Ascending) }
                };

                try
                {
                    var result = service.RetrieveMultiple(query);
                    return result.Entities.Select(e => new EntityMetadataSummary
                    {
                        LogicalName = e.GetAttributeValue<string>("logicalname") ?? e.GetAttributeValue<string>("name"),
                        DisplayName = e.GetAttributeValue<string>("name"),
                        ObjectTypeCode = e.GetAttributeValue<int?>("objecttypecode")
                    }).ToList();
                }
                catch
                {
                    return new List<EntityMetadataSummary>();
                }
            }
        }

        public (string ComponentTypeName, string EntityName) GetComponentTypeDetails(int componentType)
        {
            return ComponentTypes.ContainsKey(componentType)
                ? ComponentTypes[componentType]
                : ("Unknown", string.Empty);
        }

        public string GetComponentName(IOrganizationService service, string entityLogicalName, Guid objectId)
        {
            if (service == null || string.IsNullOrEmpty(entityLogicalName) || objectId == Guid.Empty)
                return "Unknown";

            try
            {
                Entity entity = service.Retrieve(entityLogicalName, objectId, new ColumnSet("name"));
                if (entity != null && entity.Contains("name"))
                {
                    return entity.GetAttributeValue<string>("name");
                }
            }
            catch
            {
                try
                {
                    Entity entity = service.Retrieve(entityLogicalName, objectId, new ColumnSet(true));
                    if (entity != null)
                    {
                        foreach (var attr in new[] { "friendlyname", "displayname", "schemaname", "uniquename" })
                        {
                            if (entity.Contains(attr))
                                return entity.GetAttributeValue<string>(attr);
                        }
                    }
                }
                catch
                {
                    // Ignore exceptions
                }
            }

            return "Unnamed";
        }

        public List<ComponentItem> GetComponentsByType(IOrganizationService service, int componentType, string entityLogicalName = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var details = GetComponentTypeDetails(componentType);
            var list = new List<ComponentItem>();

            try
            {
                if (componentType == 24) // Form (systemform)
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
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("name") ?? "Unnamed Form",
                            ComponentType = 24,
                            ComponentTypeName = "Form",
                            EntityLogicalName = e.GetAttributeValue<string>("objecttypecode")
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
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("name") ?? "Unnamed View",
                            ComponentType = 26,
                            ComponentTypeName = "View",
                            EntityLogicalName = e.GetAttributeValue<string>("returnedtypecode")
                        });
                    }
                }
                else if (componentType == 29) // Workflow / Cloud Flow
                {
                    var query = new QueryExpression("workflow")
                    {
                        ColumnSet = new ColumnSet("workflowid", "name", "category"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    query.Criteria.AddCondition("type", ConditionOperator.Equal, 1); // Definition
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("name") ?? "Unnamed Workflow",
                            ComponentType = 29,
                            ComponentTypeName = "Workflow / Flow",
                            EntityLogicalName = null
                        });
                    }
                }
                else if (componentType == 20) // Role
                {
                    var query = new QueryExpression("role")
                    {
                        ColumnSet = new ColumnSet("roleid", "name"),
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("name") ?? "Unnamed Role",
                            ComponentType = 20,
                            ComponentTypeName = "Role",
                            EntityLogicalName = null
                        });
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
                        TopCount = 200
                    };
                    var result = service.RetrieveMultiple(query);
                    foreach (var e in result.Entities)
                    {
                        string name = GetComponentName(service, details.EntityName, e.Id);
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = name,
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

            return list;
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

            foreach (var entity in result.Entities)
            {
                Guid solutionId = entity.GetAttributeValue<Guid>("solutionid");

                try
                {
                    var solQuery = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "installedon", "publisherid")
                    };
                    solQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solutionId);

                    var linkPub = solQuery.AddLink("publisher", "publisherid", "publisherid", JoinOperator.LeftOuter);
                    linkPub.Columns = new ColumnSet("friendlyname");
                    linkPub.EntityAlias = "pub";

                    var solRecords = service.RetrieveMultiple(solQuery);
                    if (solRecords != null && solRecords.Entities.Count > 0)
                    {
                        var sol = solRecords.Entities[0];
                        string pubName = sol.Contains("pub.friendlyname")
                            ? sol.GetAttributeValue<AliasedValue>("pub.friendlyname")?.Value as string
                            : "Unknown";

                        history.Add(new ComponentHistoryItem
                        {
                            SolutionName = sol.GetAttributeValue<string>("friendlyname") ?? sol.GetAttributeValue<string>("uniquename"),
                            SolutionUniqueName = sol.GetAttributeValue<string>("uniquename"),
                            Version = sol.GetAttributeValue<string>("version"),
                            PublisherName = pubName ?? "Unknown",
                            IsManaged = sol.GetAttributeValue<bool>("ismanaged"),
                            CreatedOn = sol.GetAttributeValue<DateTime?>("createdon"),
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
                .OrderByDescending(h => h.InstalledOn ?? h.CreatedOn ?? DateTime.MinValue)
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
                ColumnSet = new ColumnSet("componenttype", "objectid", "solutionid")
            };
            compQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, selectedSolutionId);

            var solutionComponents = service.RetrieveMultiple(compQuery);
            if (solutionComponents == null || solutionComponents.Entities.Count == 0)
                return new List<SolutionOverlapItem>();

            var overlaps = new List<SolutionOverlapItem>();

            foreach (var comp in solutionComponents.Entities)
            {
                Guid objectId = comp.GetAttributeValue<Guid>("objectid");
                int compType = comp.GetAttributeValue<int>("componenttype");

                var details = GetComponentTypeDetails(compType);
                string compName = GetComponentName(service, details.EntityName, objectId);

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
                    Guid otherSolutionId = relComp.GetAttributeValue<Guid>("solutionid");

                    try
                    {
                        var solQuery = new QueryExpression("solution")
                        {
                            ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "installedon", "publisherid")
                        };
                        solQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, otherSolutionId);

                        var linkPub = solQuery.AddLink("publisher", "publisherid", "publisherid", JoinOperator.LeftOuter);
                        linkPub.Columns = new ColumnSet("friendlyname");
                        linkPub.EntityAlias = "pub";

                        var solRecords = service.RetrieveMultiple(solQuery);
                        if (solRecords != null && solRecords.Entities.Count > 0)
                        {
                            var sol = solRecords.Entities[0];
                            string pubName = sol.Contains("pub.friendlyname")
                                ? sol.GetAttributeValue<AliasedValue>("pub.friendlyname")?.Value as string
                                : "Unknown";

                            overlaps.Add(new SolutionOverlapItem
                            {
                                ComponentName = compName,
                                ComponentType = details.ComponentTypeName,
                                ComponentId = objectId,
                                RelatedSolutionName = sol.GetAttributeValue<string>("friendlyname") ?? sol.GetAttributeValue<string>("uniquename"),
                                RelatedSolutionUniqueName = sol.GetAttributeValue<string>("uniquename"),
                                Version = sol.GetAttributeValue<string>("version"),
                                PublisherName = pubName ?? "Unknown",
                                IsManaged = sol.GetAttributeValue<bool>("ismanaged"),
                                CreatedOn = sol.GetAttributeValue<DateTime?>("createdon"),
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
