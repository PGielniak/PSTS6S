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
using PSTS6.Models.IdentityModels;
using PSTS6.HelperClasses;
using Task = PSTS6.Models.Task;
using PSTS6.Configuration;
using Microsoft.Extensions.Options;

namespace PSTS6.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;
        private readonly ProjectSettings _settings;
        private readonly BackgroundCalculations _backgroundCalculations;


        public ProjectsController(PSTS6Context context, IMapper mapper, IOptionsMonitor<ProjectSettings> settings, BackgroundCalculations backgroundCalculations)
        {
            _context = context;
            _mapper = mapper;
            _settings = settings.CurrentValue;
            _backgroundCalculations = backgroundCalculations;
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
                StartDate = DateTime.Today.AddDays(Convert.ToInt32(_settings.DefaultDateMode)),
                EstimatedEndDate = DateTime.Today.AddDays(Convert.ToInt32(_settings.DefaultDateMode + _settings.EndDateMode)),
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
                    if (_settings.CreationMode.Equals("OnlyTemplate"))
                    {
                        //put error code here
                        return Content("Error");
                    }
                    else
                    {
                        _context.Add(project);
                        await _context.SaveChangesAsync();
                    }
                    


                }
                else
                {
                    if (_settings.CreationMode.Equals("OnlyManual"))
                    {
                        //put error code here
                    }
                    else
                    {
                        CreateProjectFromTemplate(selectedTemplate);
                    }

                   

                }
                    
                
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        private void CreateProjectFromTemplate(string projectTemplate)
        {
            var template = _context.ProjectTemplate.Where(x => x.ID == Convert.ToInt32(projectTemplate)).Include(x=>x.TaskTemplates).ThenInclude(y=>y.ActivityTemplates).FirstOrDefault();


            var project = _mapper.Map<Project>(template);
            _context.Add(project);

            _context.SaveChanges();

            foreach (var taskTemplate in template.TaskTemplates)
            {
               var task = _mapper.Map<Task>(taskTemplate);
                task.ProjectID = project.ID;
                _context.Add(task);
                _context.SaveChanges();

                foreach (var actTemplate in taskTemplate.ActivityTemplates)
                {
                    var activity = _mapper.Map<Activity>(actTemplate);
                    activity.TaskID = task.ID;
                    _context.Add(activity);
                    
                    _context.SaveChanges();
                }

                _backgroundCalculations.UpdateBudget(_context, task.Activities);
            }

           
        }

        // GET: Projects/Edit/5

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           

            var project = await _context.Project.Where(x=>x.ID==id).Include(x=>x.Tasks).FirstOrDefaultAsync();

            var projectManager = project.ProjectManager;
            
            var dbUsers = await _context.Users.ToListAsync();

            var projectUsers = await _context.ProjectUsers.ToListAsync();

            //var projectTeam = dbUsers.Join(projectUsers,
            //                                user => user.Id,
            //                                prjUser => prjUser.UserID,
            //                                (user,prjUser)=>new { user.UserName, user.Email, prjUser.ProjectID})
            //                            .Where(x=>x.ProjectID==project.ID).ToList();


            var projectTeam= from user in dbUsers
                            join prjUser in projectUsers on user.Id equals prjUser.UserID
                            where prjUser.ProjectID == project.ID
                            select user ;



            IEnumerable<SelectListItem> users = dbUsers.Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.UserName,
               
                
            });


            foreach (var item in users)
            {
                if (item.Text.Equals(projectManager))
                {
                    item.Selected = true;
                }
            }
            

            

            var viewModel = _mapper.Map<ProjectEditViewModel>(project);
       
            viewModel.Users = dbUsers;
            viewModel.AvailableProjectManagers = users;
            viewModel.ProjectTeam = projectTeam;
            viewModel.ProjectManager = projectManager;
            viewModel.ActualEndDateSetting = _settings.ActualEndDateMode;

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
        [Authorize(Policy = "PMRolePolicy")]
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
                    _context.Attach(project);
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
        [Authorize(Policy = "PMRolePolicy")]
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
        [Authorize(Policy = "PMRolePolicy")]
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
