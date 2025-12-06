using MockScimServer.Domain;

namespace MockScimServer.Application.Models;

public class UserListResult
{
    public required List<ScimUser> Resources { get; init; }
    public required int TotalResults { get; init; }
    public int StartIndex { get; init; }
    public int ItemsPerPage { get; init; }
}
