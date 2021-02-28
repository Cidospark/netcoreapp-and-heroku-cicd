using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.Models
{
    public class AppUser : IdentityUser
    {
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; }


        public AppUser()
        {
            Photos = new List<Photo>();
        }
    }
}
