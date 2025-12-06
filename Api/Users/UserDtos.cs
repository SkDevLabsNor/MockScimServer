using MockScimServer.Domain;

namespace MockScimServer.Api.Users;

public record UserResponse(
    string[] Schemas,
    string Id,
    string ExternalId,
    string UserName,
    bool Active,
    ScimName Name,
    List<ScimEmail> Emails);

public record ListResponse(
    string[] Schemas,
    int TotalResults,
    int StartIndex,
    int ItemsPerPage,
    List<ScimUser> Resources);
