using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UMS.Models;
using UMS.ViewModels;

namespace UMS.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<AppUser> _userMgr;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleMgr = roleManager;
            _userMgr = userManager;
        }

        public async Task<IActionResult> Index(string statusMessage, string roleId, bool showUsers, bool err)
        {
            ViewBag.Msg = statusMessage;
            ViewBag.ShowRoleUsers = "false";
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

            var roleToEdit = await _roleMgr.FindByIdAsync(roleId);
            if(roleId != null && showUsers == true)
            {
                var users = await _userMgr.GetUsersInRoleAsync(roleToEdit.Name);
                foreach (var roleUser in users)
                {
                    var roleUserToReturn = new RoleUsersViewModel
                    {
                        UserId = roleUser.Id,
                        Username = roleUser.UserName,
                        IsInRole = true
                    };

                    model.ListOfRoleUsers.Add(roleUserToReturn);
                }
                model.RoleName = roleToEdit.Name;
                ViewBag.ShowRoleUsers = "true";
            }else if(roleId != null && showUsers == false)
            {
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


        [HttpGet][HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if(string.IsNullOrWhiteSpace(roleId))
                return RedirectToAction("Index", "Administration", new { statusMessage = "failed" });

            var role = await _roleMgr.FindByIdAsync(roleId);
            if (role == null)
                return RedirectToAction("Index", "Administration", new { statusMessage = "failed" });

            var result = await _roleMgr.DeleteAsync(role);
            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return RedirectToAction("Index", "Administration", new { statusMessage = "failed" });
            }

            return RedirectToAction("Index", "Administration");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRoleUsers(CreateRoleViewModel model)
        {
            if(!ModelState.IsValid)
                return RedirectToAction("Index", "Administration", new { statusMessage = "invalid" });

            foreach(var RoleUser in model.ListOfRoleUsers)
            {
                var user = await _userMgr.FindByIdAsync(RoleUser.UserId);
                await _userMgr.RemoveFromRoleAsync(user, model.RoleName);
            }

            foreach (var RoleUser in model.ListOfRoleUsers)
            {
                var user = await _userMgr.FindByIdAsync(RoleUser.UserId);
                if (RoleUser.IsInRole)
                {
                    await _userMgr.AddToRoleAsync(user, model.RoleName);
                }
            }

            return RedirectToAction("Index", "Administration");

        }
    }
}
