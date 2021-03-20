using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public List<RolesViewModel> ListOfRoles { get; set; }

        public RolesViewModel NewRole { get; set; }

        public CreateRoleViewModel()
        {
            ListOfRoles = new List<RolesViewModel>();
        }
    }
}
