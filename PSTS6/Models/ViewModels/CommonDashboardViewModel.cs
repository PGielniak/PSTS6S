using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models.ViewModels
{
    public class CommonDashboardViewModel
    {
        public UserDashboardViewModel UserDashboardViewModel { get; set; }

        public ManagerDashboardViewModel ManagerDashboardViewModel { get; set; }
    }
}
