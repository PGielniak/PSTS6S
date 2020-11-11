using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PSTS6.Data;
using PSTS6.HelperClasses;
using PSTS6.Models;
using PSTS6.Models.IdentityModels;
using PSTS6.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Repository
{
    public class DbRepository : IRepository
    {

        #region Private Fields
        private readonly PSTS6Context _context;
        private readonly IHttpContextAccessor _http;
        private readonly BackgroundCalculations _backgroundCalculations;
        #endregion

        #region Constructor
        public DbRepository(PSTS6Context context, IHttpContextAccessor http, BackgroundCalculations backgroundCalculations)
        {
            _context = context;
            _http = http;
            _backgroundCalculations = backgroundCalculations;
        }
        #endregion

        #region ProjectMethods
        public async Task<Project> AddProject(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }



        public async Task<Project> DeleteProject(Project project)
        {
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> GetProject(int? id)
        {
            return await _context.Project.Where(x => x.ID == id).Include(x => x.Tasks).FirstOrDefaultAsync();
        }

        public  IEnumerable<Project> GetProjects(bool track, bool filter)
        {
           // return await _context.Project.ToListAsync();

            switch (track, filter)
            {
                case (true, true):
                    return  _context.Project.Where(x => x.ProjectManager == _http.HttpContext.User.Identity.Name);

                case (true, false):
                    return _context.Project;

                case (false, true):
                    return _context.Project.AsNoTracking().Where(x => x.ProjectManager == _http.HttpContext.User.Identity.Name);

                case (false, false):
                    return _context.Project.AsNoTracking();


            }
        }
        public async Task<List<Project>> GetProjectsAsync(bool filterByUser)
        {
            switch (filterByUser)
            {
                case (true):
                    return await _context.Project.AsNoTracking().Where(x => x.ProjectManager == _http.HttpContext.User.Identity.Name).ToListAsync();
                default:
                    return await _context.Project.AsNoTracking().ToListAsync();
            }
            
        }

        public bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }

        public async Task<Project> UpdateProject(Project project)
        {
            _context.Attach(project);
            _context.Update(project);
            await _context.SaveChangesAsync();

            return project;
        }

        #endregion

        #region UserMethods
        public async Task<List<ProjectUser>> GetProjectUsers()
        {
            return await _context.ProjectUsers.ToListAsync();
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            return  _context.Users.AsNoTracking();
        }

        public async Task<List<IdentityUser>> GetUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        #endregion

        #region ProjectTemplateMethods
        public IEnumerable<ProjectTemplate> GetProjectTemplates()
        {
            return  _context.ProjectTemplate.AsNoTracking();
        }

        public async Task<ProjectTemplate> GetProjectTemplateAndIncludeAll(string id)
        {
            return await _context.ProjectTemplate.Where(x => x.ID == Convert.ToInt32(id)).Include(x => x.TaskTemplates).ThenInclude(y => y.ActivityTemplates).FirstOrDefaultAsync();
        }


        #endregion

        #region ActivityMethods

        public  IEnumerable<Activity> GetActivities(bool track = false, bool filteredByCurrentUser = false)
        {
          
            switch (track,filteredByCurrentUser)
            {
                case (true, true):
                    return  _context.Activity.Where(x=>x.Owner==_http.HttpContext.User.Identity.Name);
                   
                case (true, false):
                    return  _context.Activity;
                    
                case (false, true):
                    return  _context.Activity.AsNoTracking().Where(x => x.Owner == _http.HttpContext.User.Identity.Name);
                    
                case (false, false):
                    return  _context.Activity.AsNoTracking().Include(x=>x.Task);
                    
                
            }

        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync(bool filterByUser)
        {
            switch (filterByUser)
            {
                case (true):
                    return await _context.Activity.AsNoTracking().Where(x => x.Owner == _http.HttpContext.User.Identity.Name).ToListAsync();
                default:
                    return await _context.Activity.AsNoTracking().ToListAsync();
            }
            
        }
        public async Task<Activity> GetActivityAsync(int? id)
        {
            return await _context.Activity.Where(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Activity> GetActivityAndLoadRelatedDataAsync(int id)
        {
            return await _context.Activity.Include(x => x.Task).ThenInclude(x => x.Project).Where(q => q.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Activity> AddActivityAsync(Activity activity)
        {
            _context.Add(activity);
            await _context.SaveChangesAsync();

            return activity;
        }

        public async Task<Activity> UpdateActivityAsync(Activity activity)
        {
            _context.Update(activity);
            _backgroundCalculations.UpdateBudget(activity);
            await _context.SaveChangesAsync();

            return activity;
        }

        public async Task<Activity> DeleteActivityAsync(Activity activity)
        {
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();

            return activity;
        }
        public bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.ID == id);
        }


        #endregion

        #region SearchMethods
        public IEnumerable<int> GetProjectSearchResults(string SearchString)
        {
            return _context.ProjectSearch.AsNoTracking().Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.ID);
        }

        public IEnumerable<int> GetTaskSearchResults(string SearchString)
        {
            return _context.TaskSearch.Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.ID).ToList();
        }

        public IEnumerable<int> GetActivitySearchResults(string SearchString)
        {
            return _context.ActivitySearch.Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.ID).ToList();
        }

        public IEnumerable<int> GetProjectTemplateSearchResults(string SearchString)
        {
            return _context.ProjectTemplateSearch.Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.ID).ToList();
        }

        public IEnumerable<string> GetUserSearchResults(string SearchString)
        {
            return _context.UserSearch.Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.Id).ToList();
        }
        #endregion

        #region TaskMethods
        public IEnumerable<Models.Task> GetTasks()
        {
            return _context.Task.AsNoTracking().Include(x=>x.Project);
        }

        public async Task<List<Models.Task>> GetTasksAsync()
        {
            return await _context.Task.ToListAsync();
        }

        public async Task<Models.Task> GetTask(int? id)
        {
            return await _context.Task.Where(x => x.ID == id).Include(x => x.Activities).FirstOrDefaultAsync();
        }

        public async Task<Models.Task> AddTask(Models.Task task)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<Models.Task> UpdateTask(Models.Task task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<Models.Task> DeleteTask(Models.Task task)
        {
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.ID == id);
        }

     





        #endregion



    }
}
