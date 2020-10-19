using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PSTS6.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PSTS6.HelperClasses;

namespace PSTS6.Areas.Security
{
    public class CanOnlyEditOwnedActivitiesHandler :
        AuthorizationHandler<ManageEditDetailsActivityRequirements>
    {
        private readonly PSTS6Context _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyEditOwnedActivitiesHandler(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageEditDetailsActivityRequirements requirement)
        {

            //PSTS6Context context = new PSTS6Context();


            ProjectSecurity projectSecurity = new ProjectSecurity(_context, _httpContextAccessor, context, "Activity");

            string loggedInOwnerId =
                   projectSecurity.LoggedInUser;

            PSTS6.Models.Activity editedRecord = (Models.Activity)projectSecurity.EditedRecord;

           

            var ownerId = _context.Users.Where(x => x.UserName == editedRecord.Owner).Select(x=>x.Id).FirstOrDefault();

            var pmId = _context.Users.Where(x => x.UserName == editedRecord.Task.Project.ProjectManager).Select(x => x.Id).FirstOrDefault();

            if ( context.User.IsInRole("Admin") 
                || (context.User.IsInRole("ProjectManager") && loggedInOwnerId==pmId) 
                || (context.User.IsInRole("Owner") && loggedInOwnerId == ownerId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
