using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSTS6.Data;
using PSTS6.HelperClasses;
using PSTS6.Models;

namespace PSTS6.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly PSTS6Context _context;
        private readonly IMapper _mapper;
        public DashboardsController(PSTS6Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task <IActionResult> Index(string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var viewModel = new DashboardViewModel();

            

            int pageSize = 3;
            return View(viewModel);
        }
    }
}
