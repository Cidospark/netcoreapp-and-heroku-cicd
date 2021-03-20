using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UMS.ViewModels;

namespace UMS.Controllers
{
    public class AdministrationController : Controller
    {
        private RoleManager<IdentityRole> _roleMgr;

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleMgr = roleManager;
        }
        public async Task<IActionResult> Index(string statusMessage, string roleId, bool err)
        {
            ViewBag.Msg = statusMessage;
            var roles = _roleMgr.Roles.ToList();

            if (err)
            {
                ViewBag.ModelErr = "Role name is empty!";
            }


            var lstRoles = new List<RolesViewModel>();
            if(roles != null)
            {
                foreach(var role in roles)
                {
                    var roleItem = new RolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    lstRoles.Add(roleItem);
                }
            }

            var model = new CreateRoleViewModel
            {
                ListOfRoles = lstRoles
            };
            if(roleId != null)
            {
                var roleToEdit = await _roleMgr.FindByIdAsync(roleId);
                model.NewRole = new RolesViewModel
                {
                    RoleId = roleToEdit.Id,
                    RoleName = roleToEdit.Name
                };
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var role = new IdentityRole
            {
                Name = model.RoleName
            };

            var result = await _roleMgr.CreateAsync(role);

            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View("Index", model);
            }

            return RedirectToAction("Index", "Administration", new { statusMessage = "success" });
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(CreateRoleViewModel model)
        {
            model.RoleName = model.NewRole.RoleName;

            if (string.IsNullOrWhiteSpace(model.RoleName))
                return RedirectToAction("Index", "Administration", new { err = true });

            var role = await _roleMgr.FindByIdAsync(model.NewRole.RoleId);

            if(role == null)
                return RedirectToAction("Index", "Administration", new { err = true });

            role.Name = model.NewRole.RoleName;

            var result = await _roleMgr.UpdateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View("Index", model);
            }

            return RedirectToAction("Index", "Administration", new { statusMessage = "edited" });
        }
    }
}
