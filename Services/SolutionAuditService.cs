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
            { 16, ("Privilege", "privilege") },
            { 17, ("PrivilegeObjectTypeCode", "") },
            { 18, ("Index", "entityindex") },
            { 20, ("Role", "role") },
            { 21, ("Role Privilege", "") },
            { 22, ("Display String", "") },
            { 23, ("Display String Map", "") },
            { 24, ("Form", "systemform") },
            { 25, ("Organization", "organization") },
            { 26, ("View", "savedquery") },
            { 29, ("Workflow / Cloud Flow", "workflow") },
            { 31, ("Report", "report") },
            { 32, ("Report Entity", "") },
            { 33, ("Report Category", "") },
            { 34, ("Report Visibility", "") },
            { 35, ("Attachment", "") },
            { 36, ("Email Template", "template") },
            { 37, ("Contract Template", "contracttemplate") },
            { 38, ("KB Article Template", "kbarticletemplate") },
            { 39, ("Mail Merge Template", "mailmergetemplate") },
            { 44, ("Duplicate Rule", "duplicaterule") },
            { 45, ("Duplicate Rule Condition", "") },
            { 46, ("Entity Map", "") },
            { 47, ("Attribute Map", "") },
            { 48, ("Ribbon Command", "") },
            { 49, ("Ribbon Context Group", "") },
            { 50, ("Ribbon Customization", "ribboncustomization") },
            { 52, ("Ribbon Rule", "") },
            { 53, ("Ribbon Tab To Command Map", "") },
            { 55, ("Ribbon Diff", "") },
            { 59, ("Charts", "savedqueryvisualization") },
            { 60, ("System Form", "systemform") },
            { 61, ("Web Resource", "webresource") },
            { 62, ("Site Map", "sitemap") },
            { 63, ("Connection Role", "connectionrole") },
            { 64, ("Complex Control", "") },
            { 65, ("Hierarchy Rule", "hierarchyrule") },
            { 66, ("Custom Control", "customcontrol") },
            { 68, ("Custom Control Default Config", "customcontroldefaultconfig") },
            { 70, ("Field Security Profile", "fieldsecurityprofile") },
            { 71, ("Field Permission", "fieldpermission") },
            { 80, ("Model-Driven App", "appmodule") },
            { 81, ("App Module Component", "appmodulecomponent") },
            { 90, ("Plugin Type", "plugintype") },
            { 91, ("Plugin Assembly", "pluginassembly") },
            { 92, ("SDK Message Processing Step", "sdkmessageprocessingstep") },
            { 93, ("SDK Message Processing Step Image", "sdkmessageprocessingstepimage") },
            { 95, ("Service Endpoint", "serviceendpoint") },
            { 150, ("Routing Rule", "routingrule") },
            { 151, ("Routing Rule Item", "routingruleitem") },
            { 152, ("SLA", "sla") },
            { 153, ("SLA Item", "slaitem") },
            { 154, ("Convert Rule", "convertrule") },
            { 155, ("Convert Rule Item", "convertruleitem") },
            { 161, ("Mobile Offline Profile", "mobileofflineprofile") },
            { 162, ("Mobile Offline Profile Item", "mobileofflineprofileitem") },
            { 165, ("Similarity Rule", "similarityrule") },
            { 166, ("Data Source Mapping", "") },
            { 201, ("SDK Message", "sdkmessage") },
            { 202, ("SDK Message Filter", "sdkmessagefilter") },
            { 203, ("SDK Message Pair", "sdkmessagepair") },
            { 204, ("SDK Message Request", "") },
            { 205, ("SDK Message Request Field", "") },
            { 206, ("SDK Message Response", "") },
            { 207, ("SDK Message Response Field", "") },
            { 208, ("Import Map", "importmap") },
            { 210, ("WebWizard", "webwizard") },
            { 300, ("Canvas App", "canvasapp") },
            { 371, ("Connector", "connector") },
            { 372, ("Connection Reference", "connectionreference") },
            { 380, ("Environment Variable Definition", "environmentvariabledefinition") },
            { 381, ("Environment Variable Value", "environmentvariablevalue") },
            { 400, ("AI Project Type", "") },
            { 401, ("AI Project", "") },
            { 402, ("AI Configuration", "") },
            { 430, ("Entity Analytics Configuration", "") },
            { 431, ("Attribute Image Configuration", "") },
            { 432, ("Entity Image Configuration", "") },
            { 973, ("Copilot / Bot", "bot") },
            { 974, ("Bot Component", "botcomponent") },
            { 10020, ("Custom API", "customapi") },
            { 10021, ("Custom API Request Parameter", "customapirequestparameter") },
            { 10022, ("Custom API Response Property", "customapiresponseproperty") },
            { 10080, ("Plugin Package", "pluginpackage") },
            { 10091, ("Plugin Package", "pluginpackage") },
            { 10097, ("Desktop Flow", "workflow") },
            { 10098, ("Connection Reference", "connectionreference") },
            { 10110, ("Power Pages Site", "powerpagesite") },
            { 10111, ("Power Pages Component", "powerpagescomponent") },
            { 10200, ("AI Model / Prompt", "msdyn_aimodel") }
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
                || componentType == 60  // Entity Form (systemform)
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

        public (string ComponentTypeName, string EntityName) GetComponentTypeDetails(int componentType)
        {
            return ComponentTypes.ContainsKey(componentType)
                ? ComponentTypes[componentType]
                : ("Unknown", string.Empty);
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
                try
                {
                    var req = new RetrieveAttributeRequest
                    {
                        MetadataId = objectId,
                        RetrieveAsIfPublished = true
                    };
                    var resp = (RetrieveAttributeResponse)service.Execute(req);
                    if (resp?.AttributeMetadata != null)
                    {
                        string disp = resp.AttributeMetadata.DisplayName?.UserLocalizedLabel?.Label;
                        string logName = resp.AttributeMetadata.LogicalName;
                        string entityName = resp.AttributeMetadata.EntityLogicalName;
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

            // 4. OptionSet (componentType 9)
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

            // 5. Form / Entity Form (componentType 24, 60)
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

            // 6. View (componentType 26)
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

            // 7. Generic CRM Table lookup
            string targetEntity = !string.IsNullOrEmpty(entityLogicalName) ? entityLogicalName : GetComponentTypeDetails(componentType).EntityName;
            if (!string.IsNullOrEmpty(targetEntity) && targetEntity != "entity" && targetEntity != "attribute" && targetEntity != "relationship" && targetEntity != "entityrelationship")
            {
                try
                {
                    Entity entity = service.Retrieve(targetEntity, objectId, new ColumnSet(true));
                    if (entity != null)
                    {
                        foreach (var attr in new[] { "displayname", "name", "title", "subject", "friendlyname", "schemaname", "uniquename", "typename", "connectionreferencedisplayname", "connectionreferencename", "filename" })
                        {
                            if (entity.Contains(attr) && entity[attr] is string strVal && !string.IsNullOrEmpty(strVal))
                            {
                                return strVal;
                            }
                        }
                    }
                }
                catch { }
            }

            return $"Unnamed ({objectId})";
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
                        list.Add(new ComponentItem
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("name") ?? "Unnamed Form",
                            ComponentType = componentType,
                            ComponentTypeName = details.ComponentTypeName,
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

            return list;
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
                Guid solutionId = GetGuidAttribute(entity, "solutionid");

                try
                {
                    var solQuery = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "modifiedon", "installedon", "publisherid", "modifiedby")
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
                        string pubName = sol.Contains("pub.friendlyname")
                            ? sol.GetAttributeValue<AliasedValue>("pub.friendlyname")?.Value as string
                            : "Unknown";

                        string modUser = sol.Contains("usr.fullname")
                            ? sol.GetAttributeValue<AliasedValue>("usr.fullname")?.Value as string
                            : (sol.Contains("modifiedby") ? sol.GetAttributeValue<EntityReference>("modifiedby")?.Name : "System");

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
                ColumnSet = new ColumnSet("componenttype", "objectid", "solutionid")
            };
            compQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, selectedSolutionId);

            var solutionComponents = service.RetrieveMultiple(compQuery);
            if (solutionComponents == null || solutionComponents.Entities.Count == 0)
                return new List<SolutionOverlapItem>();

            var overlaps = new List<SolutionOverlapItem>();
            var compNameCache = new Dictionary<Guid, string>();

            foreach (var comp in solutionComponents.Entities)
            {
                Guid objectId = GetGuidAttribute(comp, "objectid");
                int compType = GetOptionSetAttribute(comp, "componenttype");

                var details = GetComponentTypeDetails(compType);
                if (!compNameCache.TryGetValue(objectId, out string compName))
                {
                    compName = GetComponentName(service, details.EntityName, objectId, compType);
                    compNameCache[objectId] = compName;
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
                            ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged", "createdon", "modifiedon", "installedon", "publisherid", "modifiedby")
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
                            string pubName = sol.Contains("pub.friendlyname")
                                ? sol.GetAttributeValue<AliasedValue>("pub.friendlyname")?.Value as string
                                : "Unknown";

                            string modUser = sol.Contains("usr.fullname")
                                ? sol.GetAttributeValue<AliasedValue>("usr.fullname")?.Value as string
                                : (sol.Contains("modifiedby") ? sol.GetAttributeValue<EntityReference>("modifiedby")?.Name : "System");

                            overlaps.Add(new SolutionOverlapItem
                            {
                                ComponentName = compName,
                                ComponentType = details.ComponentTypeName,
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
