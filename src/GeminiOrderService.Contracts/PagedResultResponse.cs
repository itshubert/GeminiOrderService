namespace GeminiOrderService.Contracts;

public record PagedResultResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);