using MockScimServer.Application.Models;
using MockScimServer.Domain;

namespace MockScimServer.Infrastructure.Persistence;

public interface IUserRepository
{
    Task<ScimUser?> GetByIdAsync(string id);
    Task<ScimUser?> GetByUserNameAsync(string userName);
    Task<List<ScimUser>> ListAsync(FilterCriteria filter, Paging paging);
    Task AddAsync(ScimUser user);
    Task UpdateAsync(ScimUser user);
    Task DeleteAsync(string id);
}
