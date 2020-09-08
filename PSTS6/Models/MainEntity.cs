using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public abstract class MainEntity: BaseEntity
    {
        
        public abstract int PrcCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        [DataType(DataType.Currency)]
        public abstract decimal? Budget { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Spent { get; set; }
    }
}
