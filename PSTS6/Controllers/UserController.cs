using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Controllers
{
    public class UserController : Controller
    {
        private readonly PSTS6Context _context;

        public UserController(PSTS6Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            var userRole = await _context.UserRoles.Where(x => x.UserId == id).FirstOrDefaultAsync();

            var roles = await _context.Roles.ToListAsync();

            var role = roles.Where(x => x.Id == userRole.RoleId).FirstOrDefault();

            var rolesSelectList = roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name,


            });

            foreach (var item in rolesSelectList)
            {
                if (item.Text.Equals(role))
                {
                    item.Selected = true;
                }
            }

            var viewModel = new UserEditViewModel 
            { 
                UserName=user.UserName,
                Email=user.Email,
               
                AvailableRoles=rolesSelectList
            };

            

            return View(viewModel);
        }

    }
}
