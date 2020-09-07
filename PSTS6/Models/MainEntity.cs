using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public abstract class MainEntity: BaseEntity
    {
        [DefaultValue(0)]
        public abstract int PrcCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Spent { get; set; }
    }
}
