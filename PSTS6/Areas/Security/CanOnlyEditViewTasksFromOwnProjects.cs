using Microsoft.AspNetCore.Authorization;
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
    public class CanOnlyEditViewTasksFromOwnProjects : AuthorizationHandler<ManageEditDetailsProjectRequirements>
    {

        private readonly PSTS6Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyEditViewTasksFromOwnProjects(IHttpContextAccessor httpContextAccessor, PSTS6Context context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageEditDetailsProjectRequirements requirement)
        {
            ProjectSecurity projectSecurity = new ProjectSecurity(_context, _httpContextAccessor, context, "Task");

            string loggedInOwnerId = projectSecurity.LoggedInUser;

            Models.Task editedRecord = (Models.Task)projectSecurity.EditedRecord;

            string pmId = _context.Users.Where(x => x.UserName == editedRecord.Project.ProjectManager).Select(x => x.Id).FirstOrDefault();

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
