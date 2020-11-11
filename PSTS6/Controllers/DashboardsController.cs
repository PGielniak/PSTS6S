using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.Models.ViewModels;
using PSTS6.Repository;
using ReflectionIT.Mvc.Paging;

namespace PSTS6.Controllers
{
    public class DashboardsController : Controller
    {

        #region Private Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repo;
        #endregion

        #region Constructor
        public DashboardsController(IHttpContextAccessor httpContextAccessor, IRepository repo)
        {
            _httpContextAccessor = httpContextAccessor;
            _repo = repo;
        }
        #endregion

        #region Controller Action
        public async Task<IActionResult> Index(int plannedIndex = 1, int pendingIndex = 1, int overbudgetIndex = 1, int finishedIndex = 1)
        {
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return View(await GetAdminDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("ProjectManager"))
            {
                return View(await GetProjectManagerDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }
            else
            {
                return View(await GetUserDashboardData(plannedIndex, pendingIndex, overbudgetIndex, finishedIndex));
            }


        }
        #endregion

        #region GetUserDashboardData
        private async Task<CommonDashboardViewModel> GetUserDashboardData(int plannedIndex, int pendingIndex, int overbudgetIndex, int finishedIndex)
        {
            
            var query = await _repo.GetActivitiesAsync();

            var pendingQuery = query.AsQueryable()
                .Where(x => x.PrcCompleted != 100 && x.ActualEndDate == null)
                .OrderBy(x => x.ActualEndDate);

            var pending =  PagingList.Create(qry: pendingQuery, pageSize: 2, pageIndex: pendingIndex);

            pending.PageParameterName = "pendingIndex";

            var overbudgetQuery = query.AsQueryable()
                .Where(x => x.Budget < x.Spent)
                .OrderBy(x => x.Spent);

            var overbudget = PagingList.Create(overbudgetQuery, 2, overbudgetIndex);

            overbudget.PageParameterName = "overbudgetIndex";

            var plannedQuery = query.AsQueryable()
                .Where(x => x.StartDate < DateTime.Today)
                .OrderBy(x => x.StartDate);

            var planned = PagingList.Create(plannedQuery, 2, plannedIndex);

            overbudget.PageParameterName = "plannedIndex";

            var finishedQuery = query.AsQueryable()
                .Where(x => x.PrcCompleted == 100)
                .OrderBy(x => x.ActualEndDate);

            var finished = PagingList.Create(finishedQuery, 2, finishedIndex);

            overbudget.PageParameterName = "finishedIndex";


            return new CommonDashboardViewModel
            {
                UserDashboardViewModel = new UserDashboardViewModel
                {
                    PendingActivities = pending,
                    PlannedActivities = planned,
                    OverBudgetActivities = overbudget,
                    FinishedActivities = finished
                }
        };
           
        }
        #endregion

        #region GetPMDashboardData
        private async Task<CommonDashboardViewModel> GetProjectManagerDashboardData(int plannedIndex, int pendingIndex, int overbudgetIndex, int finishedIndex)
        {
           
            var query = _repo.GetProjects(track: false, filter: false);

            var pendingQuery = query.AsQueryable()
                .Where(x => x.PrcCompleted != 100 && x.ActualEndDate == null)
                .OrderBy(x => x.ActualEndDate);

            var pending = await PagingList.CreateAsync(qry: pendingQuery, pageSize: 2, pageIndex: pendingIndex);

            pending.PageParameterName = "pendingIndex";

            var overbudgetQuery = query.AsQueryable()
                .Where(x => x.Budget < x.Spent)
                .OrderBy(x => x.Spent);

            var overbudget = await PagingList.CreateAsync(overbudgetQuery, 2, overbudgetIndex);

            overbudget.PageParameterName = "overbudgetIndex";        

            var finishedQuery = query.AsQueryable()
                .Where(x => x.PrcCompleted == 100)
                .OrderBy(x => x.ActualEndDate);

            var finished = await PagingList.CreateAsync(finishedQuery, 2, finishedIndex);

            overbudget.PageParameterName = "finishedIndex";

            return new CommonDashboardViewModel
            {
                ManagerDashboardViewModel= new ManagerDashboardViewModel
              {
                 PendingProjects = pending,             
                 OverBudgetProjects = overbudget,
                 FinishedProjects = finished
              }
            };
            
        }
        #endregion

        #region GetAdminDashboardData
        private async Task<CommonDashboardViewModel> GetAdminDashboardData(int i=1, int j=1, int k=1, int l=1)
        {
            var userViewModel = await GetUserDashboardData(i,j,k,l);
            var pmViewModel = await GetProjectManagerDashboardData(i, j, k, l);

            var adminViewModel = new CommonDashboardViewModel
            {
                ManagerDashboardViewModel = pmViewModel.ManagerDashboardViewModel,
                UserDashboardViewModel = userViewModel.UserDashboardViewModel
            };

            return adminViewModel;

        }
        #endregion

    }
}
