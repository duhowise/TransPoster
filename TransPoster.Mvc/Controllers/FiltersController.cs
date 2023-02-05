using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Interfaces;
using TransPoster.Mvc.Models;
using TransPoster.Mvc.Mvc;

namespace TransPoster.Mvc.Controllers;

[AllowAnonymous]
public sealed class FiltersController : Controller
{
    #region fields, ctor

    private readonly SampleDbContext db;

    public FiltersController(SampleDbContext db) => this.db = db ?? throw new ArgumentNullException(nameof(db));

    #endregion

    public Task<JsonResult> Categories() => GetFilterAsync(db.Categories);


    #region private

    private async Task<JsonResult> GetFilterAsync<TEntity>(IQueryable<TEntity> source)
          where TEntity : class, IIdName
    {
        var data = await GetFilterData<TEntity, int>(source);
        return this.JsonDefaultContract(data);
    }

    private static async Task<IEnumerable<SimpleIdName<TKey>>> GetFilterData<TEntity, TKey>( IQueryable<TEntity> source)
            where TEntity : class, IIdName<TKey>
    {
        var query = from x in source
                    orderby x.Name
                    select new SimpleIdName<TKey>
                    {
                        Id = x.Id,
                        Name = x.Name
                    };

        return await query.ToListAsync();
    }

    public sealed class SimpleIdName : IIdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class SimpleIdName<TKey> : IIdName<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
    }

    #endregion
}
