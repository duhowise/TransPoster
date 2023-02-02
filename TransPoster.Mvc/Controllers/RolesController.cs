using Microsoft.AspNetCore.Mvc;
using TransPoster.Mvc.Models.Roles;
using TransPoster.Mvc.Services;

namespace TransPoster.Mvc.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.FindAllAsync();
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (!ModelState.IsValid) return View();
            var role = await _roleService.CreateAsync(model);
            return Created("/Roles", role);
        }
    }
}

