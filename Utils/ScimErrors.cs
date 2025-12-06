namespace MockScimServer.Utils;

public class ScimError
{
    public required string Detail { get; init; }
    public string? ScimType { get; init; }
    public int Status { get; init; } = StatusCodes.Status400BadRequest;
}

public class ScimErrorException : Exception
{
    public ScimError Error { get; }

    public ScimErrorException(ScimError error) : base(error.Detail)
    {
        Error = error;
    }
}

public static class ScimErrors
{
    public static ScimError BadRequest(string detail, string? scimType = null) =>
        new()
        {
            Detail = detail,
            ScimType = scimType,
            Status = StatusCodes.Status400BadRequest
        };

    public static ScimError Conflict(string detail) =>
        new()
        {
            Detail = detail,
            Status = StatusCodes.Status409Conflict,
            ScimType = "uniqueness"
        };

    public static IResult ToResult(this ScimError error) =>
        Results.Json(
            new
            {
                detail = error.Detail,
                scimType = error.ScimType,
                status = error.Status.ToString()
            },
            statusCode: error.Status);
}
