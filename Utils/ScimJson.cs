using System.Text.Json.Nodes;
using MockScimServer.Domain;

namespace MockScimServer.Utils;

public static class ScimJson
{
    public static List<ScimEmail> ParseEmails(JsonNode? node)
    {
        var list = new List<ScimEmail>();
        if (node is JsonArray arr)
        {
            foreach (var i in arr)
            {
                list.Add(new ScimEmail
                {
                    Value = i?["value"]?.GetValue<string>() ?? string.Empty,
                    Type = i?["type"]?.GetValue<string>() ?? "work",
                    Primary = i?["primary"]?.GetValue<bool>() ?? false
                });
            }
        }
        return list;
    }
}
