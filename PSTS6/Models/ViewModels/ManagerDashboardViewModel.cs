using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class ManagerDashboardViewModel
    {
        public PagingList<Project> PendingProjects { get; set; }

        public PagingList<Project> OverBudgetProjects { get; set; }

        public PagingList<Project> FinishedProjects { get; set; }

      
    }
}
