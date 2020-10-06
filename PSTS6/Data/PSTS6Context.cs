using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PSTS6.Models;
using PSTS6.Models.IdentityModels;

namespace PSTS6.Data
{
    public class PSTS6Context : IdentityDbContext<IdentityUser>
    {
        public PSTS6Context(DbContextOptions<PSTS6Context> options)
            : base(options)
        {
        }

        public DbSet<PSTS6.Models.Project> Project { get; set; }

        public DbSet<PSTS6.Models.Task> Task { get; set; }

        public DbSet<PSTS6.Models.Activity> Activity { get; set; }

        public DbSet<PSTS6.Models.ProjectTemplate> ProjectTemplate { get; set; }

        public DbSet<PSTS6.Models.TaskTemplate> TaskTemplate { get; set; }

        public DbSet<PSTS6.Models.ActivityTemplate> ActivityTemplate { get; set; }

       public override DbSet<IdentityUser> Users { get; set; }

        
    }
}
