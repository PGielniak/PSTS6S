using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Project : MainEntity
    {
        private int prcCompleted;

        public IEnumerable<Task> Tasks { get; set; }
        public User ProjectManager { get; set; }
        public IEnumerable<User> ProjectTeam { get; set; }
        public bool Completed 
        {
            get
            {
                if (prcCompleted==100)
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
    }
}
