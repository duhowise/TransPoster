using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransPoster.Mvc.Data.Products;
using TransPoster.Mvc.DataTables.Model;
using TransPoster.Mvc.Mvc;

namespace TransPoster.Mvc.Controllers;

[AllowAnonymous]
public sealed class ProductsController : Controller
{
    public ViewResult Index() => View();

    public async Task<JsonResult> IndexTable(AjaxDataRequest param, [FromServices] ProductViewModelProccesser proccesser)
    {
        var viewModels = await proccesser.ProccessAsync(param);
        return this.JsonDefaultContract(viewModels);

    }
}
