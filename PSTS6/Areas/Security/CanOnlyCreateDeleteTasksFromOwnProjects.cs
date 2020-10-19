using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
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
    public class CanOnlyCreateDeleteTasksFromOwnProjects : AuthorizationHandler<ManageCreateDeleteTaskRequirements>
    {
        private readonly PSTS6Context _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyCreateDeleteTasksFromOwnProjects(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageCreateDeleteTaskRequirements requirement)
        {
            ProjectSecurity projectSecurity = new ProjectSecurity(_context,_httpContextAccessor,context,"Task");

            string loggedInOwnerId = projectSecurity.LoggedInUser;
          
            PSTS6.Models.Task editedRecord = (Models.Task)projectSecurity.EditedRecord;

            var pmId = _context.Users.Where(x => x.UserName == editedRecord.Project.ProjectManager).Select(x => x.Id).FirstOrDefault();

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
