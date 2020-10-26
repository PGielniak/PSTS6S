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
        Task <List<Project>> GetProjects();
        Task <Project> GetProject(int? id);
        Task< List<ProjectTemplate>> GetProjectTemplates();
        Task <List<IdentityUser>> GetUsers();
        Task<Project> AddProject(Project project);
        Task<List<ProjectUser>> GetProjectUsers();
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(Project project);
        bool ProjectExists(int id);





        #endregion



    }
}
