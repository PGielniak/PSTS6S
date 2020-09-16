using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;

namespace PSTS6.Controllers
{
    public class ActivityTemplatesController : Controller
    {
        private readonly PSTS6Context _context;

        public ActivityTemplatesController(PSTS6Context context)
        {
            _context = context;
        }

        // GET: ActivityTemplates
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityTemplate.ToListAsync());
        }

        // GET: ActivityTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityTemplate = await _context.ActivityTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityTemplate == null)
            {
                return NotFound();
            }

            return View(activityTemplate);
        }

        // GET: ActivityTemplates/Create
        public IActionResult Create(string btnAddActivityTemplate)
        {
            var viewModel = new ActivityTemplateCreateViewModel
            {
                TaskTemplateID = Convert.ToInt32(btnAddActivityTemplate),

            };
            return View(viewModel);
        }

        // POST: ActivityTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Budget,ID,Name,Description,TaskTemplateID")] ActivityTemplate activityTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "TaskTemplates", new { id = activityTemplate.TaskTemplateID });
            }
            return View(activityTemplate);
        }

        // GET: ActivityTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityTemplate = await _context.ActivityTemplate.FindAsync(id);
            if (activityTemplate == null)
            {
                return NotFound();
            }
            return View(activityTemplate);
        }

        // POST: ActivityTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Budget,ID,Name,Description")] ActivityTemplate activityTemplate)
        {
            if (id != activityTemplate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTemplateExists(activityTemplate.ID))
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
            return View(activityTemplate);
        }

        // GET: ActivityTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityTemplate = await _context.ActivityTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityTemplate == null)
            {
                return NotFound();
            }

            return View(activityTemplate);
        }

        // POST: ActivityTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityTemplate = await _context.ActivityTemplate.FindAsync(id);
            _context.ActivityTemplate.Remove(activityTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityTemplateExists(int id)
        {
            return _context.ActivityTemplate.Any(e => e.ID == id);
        }
    }
}
