using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Project>  Projects { get; set; }
        public IEnumerable<Task> Tasks { get; set; }

        public IEnumerable<Activity> Activities { get; set; }

        public IEnumerable<ProjectTemplate> ProjectTemplates { get; set; }

        public IEnumerable<IdentityUser> Users { get; set; }


    }
}
