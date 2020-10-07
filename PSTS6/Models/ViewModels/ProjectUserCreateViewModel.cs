using Microsoft.AspNetCore.Identity;
using PSTS6.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models.ViewModels
{
    public class ProjectUserCreateViewModel
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public IEnumerable<IdentityUser> SelectedUsers { get; set; }

        public IEnumerable<IdentityUser> NotSelectedUsers { get; set; }
    }
}
