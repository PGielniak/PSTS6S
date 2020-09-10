using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class TaskCreateViewModel : BaseViewModel
    {
       public string Owner { get; set; }

        public IEnumerable<SelectListItem> availableOwners { get; set; }

        public Project Project { get; set; }

        public int ProjectID { get; set; }

        public IEnumerable<SelectListItem> availableProjects { get; set; }
    }
}
