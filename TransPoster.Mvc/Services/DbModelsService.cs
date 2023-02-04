using Microsoft.EntityFrameworkCore;
using TransPoster.Data;

namespace TransPoster.Mvc.Services;

public class DbModelsService<T> : IDbModelsService<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public DbModelsService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<T>> FindAllAsync() => await _context.Set<T>().ToListAsync();
}