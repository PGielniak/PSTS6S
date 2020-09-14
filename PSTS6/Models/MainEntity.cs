using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public abstract class MainEntity: BaseEntity
    {
        [DisplayName("Percentage Completed")]
        public abstract int PrcCompleted { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Estimated End Date")]
        public DateTime EstimatedEndDate { get; set; }

        [DisplayName("Actual End Date")]
        public DateTime? ActualEndDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public abstract decimal Budget { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Spent { get; set; }
    }
}
