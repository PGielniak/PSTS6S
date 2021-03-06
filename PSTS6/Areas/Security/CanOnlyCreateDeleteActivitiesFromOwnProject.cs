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
    public class CanOnlyCreateDeleteActivitiesFromOwnProject : AuthorizationHandler<ManageEditDetailsActivityRequirements>
    {
        private readonly PSTS6Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyCreateDeleteActivitiesFromOwnProject(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }



        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageEditDetailsActivityRequirements requirement)
        {
            ProjectSecurity projectSecurity = new ProjectSecurity(_context, _httpContextAccessor, context, "Activity");

            string loggedInOwnerId = projectSecurity.LoggedInUser;

            Models.Activity editedRecord = (Models.Activity)projectSecurity.EditedRecord;

            var ownerId = _context.Users.Where(x => x.UserName == editedRecord.Owner).Select(x => x.Id).FirstOrDefault();

            var pmId = _context.Users.Where(x => x.UserName == editedRecord.Task.Project.ProjectManager).Select(x => x.Id).FirstOrDefault();

            if (context.User.IsInRole("Admin")
                || (context.User.IsInRole("ProjectManager") && loggedInOwnerId == pmId)
                )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
