﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task <IActionResult> Index(int page = 1)
        {

            var query = _context.Project.AsNoTracking().OrderBy(s => s.Name);

           

            var viewModel = new DashboardViewModel();

           
            return View(viewModel);
        }
    }
}
