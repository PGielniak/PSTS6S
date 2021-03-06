﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSTS6.Areas.Security
{
    public class CanOnlyEditViewDeleteProjectsWhereIsPMOrAdmin : AuthorizationHandler<ManageEditDetailsProjectRequirements>
    {
        private readonly PSTS6Context _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyEditViewDeleteProjectsWhereIsPMOrAdmin(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageEditDetailsProjectRequirements requirement)
        {
            ProjectSecurity projectSecurity = new ProjectSecurity(_context, _httpContextAccessor, context, "Project");

            string loggedInOwnerId = projectSecurity.LoggedInUser;

            Models.Project editedRecord = (Models.Project)projectSecurity.EditedRecord;

            string pmId = _context.Users.Where(x => x.UserName == editedRecord.ProjectManager).Select(x => x.Id).FirstOrDefault();

            if (context.User.IsInRole("Admin")
                || (context.User.IsInRole("ProjectManager") && loggedInOwnerId==pmId)
                )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
