using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Task : MainEntity
    {
        private int prcCompleted;
        private decimal? budget;
        public Project Project { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public User Owner { get; set; }

        [Range(0,100,ErrorMessage ="Prc Completed cannot be more than 100%")]
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
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public override decimal? Budget
        {
            get
            {
                return Activities.Select(x => x.Budget).Sum();
            }
            set
            {
                Budget = budget;
            }
        }
    }
}
