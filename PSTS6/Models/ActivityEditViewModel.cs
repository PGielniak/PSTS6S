using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class ActivityEditViewModel : BaseViewModel
    {

        public int ID { get; set; }

        public int TaskID { get; set; }


        public int PrcCompleted { get; set; }

        public decimal Budget { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public decimal Spent { get; set; }


    }
}
