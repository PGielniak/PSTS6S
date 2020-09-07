using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Entity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PrcCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Spent { get; set; }

    }
}
