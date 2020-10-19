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

namespace PSTS6.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;

        public ActivitiesController(PSTS6Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activity.ToListAsync());
        }

        // GET: Activities/Details/5
        [Authorize(Policy = "OwnerRolePolicy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.ID == id);
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
                 

                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Tasks", new { id = activity.TaskID });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Policy = "OwnerRolePolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.Include(x=>x.Task).ThenInclude(x=>x.Project).Where(q=>q.ID==id).FirstOrDefaultAsync();



            var project = activity.Task.Project;

            var users = _context.Users.AsEnumerable();
            var projectUsers = await _context.ProjectUsers.ToListAsync();

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
        [Authorize(Policy = "OwnerRolePolicy")]
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
                    _context.Update(activity);
                    BackgroundCalculations.UpdateBudget(_context, activity);
                    await _context.SaveChangesAsync();
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

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.ID == id);
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
            var activity = await _context.Activity.FindAsync(id);
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.ID == id);
        }
    }
}
