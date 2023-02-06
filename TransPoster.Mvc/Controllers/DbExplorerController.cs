using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransPoster.Data;
using TransPoster.Mvc.DataTables;
using TransPoster.Mvc.DataTables.Model;
using TransPoster.Mvc.Mvc;

namespace TransPoster.Mvc.Controllers;

[AllowAnonymous]
public sealed class DbExplorerController : Controller
{
    public ViewResult Index(string typeName) => View();

    public async Task<JsonResult> IndexTable(string typeName, AjaxDataRequest param, [FromServices] ApplicationDbContext db)
    {
        var proccesser = new DbSetProccesser(db);
        var viewModels = await proccesser.ProccessAsync(typeName, param);
        return this.JsonDefaultContract(viewModels);
    }

}
