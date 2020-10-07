using PSTS6.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models.MainModels
{
    public class ProjectUser
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public string UserID { get; set; }

        public User User { get; set; }
    }
}
