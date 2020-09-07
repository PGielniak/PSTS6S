using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class Project : Entity
    {
        public IEnumerable<Task> Tasks { get; set; }
        public User ProjectManager { get; set; }
        public IEnumerable<User> ProjectTeam { get; set; }
        public bool Completed { get; set; }

    }
}
