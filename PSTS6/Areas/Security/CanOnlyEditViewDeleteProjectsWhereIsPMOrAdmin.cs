using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
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
            string loggedInOwnerId =
                   context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string editedProjectId= _httpContextAccessor.HttpContext.GetRouteValue("id").ToString();

            var editedProject = _context.Project.AsNoTracking().Where(x => x.ID == Convert.ToInt32(editedProjectId)).FirstOrDefault();   //Find(Convert.ToInt32(editedProjectId));

            var PmId = _context.Users.Where(x => x.UserName == editedProject.ProjectManager).Select(x => x.Id).FirstOrDefault();

            if (context.User.IsInRole("Admin")
                || (context.User.IsInRole("ProjectManager") && loggedInOwnerId==PmId)
                )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
