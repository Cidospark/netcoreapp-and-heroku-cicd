using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.ViewModels
{
    public class UserToReturn
    {
        public List<string> _userMgr;

        public UserToReturn(UserManager<AppUser> usermgr)
        {
            _userMgr = usermgr.Users.Select(x => x.Email).ToList();

        }
        public IFormFile Photo { get; set; }
        [Required]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }

        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller:"Account")]
        //[IfEmailIsUsed(ErrorMessage = "Email is already taken!")]
        public string Email { get; set; }
    }

    //public class IfEmailIsUsedAttribute: ValidationAttribute
    //{

    //    public override bool IsValid(object value)
    //    {
    //        // how to get the userManager into this env was the challenge
    //        var email = value.ToString();
    //        var user = _userMgr.Users.FirstOrDefault(x => x.Email == email);
    //        if(user == null)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //}
}
