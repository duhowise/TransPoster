using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransPoster.Data.Models;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleModel model)
        {
            if (!ModelState.IsValid) return View();
            var role = await _roleService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id)
        {
            var role = _roleService.GetIdentityRoleAsync(id);
            if (role is null) return RedirectToAction(nameof(Index));
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]string id, ApplicationRole identityRole)
        {
            try
            {
                var result = await _roleService.UpdateRoleAsync(id, identityRole);
                return Ok(result);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _roleService.DeleteRoleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

