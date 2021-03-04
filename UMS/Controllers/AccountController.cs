﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UMS.Models;
using UMS.ViewModels;

namespace UMS.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userRepo;
        private readonly IHostingEnvironment _hostEnv;

        public AccountController(UserManager<AppUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userRepo = userManager;
            _hostEnv = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string photoPath)
        {
            throw new Exception("This page must not LOAD!");
            ViewBag.PhotoPath = photoPath;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UserToReturn model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Model state is invalid";
                return View("index");
            }

            var photoName = "";
            if(model.Photo != null)
            {
                // get folder name and file name
                var rootAddress = _hostEnv.WebRootPath.ToString();
                var foldername = rootAddress+"/images";
                photoName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                // get the full path
                var fullPath = Path.Combine(foldername, photoName);

                // copy file to the file path
                using(var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            // create a list and add the file name to it
            List<Photo> lstOfPhotos = new List<Photo>
            {
                new Photo { Url = photoName }
            };

            // create the user
            var user = new AppUser
            {
                UserName = model.Username.Trim(),
                Photos = lstOfPhotos
            };

            var result = await _userRepo.CreateAsync(user, "P@$$w0rd1");
            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View("index", model);
            }

            ViewBag.UserPhoto = photoName;
            ViewBag.Username = user.UserName;

            return View("index");
        }
    }
}
