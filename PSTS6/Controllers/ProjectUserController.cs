using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.Models.MainModels;
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

            var users = _context.Users.AsEnumerable();

            var existingProjectMembers = _context.ProjectUsers.Where(x=>x.ProjectID== Convert.ToInt32(btnAddTeam)).ToList();

            var selectedUsers = from user in users
                                join prjUser in existingProjectMembers on user.Id equals prjUser.UserID
                                where prjUser.ProjectID == project.ID
                                select user;

            var notSelectedUsers = from user in users                           
                                   where !(selectedUsers.Contains(user))
                                   select user;


            var viewModel = new ProjectUserCreateViewModel
            {
                Project = project,
                ProjectID = project.ID,
                SelectedUsers = selectedUsers,
                NotSelectedUsers = notSelectedUsers
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

                var projectUsers = _context.ProjectUsers.Where(x=>x.ProjectID==projectid).ToList();

                _context.RemoveRange(projectUsers);
                _context.SaveChanges();

                projectUsers.Clear();


                List<ProjectUser> newList = new List<ProjectUser>();

                

                foreach (var item in list)
                {
                    var projectUser = new ProjectUser
                    {
                        ProjectID = projectid,
                        UserID = _context.Users.Where(z => z.UserName == item).Select(x => x.Id).FirstOrDefault()
                    };

                    projectUsers.Add(projectUser);

                }

                _context.AddRange(projectUsers);

                _context.SaveChanges();


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
