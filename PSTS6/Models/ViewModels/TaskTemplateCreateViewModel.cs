using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class TaskTemplateCreateViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProjectTemplateID { get; set; }
    }
}
