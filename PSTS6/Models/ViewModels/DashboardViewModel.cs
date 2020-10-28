using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class DashboardViewModel
    {

        public PagingList<Activity> PendingActivities { get; set; }

        public PagingList<Activity> OverBudgetActivities { get; set; }

        public PagingList<Activity> PlannedActivities { get; set; }

        public PagingList<Activity> FinishedActivities { get; set; }

        public PagingList<Project> PendingProjects { get; set; }

        public PagingList<Project> OverBudgetProjects { get; set; }

        public PagingList<Project> FinishedProjects { get; set; }

        public PagingList<IdentityUser> BestScorers { get; set; }







    }
}
