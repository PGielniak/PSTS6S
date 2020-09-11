using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    [NotMapped]
    public class TaskEditViewModel : BaseViewModel
    {
        public int ID { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public IEnumerable<Activity> Activities { get; set; }



    }
}
