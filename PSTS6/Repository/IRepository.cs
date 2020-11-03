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
        Task <Project> GetProject(int? id);
        IEnumerable<ProjectTemplate> GetProjectTemplates();
        IEnumerable<IdentityUser> GetUsers();
        Task<Project> AddProject(Project project);
        Task<List<ProjectUser>> GetProjectUsers();
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(Project project);
        bool ProjectExists(int id);






        #endregion

        #region TaskMethods
        IEnumerable<PSTS6.Models.Task> GetTasks();
        Task<PSTS6.Models.Task> GetTask(int? id);
        Task<PSTS6.Models.Task> AddTask(PSTS6.Models.Task task);
        Task<PSTS6.Models.Task> UpdateTask(PSTS6.Models.Task task);
        Task<PSTS6.Models.Task> DeleteTask(PSTS6.Models.Task task);

        #endregion

        #region ActivityMethods

        IEnumerable<Activity> GetActivities(bool track, bool filteredByCurrentUser);
        #endregion

        #region SearchMethods
        IEnumerable<int> GetProjectSearchResults(string SearchString);
        IEnumerable<int> GetTaskSearchResults(string SearchString);
        IEnumerable<int> GetActivitySearchResults(string SearchString);
        IEnumerable<int> GetProjectTemplateSearchResults(string SearchString);
        IEnumerable<string> GetUserSearchResults(string SearchString);
        #endregion


    }
}
