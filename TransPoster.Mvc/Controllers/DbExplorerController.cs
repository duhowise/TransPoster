using Microsoft.AspNetCore.Mvc;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Controllers
{
    public class DbExplorerController : Controller
    {
        private readonly IConfiguration _config;
        public DbExplorerController(IConfiguration config) => _config = config;

        //[HttpPost]
        //public IActionResult Products()
        //{
        //    using var db = new Database("sqlserver", _config.GetConnectionString("DefaultConnection"), "Microsoft.Data.SqlClient");

        //    var response = new Editor(db, "Products")
        //        .Model<Product>()
        //        .Process(Request.Form)
        //        .Data();

        //    return Json(response);
        //}

        //public IActionResult Orders()
        //{
        //    using var db = new Database("sqlserver", _config.GetConnectionString("DefaultConnection"), "Microsoft.Data.SqlClient");

        //    var response = new Editor(db, "Orders")
        //        .Model<Order>()
        //        .Process(Request.Form)
        //        .Data();

        //    return Json(response);
        //}
    }
}
