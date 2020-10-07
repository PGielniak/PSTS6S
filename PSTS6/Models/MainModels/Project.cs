using PSTS6.Models.IdentityModels;
using PSTS6.Models.MainModels;
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


        public IEnumerable<Task> Tasks { get; set; }
        public string ProjectManager { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public bool Completed
        {
            get
            {
                if (PrcCompleted == 100)
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
