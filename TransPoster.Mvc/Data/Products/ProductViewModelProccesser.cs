using System.Linq.Expressions;
using TransPoster.Data;
using TransPoster.Data.Models;
using TransPoster.Mvc.DataTables;
using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.Data.Products;

public sealed class ProductViewModelProccesser : RemoteProccesserBase<Product, ProductViewModel>
{
    private readonly ApplicationDbContext db;

    public ProductViewModelProccesser(ApplicationDbContext db)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public Task<AjaxData> ProccessAsync(AjaxDataRequest param) => base.ProccessAsync(db.Products, param);

    protected override Expression<Func<Product, ProductViewModel>> Convert() => model => new ProductViewModel
    {
        Id = model.Id,
        Name = model.Name,
        CreatedAt = model.CreatedAt
        
    };

}