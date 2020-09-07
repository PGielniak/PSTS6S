using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Activity : Entity
    {

        private int prcCompleted;
        public Task Task { get; set; }

        
        public override int PrcCompleted 
        {

            get     
            {
                return prcCompleted;
            }

            set 
            {
                PrcCompleted = prcCompleted;
            }
        
        }
    }
}
