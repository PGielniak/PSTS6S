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

namespace PSTS6.Areas.Security
{
    public class CanOnlyEditOwnedActivitiesHandler :
        AuthorizationHandler<ManageOwnerRequirements>
    {
        private readonly PSTS6Context _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanOnlyEditOwnedActivitiesHandler(PSTS6Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageOwnerRequirements requirement)
        {

           

            string loggedInOwnerId =
                   context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string editedRecordId = _httpContextAccessor.HttpContext.GetRouteValue("id").ToString();

            var editedActivity = _context.Activity.Find(Convert.ToInt32(editedRecordId));

            var ownerId = _context.Users.Where(x => x.UserName == editedActivity.Owner).Select(x=>x.Id).FirstOrDefault();

            if ( loggedInOwnerId == ownerId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
