using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PSTS6.Data;
using PSTS6.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Areas.Security
{
    public class CanOnlyCreateDeleteOwnProjects : AuthorizationHandler<ManageCreateDeleteProjectRequirements>
    {
        private readonly PSTS6Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyCreateDeleteOwnProjects(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageCreateDeleteProjectRequirements requirement)
        {
            ProjectSecurity projectSecurity = new ProjectSecurity(_context, _httpContextAccessor, context, "Project");

            string loggedInOwnerId = projectSecurity.LoggedInUser;

            PSTS6.Models.Project editedRecord = (Models.Project)projectSecurity.EditedRecord;

            var pmId = _context.Users.Where(x => x.UserName == editedRecord.ProjectManager).Select(x => x.Id).FirstOrDefault();

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
