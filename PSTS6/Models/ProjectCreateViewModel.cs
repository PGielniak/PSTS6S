using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class ProjectCreateViewModel : BaseViewModel
    {
      

        public string ProjectManager { get; set; }
        public IEnumerable<SelectListItem> availableProjectManagers { get; set; }
    }
}
