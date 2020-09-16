using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class ActivityTemplate: TemplateEntity
    {

        public int TaskTemplateID { get; set; }
        public decimal Budget { get; set; }
     
    }
}
