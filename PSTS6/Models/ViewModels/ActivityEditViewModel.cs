using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PSTS6.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class ActivityEditViewModel : BaseViewModel
    {

        public int ID { get; set; }

        public int TaskID { get; set; }

        [Range(0, 100, ErrorMessage = "Prc Completed cannot be more than 100%")]
        public int PrcCompleted { get; set; }

        public decimal Budget { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public decimal Spent { get; set; }


        public string Owner { get; set; }

        public IEnumerable<SelectListItem> AvailableOwners { get; set; }
        public IEnumerable<IdentityUser> Users { get; set; }

    }
}
