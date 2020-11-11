using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.HelperClasses;
using PSTS6.Repository;

namespace PSTS6.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
       
        public ActivitiesController(PSTS6Context context, IMapper mapper, IRepository repo)
        {
            _context = context;
            _mapper = mapper;
            
            _repo = repo;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetActivitiesAsync(filterByUser:false));
        }

        // GET: Activities/Details/5
        [Authorize(Policy = "OwnerRolePolicy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _repo.GetActivityAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create(string btnAddActivity)
        {

            var viewModel = new ActivityCreateViewModel
            {
                TaskID = Convert.ToInt32(btnAddActivity),
                StartDate = DateTime.Today,
                EstimatedEndDate = DateTime.Today
            };
            return View(viewModel);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,TaskID")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddActivityAsync(activity);
                return RedirectToAction("Edit", "Tasks", new { id = activity.TaskID });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Policy = "EditDetailsActivityPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _repo.GetActivityAndLoadRelatedDataAsync(id.GetValueOrDefault());
          
            var project = activity.Task.Project;
        
            var users = await _repo.GetUsersAsync();

            var projectUsers = await _repo.GetProjectUsers();

            var projectTeam= from user in users
                             join prjUser in projectUsers on user.Id equals prjUser.UserID
                             where prjUser.ProjectID == project.ID
                             select user;


            IEnumerable<SelectListItem> owners = projectTeam.Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.UserName
            });

            var viewModel = _mapper.Map<ActivityEditViewModel>(activity);
            viewModel.AvailableOwners=owners;

            viewModel.Owner = activity.Owner;


            if (activity == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDetailsActivityPolicy")]
        public async Task<IActionResult> Edit(int id, [Bind("PrcCompleted,Budget,StartDate,EstimatedEndDate,ActualEndDate,Spent,ID,Name,Description,TaskID,Owner")] Activity activity)
        {
            if (id != activity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateActivityAsync(activity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Tasks", new { id = activity.TaskID });
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _repo.GetActivityAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _repo.GetActivityAsync(id);
            await _repo.DeleteActivityAsync(activity);
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _repo.ActivityExists(id);
        }
    }
}
