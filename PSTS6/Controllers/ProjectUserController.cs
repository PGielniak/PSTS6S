using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.Models.ViewModels;

namespace PSTS6.Controllers
{
    public class ProjectUserController : Controller

        
    {
        private readonly PSTS6Context _context;

        public ProjectUserController(PSTS6Context context)
        {
            _context = context;
        }

        // GET: ProjectTeam
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectTeam/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectTeam/Create
        public ActionResult Create(string btnAddTeam)
        {
            var project = _context.Project.Where(x => x.ID == Convert.ToInt32(btnAddTeam)).FirstOrDefault();

            var users = _context.Users.ToList();

            var viewModel = new ProjectUserCreateViewModel
            { 
                Project = project,
                ProjectID=project.ID,
                Users = users  
            };




            return View(viewModel);
        }

        // POST: ProjectTeam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string btnAdd, IEnumerable<string> list)
        {
            try
            {
                int projectid = Convert.ToInt32(btnAdd);

               // var users = Request.Form[];

                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectTeam/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectTeam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectTeam/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectTeam/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
