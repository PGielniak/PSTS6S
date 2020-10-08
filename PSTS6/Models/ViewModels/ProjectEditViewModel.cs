using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using PSTS6.Data;
using PSTS6.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    [NotMapped]
    public class ProjectEditViewModel : BaseViewModel
    {
       
        public int ID { get; set; }

       

        public DateTime? ActualEndDate { get; set; }

        public IEnumerable<Task> Tasks { get; set; }

        public User ProjectManager { get; set; }

        public IEnumerable<SelectListItem> AvailableProjectManagers { get; set; }
        public IEnumerable<IdentityUser> Users { get; set; }

        public IEnumerable<IdentityUser> ProjectTeam { get; set; }

    }
}
