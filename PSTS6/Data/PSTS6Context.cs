using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSTS6.Models;

namespace PSTS6.Data
{
    public class PSTS6Context : DbContext
    {
        public PSTS6Context (DbContextOptions<PSTS6Context> options)
            : base(options)
        {
        }

        public DbSet<PSTS6.Models.Project> Project { get; set; }
    }
}
