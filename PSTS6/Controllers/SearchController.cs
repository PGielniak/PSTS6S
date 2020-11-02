using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSTS6.Models;

namespace PSTS6.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult GetData()
        {
            var viewModel = new SearchViewModel();

            return View(viewModel);
        }
    }
}
