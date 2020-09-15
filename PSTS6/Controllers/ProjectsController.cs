using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;

namespace PSTS6.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly PSTS6Context _context;

        

        public ProjectsController(PSTS6Context context)
        {
            _context = context;
            
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            var dbUsers = await _context.Users.ToListAsync();

            IEnumerable<SelectListItem> users = dbUsers.Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.UserName
            });

            var viewModel = new ProjectCreateViewModel();

            viewModel.availableProjectManagers = users;
            viewModel.StartDate = DateTime.Today;
            viewModel.EstimatedEndDate = DateTime.Today;
            return View(viewModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Completed,PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,ProjectManager")] Project project)
        {
            //var tasks = _context.Task.ToList();


            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           

            var project = await _context.Project.Where(x=>x.ID==id).Include(x=>x.Tasks).FirstOrDefaultAsync();
            
            var dbUsers = await _context.Users.ToListAsync();

            IEnumerable<SelectListItem> users = dbUsers.Select(x => new SelectListItem
            {
                Text=x.UserName,
                Value=x.UserName
            });

            var viewModel = new ProjectEditViewModel();



            viewModel.ID = project.ID;
            viewModel.Name = project.Name;
            viewModel.Description = project.Description;
            viewModel.ActualEndDate = project.ActualEndDate;
            viewModel.EstimatedEndDate = project.EstimatedEndDate;
            viewModel.StartDate = project.StartDate;
            viewModel.Tasks = project.Tasks.ToList();
            viewModel.Users = dbUsers;
            viewModel.availableProjectManagers = users;
  
            if (project == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Completed,PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,ProjectManager")] Project project)
        {
            if (id != project.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }

       
    }
}
