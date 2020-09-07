using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Task : Entity
    {
        public Project Project { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public User Owner { get; set; }
        
    }
}
