using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PSTS6.Data;
using PSTS6.Models;
using Task = PSTS6.Models.Task;

namespace PSTS6.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;
        

        public ProjectsController(PSTS6Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
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

            var templates = await _context.ProjectTemplate.ToListAsync();

            IEnumerable<SelectListItem> users = dbUsers.Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.UserName
            });

            IEnumerable<SelectListItem> projectTemplates = templates.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }) ;

            var viewModel = new ProjectCreateViewModel
            {
                availableProjectManagers = users,
                StartDate = DateTime.Today,
                EstimatedEndDate = DateTime.Today,
                Templates = projectTemplates
            };
            return View(viewModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Completed,PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,ProjectManager,Template")] Project project)
        {
            


            if (ModelState.IsValid)
            {
                var template = Request.Form["createFromTemplate"];

                var selectedTemplate = Request.Form["Template"];

                if (template!="on")
                {
                    _context.Add(project);
                }
                else
                {
                    CreateProjectFromTemplate(selectedTemplate);
                }
                    
                
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        private void CreateProjectFromTemplate(string projectTemplate)
        {
            var template = _context.ProjectTemplate.Where(x => x.ID == Convert.ToInt32(projectTemplate)).Include(x=>x.TaskTemplates).ThenInclude(y=>y.ActivityTemplates).FirstOrDefault();


            var project = _mapper.Map<Project>(template);

            foreach (var taskTemplate in template.TaskTemplates)
            {
               var task = _mapper.Map<Task>(taskTemplate);

                _context.Add(task);

                foreach (var actTemplate in taskTemplate.ActivityTemplates)
                {

                }
            }
            //TODO map template to project, tasks and activities and create the records.
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

            var viewModel = _mapper.Map<ProjectEditViewModel>(project);
       
            viewModel.Users = dbUsers;
            viewModel.AvailableProjectManagers = users;

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
