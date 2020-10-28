using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.HelperClasses;
using PSTS6.Models;
using PSTS6.Repository;
using ReflectionIT.Mvc.Paging;

namespace PSTS6.Controllers
{
    public class DashboardsController : Controller
    {
       
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repo;
        private readonly UserManager<IdentityUser> _userManager;
        public DashboardsController(PSTS6Context context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRepository repo, UserManager<IdentityUser> userManager)
        {
            
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _repo = repo;
            _userManager = userManager;
        }
        public async Task <IActionResult> Index(int plannedIndex = 1, int pendingIndex=1, int overbudgetIndex=1, int finishedIndex=1 )
        {
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("ProjectManager"))
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }
            else 
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }

            
        }

        #region GetUserDashboardData
        private async Task<DashboardViewModel> GetUserDashboardData(int plannedIndex, int pendingIndex, int overbudgetIndex, int finishedIndex)
        {
            var query = await _repo.GetDashboardActivities(track:false, filteredByCurrentUser: true);


            var pendingQuery = (IOrderedQueryable<Activity>)query
                .Where(x => x.PrcCompleted != 100 && x.ActualEndDate == null)
               
                .OrderBy(x => x.ActualEndDate);

            var pending = await PagingList.CreateAsync(qry: pendingQuery,pageSize: 2,pageIndex: pendingIndex);

            pending.PageParameterName = "pendingIndex";

            var overbudgetQuery = (IOrderedQueryable<Activity>)query
                .Where(x => x.Budget < x.Spent)
                .OrderBy(x => x.Spent);

            var overbudget = await PagingList.CreateAsync(overbudgetQuery, 2, overbudgetIndex);

            overbudget.PageParameterName = "overbudgetIndex";

            var plannedQuery = (IOrderedQueryable<Activity>)query
                .Where(x => x.StartDate < DateTime.Today)
                .OrderBy(x => x.StartDate);

            var planned = await PagingList.CreateAsync(plannedQuery, 2, plannedIndex);

            overbudget.PageParameterName = "plannedIndex";

            var finishedQuery = (IOrderedQueryable<Activity>)query
                .Where(x => x.PrcCompleted == 100)
                .OrderBy(x => x.ActualEndDate);

            var finished = await PagingList.CreateAsync(finishedQuery, 2, finishedIndex);

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
