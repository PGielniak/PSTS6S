using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;
using PSTS6.Repository;

namespace PSTS6.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepository _repo;
        private readonly PSTS6Context _context;
        public SearchController(IRepository repo, PSTS6Context context)
        {
            _repo = repo;
            _context = context;
        }
        public IActionResult GetData(string SearchText)
        {

            var projects = _repo.GetProjects(track: false, filter: false)
                .Where(p=> _repo.GetProjectSearchResults(SearchText)
                .Contains(p.ID));

            var viewModel = new SearchViewModel 
            {
                Projects=projects
            };

            return View(viewModel);
        }
    }
}
