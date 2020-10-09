using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Activity : MainEntity
    {

      
        public Task Task { get; set; }

        public int TaskID { get; set; }


       
        public override int PrcCompleted { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public override decimal Budget { get; set; }

        public string Owner { get; set; }

    }
}
