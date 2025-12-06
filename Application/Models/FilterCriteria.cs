namespace MockScimServer.Application.Models;

public class FilterCriteria
{
    public string? UserNameEquals { get; init; }
}

public class Paging
{
    public int StartIndex { get; init; } = 1;
    public int Count { get; init; } = int.MaxValue;
}
