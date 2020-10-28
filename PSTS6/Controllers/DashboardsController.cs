using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.HelperClasses;
using PSTS6.Models;
using ReflectionIT.Mvc.Paging;

namespace PSTS6.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardsController(PSTS6Context context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task <IActionResult> Index(int plannedIndex = 1, int pendingIndex=1 )
        {
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("ProjectManager"))
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex));
            }
            else 
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex));
            }

            
        }

        #region GetUserDashboardData
        private async Task<DashboardViewModel> GetUserDashboardData(int plannedIndex, int pendingIndex)
        {
            var query = _context.Activity.AsNoTracking().OrderBy(s => s.Name);

            var pendingQuery = query
                .Where(x => x.PrcCompleted != 100 && x.ActualEndDate == null)
                .OrderBy(x => x.ActualEndDate);

            var pending = await PagingList.CreateAsync(pendingQuery, 2, pendingIndex);

            pending.PageParameterName = "pendingIndex";

            var overbudgetQuery = query
                .Where(x => x.Budget < x.Spent)
                .OrderBy(x => x.Spent);

            var overbudget = await PagingList.CreateAsync(overbudgetQuery, 2, 1);

            overbudget.PageParameterName = "overbudgetIndex";

            var plannedQuery = query
                .Where(x => x.StartDate < DateTime.Today)
                .OrderBy(x => x.StartDate);

            var planned = await PagingList.CreateAsync(query, 2, plannedIndex);

            overbudget.PageParameterName = "plannedIndex";

            var finishedQuery = query
                .Where(x => x.PrcCompleted == 100)
                .OrderBy(x => x.ActualEndDate);

            var finished = await PagingList.CreateAsync(finishedQuery, 2, 1);

            overbudget.PageParameterName = "finishedIndex";


            return new DashboardViewModel
            {
                PendingActivities = pending,
                PlannedActivities = planned,
                OverBudgetActivities = overbudget,
                FinishedActivities = finished
            };
        }
        #endregion

    }
}
