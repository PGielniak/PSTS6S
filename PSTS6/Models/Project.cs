using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Project : MainEntity
    {
        private int prcCompleted;
        private decimal budget;

        public IEnumerable<Task> Tasks { get; set; }
        public User ProjectManager { get; set; }
        public IEnumerable<User> ProjectTeam { get; set; }
        public bool Completed
        {
            get
            {
                if (prcCompleted == 100)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            set { }


        }
        [DisplayName("Percentage Completed")]
        public override int PrcCompleted
        {
            get
            {
                double avg = 0;

                if (Tasks.Count()==0)
                {
                    avg = 0;
                }
          
                else
                {
                    avg = Tasks.Select(x => x.PrcCompleted).Average();
                }
                 

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
                
                if (Tasks.Select(x => x.Budget).Sum() == null)
                    return 0;
                
                else
                    return Tasks.Select(x => x.Budget).Sum();
            }
            set
            {
                budget = (decimal)Budget;
            }
        }

    }
}
