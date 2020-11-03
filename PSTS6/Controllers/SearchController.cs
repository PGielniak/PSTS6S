using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSTS6.Models;
using PSTS6.Repository;
using ReflectionIT.Mvc.Paging;

namespace PSTS6.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepository _repo;
       
        public SearchController(IRepository repo)
        {
            _repo = repo;
            
        }
        public async Task<IActionResult> GetData(string SearchText)
        {

            var projects = _repo.GetProjects(track: false, filter: false).AsQueryable()
                .Where(p=> _repo.GetProjectSearchResults(SearchText)
                .Contains(p.ID))
                .OrderBy(x=>x.Name);

            var tasks = _repo.GetTasks().AsQueryable()
                .Where(t => _repo.GetTaskSearchResults(SearchText)
                .Contains(t.ID))
                .OrderBy(t => t.Name);

            var activities = _repo.GetActivities(track: false, filteredByCurrentUser: false).AsQueryable()
                .Where(a => _repo.GetActivitySearchResults(SearchText)
                .Contains(a.ID))
                .OrderBy(a => a.Name);

            var projectTemplates = _repo.GetProjectTemplates().AsQueryable()
                .Where(pt => _repo.GetProjectTemplateSearchResults(SearchText)
                .Contains(pt.ID))
                .OrderBy(pt => pt.Name);

            var users = _repo.GetUsers().AsQueryable()
                .Where(u => _repo.GetUserSearchResults(SearchText)
                .Contains(u.Id))
                .OrderBy(u => u.UserName);

            var projectList = await PagingList.CreateAsync(projects, 100, 1);
            var taskList = await PagingList.CreateAsync(tasks, 100, 1);
            var activityList = await PagingList.CreateAsync(activities, 100, 1);
            var projectTemplateList = await PagingList.CreateAsync(projectTemplates, 100, 1);
            var usersList = await PagingList.CreateAsync(users, 100, 1);

            var viewModel = new SearchViewModel
            {
                Projects = projectList,
                Tasks = taskList,
                Activities = activityList,
                ProjectTemplates = projectTemplateList,
                Users = usersList
            };

            return View(viewModel);
        }
    }
}
