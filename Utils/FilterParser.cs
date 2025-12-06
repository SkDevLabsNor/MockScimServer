using MockScimServer.Application.Models;
using MockScimServer.Utils;

namespace MockScimServer.Utils;

public static class FilterParser
{
    private const string UserNamePrefix = "userName eq ";

    public static FilterCriteria Parse(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return new FilterCriteria();

        if (filter.StartsWith(UserNamePrefix, StringComparison.OrdinalIgnoreCase))
        {
            var value = filter.Substring(UserNamePrefix.Length).Trim();
            if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
            {
                value = value[1..^1];
            }

            return new FilterCriteria { UserNameEquals = value };
        }

        throw new ScimErrorException(ScimErrors.BadRequest("Unsupported filter"));
    }
}
