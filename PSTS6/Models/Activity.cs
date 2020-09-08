using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Activity : MainEntity
    {

        private int prcCompleted;
        private decimal budget;
        public Task Task { get; set; }

        
        [Range(0,15,ErrorMessage ="Prc Completed cannot be more than 100%")]
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
        [DataType(DataType.Currency)]
        public override decimal? Budget
        {
            get
            {
                return budget;
            }
            set
            {
                Budget = budget;

            }
        }
    }
}
