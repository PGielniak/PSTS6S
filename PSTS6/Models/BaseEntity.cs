using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage ="You have to specify the name")]
        [StringLength(250)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [StringLength(500)]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
