using Microsoft.EntityFrameworkCore;
using TransPoster.Data;

namespace TransPoster.Mvc.Services;

public class DbManagementService : IDbManagementService
{
    private readonly ApplicationDbContext _context;

    public DbManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<string?> FindAllTableNamesAsync() => _context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct()
                .ToList();

}