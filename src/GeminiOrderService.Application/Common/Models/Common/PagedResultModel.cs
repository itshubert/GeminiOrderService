namespace GeminiOrderService.Application.Common.Models.Common;

/// <summary>
/// Generic paged result model for application layer
/// </summary>
/// <typeparam name="T">The type of items in the result</typeparam>
public record PagedResultModel<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
};