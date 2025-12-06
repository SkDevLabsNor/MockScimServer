using System.Text.Json.Nodes;
using MockScimServer.Domain;
using MockScimServer.Utils;

namespace MockScimServer.Utils;

public static class PatchApplier
{
    public static void Apply(ScimUser user, IEnumerable<JsonNode?> operations)
    {
        foreach (var op in operations)
        {
            var path = op?["path"]?.GetValue<string>() ?? string.Empty;
            var value = op?["value"];

            switch (path)
            {
                case "userName":
                    user.UserName = value?.GetValue<string>() ?? user.UserName;
                    break;

                case "active":
                    user.Active = value?.GetValue<bool>() ?? user.Active;
                    break;

                case "name.givenName":
                    user.Name.GivenName = value?.GetValue<string>() ?? user.Name.GivenName;
                    break;

                case "name.familyName":
                    user.Name.FamilyName = value?.GetValue<string>() ?? user.Name.FamilyName;
                    break;

                case "emails":
                    user.Emails = ScimJson.ParseEmails(value);
                    break;

                case "externalId":
                    user.ExternalId = value?.GetValue<string>() ?? user.ExternalId;
                    break;
            }
        }
    }
}
