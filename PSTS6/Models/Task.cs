using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Task : Entity
    {
        private int prcCompleted;
        public Project Project { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public User Owner { get; set; }

        
        public override int PrcCompleted 
        {

            get
            {
                var avg = Activities.Select(x => x.PrcCompleted).Average();

                prcCompleted = Convert.ToInt32(avg);

                return prcCompleted;
            }
            set
            {
                PrcCompleted = prcCompleted;
            }

        }
    }
}
