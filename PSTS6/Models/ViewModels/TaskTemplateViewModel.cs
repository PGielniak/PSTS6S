﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class TaskTemplateViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Budget { get; set; }

        public IEnumerable<ActivityTemplate> ActivityTemplates { get; set; }
    }
}
