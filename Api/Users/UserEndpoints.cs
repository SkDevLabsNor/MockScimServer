using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MockScimServer.Application.Models;
using MockScimServer.Application.Services;
using MockScimServer.Domain;
using MockScimServer.Infrastructure.Serialization;
using MockScimServer.Utils;

namespace MockScimServer.Api.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/scim/v2/Users", async (HttpRequest req, UserService service) =>
        {
            try
            {
                var filterString = req.Query["filter"].ToString();
                var startIndex = int.TryParse(req.Query["startIndex"], out var s) ? s : 1;
                var count = int.TryParse(req.Query["count"], out var c) ? c : int.MaxValue;

                var filter = FilterParser.Parse(filterString);
                var paging = new Paging { StartIndex = startIndex, Count = count };

                var result = await service.ListUsersAsync(filter, paging);

                return Results.Json(new ListResponse(
                    new[] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" },
                    result.TotalResults,
                    result.StartIndex,
                    result.ItemsPerPage,
                    result.Resources), options: JsonSettings.ApiOptions);
            }
            catch (ScimErrorException ex)
            {
                return ex.Error.ToResult();
            }
        });

        app.MapGet("/scim/v2/Users/{id}", async (string id, UserService service) =>
        {
            var user = await service.GetUserAsync(id);
            return user is null ? Results.NotFound() : Results.Json(ToResponse(user), options: JsonSettings.ApiOptions);
        });

        app.MapPost("/scim/v2/Users", async (HttpRequest req, UserService service) =>
        {
            try
            {
                var json = await JsonNode.ParseAsync(req.Body);
                var user = new ScimUser
                {
                    Id = Guid.NewGuid().ToString(),
                    ExternalId = json?["externalId"]?.GetValue<string>() ?? string.Empty,
                    UserName = json?["userName"]?.GetValue<string>() ?? string.Empty,
                    Active = json?["active"]?.GetValue<bool>() ?? true,
                    Name = new ScimName
                    {
                        GivenName = json?["name"]?["givenName"]?.GetValue<string>() ?? string.Empty,
                        FamilyName = json?["name"]?["familyName"]?.GetValue<string>() ?? string.Empty
                    },
                    Emails = ScimJson.ParseEmails(json?["emails"])
                };

                var created = await service.CreateUserAsync(user);

                return Results.Created($"/scim/v2/Users/{created.Id}", ToResponse(created));
            }
            catch (ScimErrorException ex)
            {
                return ex.Error.ToResult();
            }
        });

        app.MapMethods("/scim/v2/Users/{id}", new[] { "PATCH" }, async (string id, HttpRequest req, UserService service) =>
        {
            try
            {
                var json = await JsonNode.ParseAsync(req.Body);
                var ops = json?["Operations"]?.AsArray();
                if (ops is null) return Results.BadRequest();

                var updated = await service.PatchUserAsync(id, ops);
                return Results.Json(ToResponse(updated), options: JsonSettings.ApiOptions);
            }
            catch (ScimErrorException ex)
            {
                return ex.Error.ToResult();
            }
        });

        app.MapDelete("/scim/v2/Users/{id}", async (string id, UserService service) =>
        {
            var deleted = await service.DeleteUserAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }

    private static UserResponse ToResponse(ScimUser user) =>
        new(user.Schemas, user.Id, user.ExternalId, user.UserName, user.Active, user.Name, user.Emails);
}
