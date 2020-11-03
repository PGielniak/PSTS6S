using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
  
    public class ProjectSearch
    {
        public int ID { get; set; }

        public string SearchString { get; set; }
    }
}
