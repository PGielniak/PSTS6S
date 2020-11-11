using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.Repository;

namespace PSTS6.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {       
        private readonly IMapper _mapper;
        private readonly IRepository _repo;

        public TasksController(IMapper mapper, IRepository repo)
        {          
            _mapper = mapper;
            _repo = repo;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {        
            return View(await _repo.GetTasksAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _repo.GetTask(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

    
        public async Task<IActionResult> Create(string btnAddTask)
        {
            var dbUsers = await _repo.GetUsersAsync();

            IEnumerable<SelectListItem> users = dbUsers.Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.UserName
                
            });

            var projects = await _repo.GetProjectsAsync(filter:false);

            IEnumerable<SelectListItem> projectsToSelect = projects.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString(),
                        
            });

            var viewModel = new TaskCreateViewModel
            {
                availableOwners = users,
                StartDate = DateTime.Today,
                EstimatedEndDate = DateTime.Today,

                availableProjects = projectsToSelect.Where(x => x.Value == btnAddTask)
            };

            return View(viewModel);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EstimatedEndDate,ID,Name,Description,ProjectID")] PSTS6.Models.Task task)
        {
            if (ModelState.IsValid)
            {

                string selectedProject = Request.Form["Project"].ToString();

                task.ProjectID = Convert.ToInt32(selectedProject);

                await _repo.AddTask(task);
                
                return RedirectToAction("Edit","Projects", new { id= task.ProjectID});
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _repo.GetTask(id);

            var viewModel = _mapper.Map<TaskEditViewModel>(task);

            if (task == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,ProjectID")] PSTS6.Models.Task task)
        {
            if (id != task.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateTask(task);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Projects", new { id = task.ProjectID });
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _repo.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _repo.GetTask(id);

            var projectid = task.ProjectID;

            await _repo.DeleteTask(task);

            return RedirectToAction("Edit", "Projects", new { id = projectid });
        }

        private bool TaskExists(int id)
        {
            return _repo.TaskExists(id);
        }
    }
}
