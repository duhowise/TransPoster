﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransPoster.Mvc.Models.Users;
using TransPoster.Mvc.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransPoster.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IRoleService roleService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.FindAllUsersAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user is null) return NotFound();

            return View(new EditUserModel
            {
                Email = user.Email!,
                Role = user.Roles.FirstOrDefault()?.Name!,
                UserName = user.UserName!,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _userService.CreateUserAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

