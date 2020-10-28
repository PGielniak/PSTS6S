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
        private readonly HttpContextAccessor _http;

        public DbRepository(PSTS6Context context, HttpContextAccessor http)
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

        public async Task<List<Project>> GetProjects()
        {
            return await _context.Project.ToListAsync();
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

        public async  Task<IEnumerable<Activity>> GetDashboardActivities(bool track, bool filteredByCurrentUser)
        {
          
            switch (track,filteredByCurrentUser)
            {
                case (true, true):
                    return await _context.Activity.Where(x=>x.Owner==_http.HttpContext.User.Identity.Name).ToListAsync();
                   
                case (true, false):
                    return await _context.Activity.ToListAsync();
                    
                case (false, true):
                    return await _context.Activity.AsNoTracking().Where(x => x.Owner == _http.HttpContext.User.Identity.Name).ToListAsync();
                    
                case (false, false):
                    return await _context.Activity.AsNoTracking().ToListAsync();
                    
                
            }

        }

        #endregion



    }
}
