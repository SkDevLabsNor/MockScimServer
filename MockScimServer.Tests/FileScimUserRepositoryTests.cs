using MockScimServer.Models;
using MockScimServer.Persistence;
using Xunit;

namespace MockScimServer.Tests;

public class FileScimUserRepositoryTests
{
    [Fact]
    public void AddUpdateDelete_PersistsChangesToDisk()
    {
        var path = Path.Combine(Path.GetTempPath(), $"scim-users-{Guid.NewGuid()}.json");

        try
        {
            var repo = new FileScimUserRepository(path);

            var added = repo.Add(new ScimUser
            {
                Id       = Guid.NewGuid().ToString(),
                UserName = "alice",
                Emails   = new List<ScimEmail> { new() { Value = "alice@example.com", Primary = true } }
            });

            Assert.NotNull(repo.GetById(added.Id));

            var updated = repo.Update(added.Id, user =>
            {
                user.Active   = false;
                user.UserName = "alice.updated";
                user.Emails   = new List<ScimEmail> { new() { Value = "alice@new.com", Primary = true } };
            });

            Assert.True(updated);

            var reloaded = new FileScimUserRepository(path);
            var fromDisk = reloaded.GetById(added.Id);

            Assert.NotNull(fromDisk);
            Assert.Equal("alice.updated", fromDisk!.UserName);
            Assert.False(fromDisk.Active);
            Assert.Single(fromDisk.Emails);
            Assert.Equal("alice@new.com", fromDisk.Emails[0].Value);

            Assert.True(reloaded.Delete(added.Id));

            var afterDelete = new FileScimUserRepository(path);
            Assert.Empty(afterDelete.GetAll());
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void MissingFile_ReturnsEmptyList()
    {
        var path = Path.Combine(Path.GetTempPath(), $"scim-users-{Guid.NewGuid()}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var repo = new FileScimUserRepository(path);
        Assert.Empty(repo.GetAll());
    }
}
