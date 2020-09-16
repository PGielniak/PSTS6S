using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;

namespace PSTS6.Controllers
{
    public class ProjectTemplatesController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;

        public ProjectTemplatesController(PSTS6Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: ProjectTemplates
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectTemplate.ToListAsync());
        }

        // GET: ProjectTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTemplate = await _context.ProjectTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (projectTemplate == null)
            {
                return NotFound();
            }

            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] ProjectTemplate projectTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTemplate = await _context.ProjectTemplate.Where(x => x.ID == id).Include(x => x.TaskTemplates).FirstOrDefaultAsync();

            var viewModel = _mapper.Map<ProjectTemplateViewModel>(projectTemplate);

            if (projectTemplate == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: ProjectTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] ProjectTemplate projectTemplate)
        {
            if (id != projectTemplate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTemplateExists(projectTemplate.ID))
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
            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTemplate = await _context.ProjectTemplate
                .FirstOrDefaultAsync(m => m.ID == id);
            if (projectTemplate == null)
            {
                return NotFound();
            }

            return View(projectTemplate);
        }

        // POST: ProjectTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTemplate = await _context.ProjectTemplate.FindAsync(id);
            _context.ProjectTemplate.Remove(projectTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTemplateExists(int id)
        {
            return _context.ProjectTemplate.Any(e => e.ID == id);
        }
    }
}
