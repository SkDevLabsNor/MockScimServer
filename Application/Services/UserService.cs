using System.Text.Json.Nodes;
using MockScimServer.Application.Models;
using MockScimServer.Domain;
using MockScimServer.Infrastructure.Persistence;
using MockScimServer.Utils;

namespace MockScimServer.Application.Services;

public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ScimUser> CreateUserAsync(ScimUser user)
    {
        EnsureUserName(user.UserName);

        var existing = await _repo.GetByUserNameAsync(user.UserName);
        if (existing is not null)
            throw new ScimErrorException(ScimErrors.Conflict("userName already exists"));

        await _repo.AddAsync(user);
        return user;
    }

    public Task<ScimUser?> GetUserAsync(string id) => _repo.GetByIdAsync(id);

    public async Task<UserListResult> ListUsersAsync(FilterCriteria filter, Paging paging)
    {
        var all = await _repo.ListAsync(filter, paging);
        var total = all.Count;

        var start = Math.Max(paging.StartIndex, 1);
        var count = paging.Count <= 0 ? int.MaxValue : paging.Count;

        var paged = all.Skip(start - 1).Take(count).ToList();

        return new UserListResult
        {
            Resources = paged,
            TotalResults = total,
            StartIndex = start,
            ItemsPerPage = paged.Count
        };
    }

    public async Task<ScimUser> PatchUserAsync(string id, IEnumerable<JsonNode?> operations)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) throw new ScimErrorException(ScimErrors.BadRequest("User not found"));

        var originalUserName = user.UserName;

        PatchApplier.Apply(user, operations);
        EnsureUserName(user.UserName);

        if (!string.Equals(originalUserName, user.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repo.GetByUserNameAsync(user.UserName);
            if (existing is not null && existing.Id != user.Id)
                throw new ScimErrorException(ScimErrors.Conflict("userName already exists"));
        }

        await _repo.UpdateAsync(user);
        return user;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }

    private static void EnsureUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ScimErrorException(ScimErrors.BadRequest("userName is required"));
    }
}
