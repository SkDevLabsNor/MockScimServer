using System.Text.Json;
using System.Text.Json.Serialization;
using MockScimServer.Application.Models;
using MockScimServer.Domain;

namespace MockScimServer.Infrastructure.Persistence;

public class FileUserRepository : IUserRepository
{
    private readonly string _path;
    private readonly object _lock = new();
    private readonly JsonSerializerOptions _readOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly JsonSerializerOptions _writeOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private List<ScimUser> _users;

    public FileUserRepository(string path)
    {
        _path = path;
        _users = LoadUsers();
    }

    public Task<ScimUser?> GetByIdAsync(string id)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(Clone(user));
        }
    }

    public Task<ScimUser?> GetByUserNameAsync(string userName)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(Clone(user));
        }
    }

    public Task<List<ScimUser>> ListAsync(FilterCriteria filter, Paging paging)
    {
        lock (_lock)
        {
            IEnumerable<ScimUser> query = _users;

            if (!string.IsNullOrWhiteSpace(filter.UserNameEquals))
            {
                query = query.Where(u =>
                    string.Equals(u.UserName, filter.UserNameEquals, StringComparison.OrdinalIgnoreCase));
            }

            var list = query.Select(CloneNonNull).ToList();
            return Task.FromResult(list);
        }
    }

    public Task AddAsync(ScimUser user)
    {
        lock (_lock)
        {
            _users.Add(CloneNonNull(user));
            SaveUsers();
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(ScimUser user)
    {
        lock (_lock)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing is null) return Task.CompletedTask;

            Copy(user, existing);
            SaveUsers();
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        lock (_lock)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing is null) return Task.CompletedTask;

            _users.Remove(existing);
            SaveUsers();
        }

        return Task.CompletedTask;
    }

    private List<ScimUser> LoadUsers()
    {
        if (!File.Exists(_path))
            return new List<ScimUser>();

        try
        {
            var json = File.ReadAllText(_path);
            var users = JsonSerializer.Deserialize<List<ScimUser>>(json, _readOptions);
            return users ?? new List<ScimUser>();
        }
        catch
        {
            return new List<ScimUser>();
        }
    }

    private void SaveUsers()
    {
        var json = JsonSerializer.Serialize(_users, _writeOptions);
        File.WriteAllText(_path, json);
    }

    private static ScimUser? Clone(ScimUser? user) => user is null ? null : CloneNonNull(user);

    private static ScimUser CloneNonNull(ScimUser user)
    {
        return new ScimUser
        {
            Schemas = user.Schemas.ToArray(),
            Id = user.Id,
            ExternalId = user.ExternalId,
            UserName = user.UserName,
            Active = user.Active,
            Name = new ScimName
            {
                GivenName = user.Name.GivenName,
                FamilyName = user.Name.FamilyName
            },
            Emails = user.Emails
                .Select(e => new ScimEmail
                {
                    Value = e.Value,
                    Type = e.Type,
                    Primary = e.Primary
                })
                .ToList()
        };
    }

    private static void Copy(ScimUser source, ScimUser target)
    {
        target.Schemas = source.Schemas.ToArray();
        target.ExternalId = source.ExternalId;
        target.UserName = source.UserName;
        target.Active = source.Active;
        target.Name.GivenName = source.Name.GivenName;
        target.Name.FamilyName = source.Name.FamilyName;
        target.Emails = source.Emails
            .Select(e => new ScimEmail
            {
                Value = e.Value,
                Type = e.Type,
                Primary = e.Primary
            })
            .ToList();
    }
}
