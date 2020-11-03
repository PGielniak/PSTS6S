using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSTS6.Models;
using PSTS6.Repository;

namespace PSTS6.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepository _repo;
        public SearchController(IRepository repo)
        {
            _repo = repo;
        }
        public IActionResult GetData(string SearchText)
        {
            var viewModel = new SearchViewModel();

            var projects = _repo.GetProjects(track: false, filter: false);

            var tasks = _repo.GetTasks();

            var activities = _repo.GetDashboardActivities(false, false);

            var projectTemplates = _repo.GetProjectTemplates();

            var users = _repo.GetUsers();

            return View(viewModel);
        }
    }
}
