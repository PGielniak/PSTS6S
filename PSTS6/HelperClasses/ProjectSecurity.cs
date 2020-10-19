using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using PSTS6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSTS6.HelperClasses
{
    public class ProjectSecurity
    {

        private readonly PSTS6Context _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public string LoggedInUser { get; set; }

        public string EditedRecordId { get; set; }

        public object EditedRecord { get; set; }

        

        public ProjectSecurity(PSTS6Context context,
            IHttpContextAccessor httpContextAccessor,
            AuthorizationHandlerContext authContext,
            string securityObject)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

            LoggedInUser= authContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            EditedRecordId = _httpContextAccessor.HttpContext.GetRouteValue("id").ToString();            
         
            switch (securityObject)
            {

                case "Project":
                   EditedRecord = _context.Project.AsNoTracking().Where(x => x.ID == Convert.ToInt32(EditedRecordId)).FirstOrDefault();
                    break;
                case "Task":
                    EditedRecord = _context.Task.AsNoTracking().Include(x=>x.Project).Where(x => x.ID == Convert.ToInt32(EditedRecordId)).FirstOrDefault();
                    break;
                case "Activity":
                    EditedRecord = _context.Activity.AsNoTracking().Include(x => x.Task).ThenInclude(y=>y.Project).Where(x => x.ID == Convert.ToInt32(EditedRecordId)).FirstOrDefault();
                    break;
                default:
                    throw new ArgumentException("Invalid argument. The argument must be Project, Task or Activity");
                    
            }

            



        }

    }
}
