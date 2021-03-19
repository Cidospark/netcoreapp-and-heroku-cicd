using Microsoft.AspNetCore.Http;
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
        public IFormFile Photo { get; set; }
        [Required]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
