using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PSTS6.Data;
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
        private readonly PSTS6Context _context;
        private readonly IHttpContextAccessor _http;

        public DbRepository(PSTS6Context context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

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

        public async Task<List<IdentityUser>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        #endregion

        #region ProjectTemplateMethods
        public async Task<List<ProjectTemplate>> GetProjectTemplates()
        {
            return await _context.ProjectTemplate.ToListAsync();
        }
        #endregion

        #region DashboardMethods

        public  IEnumerable<Activity> GetDashboardActivities(bool track, bool filteredByCurrentUser)
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
                    return  _context.Activity.AsNoTracking();
                    
                
            }

        }

        public IEnumerable<int> GetProjectSearchResults(string SearchString)
        {
            return _context.ProjectSearch.Where(x => EF.Functions.Like(x.SearchString, $"%{SearchString}%")).Select(x => x.ID).ToList();
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
            throw new NotImplementedException();
        }

        #endregion



    }
}
