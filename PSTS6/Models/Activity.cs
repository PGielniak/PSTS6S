using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Activity : MainEntity
    {

        private readonly int prcCompleted;
        private readonly decimal budget;
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
        [Column(TypeName= "decimal(18,2)")]
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
