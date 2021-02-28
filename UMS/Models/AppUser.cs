using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; }


        public AppUser()
        {
            Photos = new List<Photo>();
        }
    }
}
