using Microsoft.AspNetCore.Mvc.Rendering;
using PSTS6.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class UserEditViewModel
    {

        public string UserName { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> AvailableRoles{ get; set; }

        public IEnumerable<Roles> Role { get; set; }
    }
}
