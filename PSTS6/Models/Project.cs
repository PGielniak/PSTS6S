using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public override int PrcCompleted
        {
            get
            {

                var avg = Tasks.Select(x => x.PrcCompleted).Average();

                prcCompleted = Convert.ToInt32(avg);

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
                return Tasks.Select(x => x.Budget).Sum();
            }
            set
            {
                Budget = budget;
            }
        }

    }
}
