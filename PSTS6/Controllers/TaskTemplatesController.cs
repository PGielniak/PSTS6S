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
    public class TaskTemplatesController : Controller
    {
        private readonly PSTS6Context _context;

        public TaskTemplatesController(PSTS6Context context)
        {
            _context = context;
        }

        // GET: TaskTemplates
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaskTemplate.ToListAsync());
        }

        // GET: TaskTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTemplate = await _context.TaskTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskTemplate == null)
            {
                return NotFound();
            }

            return View(taskTemplate);
        }

        // GET: TaskTemplates/Create
        public IActionResult Create(string btnAddTaskTemplate)
        {
            var viewModel = new TaskTemplateCreateViewModel
            {
                ProjectTemplateID = Convert.ToInt32(btnAddTaskTemplate),
             
            };
            return View(viewModel);
        }

        // POST: TaskTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,ProjectTemplateID")] TaskTemplate taskTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "ProjectTemplates", new { id = taskTemplate.ProjectTemplateID });
            }
            return View(taskTemplate);
        }

        // GET: TaskTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTemplate = await _context.TaskTemplate.FindAsync(id);
            if (taskTemplate == null)
            {
                return NotFound();
            }
            return View(taskTemplate);
        }

        // POST: TaskTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] TaskTemplate taskTemplate)
        {
            if (id != taskTemplate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTemplateExists(taskTemplate.ID))
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
            return View(taskTemplate);
        }

        // GET: TaskTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskTemplate = await _context.TaskTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskTemplate == null)
            {
                return NotFound();
            }

            return View(taskTemplate);
        }

        // POST: TaskTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskTemplate = await _context.TaskTemplate.FindAsync(id);
            _context.TaskTemplate.Remove(taskTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTemplateExists(int id)
        {
            return _context.TaskTemplate.Any(e => e.ID == id);
        }
    }
}
