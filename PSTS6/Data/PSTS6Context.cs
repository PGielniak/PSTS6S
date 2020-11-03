using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PSTS6.Models;
using PSTS6.Models.IdentityModels;
using PSTS6.Models.MainModels;

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

        public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<ProjectSearch> ProjectSearch { get; set; }
        public DbSet<TaskSearch> TaskSearch { get; set; }
        public DbSet<ActivitySearch> ActivitySearch { get; set; }
        public DbSet<ProjectTemplateSearch> ProjectTemplateSearch { get; set; }
        public DbSet<UserSearch> UserSearch { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectID, pu.UserID });

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(pu => pu.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectID);

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(pu => pu.ProjectUsers)
                .HasForeignKey(pu => pu.UserID);

            builder.Entity<IdentityUserLogin<string>>()
                .HasNoKey();

           

            builder.Entity<IdentityUserClaim<string>>()
                        .HasNoKey();

            builder.Entity<IdentityUserToken<string>>()
                .HasNoKey();

            builder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.RoleId, ur.UserId });

            



        }


    }
}
