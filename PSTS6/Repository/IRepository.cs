using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PSTS6.Models;
using PSTS6.Models.IdentityModels;
using PSTS6.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Repository
{
    public interface IRepository
    {

        #region ProjectMethods
        IEnumerable<Project> GetProjects(bool track, bool filter);
        Task<List<Project>> GetProjectsAsync();

        Task <Project> GetProject(int? id);
        IEnumerable<ProjectTemplate> GetProjectTemplates();
        
        Task<Project> AddProject(Project project);
        
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(Project project);
        bool ProjectExists(int id);






        #endregion

        #region TaskMethods
        IEnumerable<PSTS6.Models.Task> GetTasks();
        Task<List<Models.Task>> GetTasksAsync();
        Task<PSTS6.Models.Task> GetTask(int? id);
        Task<PSTS6.Models.Task> AddTask(PSTS6.Models.Task task);
        Task<PSTS6.Models.Task> UpdateTask(PSTS6.Models.Task task);
        Task<PSTS6.Models.Task> DeleteTask(PSTS6.Models.Task task);
        bool TaskExists(int id);

        #endregion

        #region ActivityMethods

        IEnumerable<Activity> GetActivities(bool track, bool filteredByCurrentUser);
        Task<Activity> AddActivity(Activity activity);
        #endregion

        #region ProjectTemplateMethods
        Task<ProjectTemplate> GetProjectTemplateAndIncludeAll(string id);

        #endregion

        #region SearchMethods
        IEnumerable<int> GetProjectSearchResults(string SearchString);
        IEnumerable<int> GetTaskSearchResults(string SearchString);
        IEnumerable<int> GetActivitySearchResults(string SearchString);
        IEnumerable<int> GetProjectTemplateSearchResults(string SearchString);
        IEnumerable<string> GetUserSearchResults(string SearchString);
        #endregion

        #region UserMethods
        IEnumerable<IdentityUser> GetUsers();
        Task<List<IdentityUser>> GetUsersAsync();
        Task<List<ProjectUser>> GetProjectUsers();
        #endregion

    }
}
