using Microsoft.AspNetCore.Identity;
using PSTS6.Models.MainModels;
using System.Collections;
using System.Collections.Generic;

namespace PSTS6.Models.IdentityModels
{
    public class User : IdentityUser
    {
        public ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}