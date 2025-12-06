using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MockScimServer.Infrastructure.Serialization;

namespace MockScimServer.Api.Metadata;

public static class MetadataEndpoints
{
    public static void MapMetadataEndpoints(this IEndpointRouteBuilder app)
    {
        MapServiceProviderConfig(app);
        MapSchemas(app);
        MapResourceTypes(app);
        MapRoot(app);
    }

    private static void MapServiceProviderConfig(IEndpointRouteBuilder app)
    {
        app.MapGet("/scim/v2/ServiceProviderConfig", () =>
        {
            var config = new
            {
                schemas = new[] { "urn:ietf:params:scim:schemas:core:2.0:ServiceProviderConfig" },
                patch = new { supported = true },
                bulk = new { supported = false, maxOperations = 0, maxPayloadSize = 0 },
                filter = new { supported = true, maxResults = 200 },
                changePassword = new { supported = false },
                sort = new { supported = false },
                etag = new { supported = false },
                authenticationSchemes = Array.Empty<object>()
            };

            return Results.Json(config, options: JsonSettings.ApiOptions);
        });
    }

    private static void MapSchemas(IEndpointRouteBuilder app)
    {
        app.MapGet("/scim/v2/Schemas", () =>
        {
            var schemas = new
            {
                schemas = new[] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" },
                Resources = new object[]
                {
                    new
                    {
                        id = "urn:ietf:params:scim:schemas:core:2.0:User",
                        name = "User",
                        description = "User Account",
                        attributes = new object[] {
                            new { name = "userName", type = "string", multiValued = false },
                            new { name = "externalId", type = "string", multiValued = false },
                            new { name = "active", type = "boolean", multiValued = false },
                            new {
                                name = "name",
                                type = "complex",
                                multiValued = false,
                                subAttributes = new object[] {
                                    new { name = "givenName", type = "string" },
                                    new { name = "familyName", type = "string" }
                                }
                            },
                            new {
                                name = "emails",
                                type = "complex",
                                multiValued = true,
                                subAttributes = new object[] {
                                    new { name = "value", type = "string" },
                                    new { name = "type", type = "string" },
                                    new { name = "primary", type = "boolean" }
                                }
                            }
                        }
                    }
                },
                totalResults = 1
            };

            return Results.Json(schemas, options: JsonSettings.ApiOptions);
        });
    }

    private static void MapResourceTypes(IEndpointRouteBuilder app)
    {
        app.MapGet("/scim/v2/ResourceTypes", () =>
        {
            var resourceTypes = new
            {
                schemas = new[] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" },
                Resources = new object[]
                {
                    new
                    {
                        id = "User",
                        name = "User",
                        endpoint = "/scim/v2/Users",
                        schema = "urn:ietf:params:scim:schemas:core:2.0:User",
                        schemaExtensions = Array.Empty<object>()
                    }
                },
                totalResults = 1
            };

            return Results.Json(resourceTypes, options: JsonSettings.ApiOptions);
        });
    }

    private static void MapRoot(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => new { status = "ok", service = "Mock SCIM 2.0" });
    }
}
