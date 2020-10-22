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

        public DbRepository(PSTS6Context context)
        {
            _context = context;
        }

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

        public  async Task<Project> GetProject(int? id)
        {        
            return await _context.Project.Where(x => x.ID == id).Include(x => x.Tasks).FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjects()
        {
            return await _context.Project.ToListAsync();
        }

        public async Task<List<ProjectTemplate>> GetProjectTemplates()
        {
            return await _context.ProjectTemplate.ToListAsync();
        }

        public async Task<List<ProjectUser>> GetProjectUsers()
        {         
          return await _context.ProjectUsers.ToListAsync();
        }

        public async Task<List<IdentityUser>> GetUsers()
        {
            return await _context.Users.ToListAsync();
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
    }
}
