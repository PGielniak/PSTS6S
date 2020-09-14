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
        private readonly int prcCompleted;
   

        public IEnumerable<Task> Tasks { get; set; }
        public string ProjectManager { get; set; }
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
            get;set;
        }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public override decimal Budget
        {
            get;set;
        }

    }
}
