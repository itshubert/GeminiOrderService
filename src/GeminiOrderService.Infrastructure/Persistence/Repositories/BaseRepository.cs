namespace GeminiOrderService.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly GeminiOrderDbContext _context;

    protected BaseRepository(GeminiOrderDbContext context)
    {
        _context = context;
    }
}