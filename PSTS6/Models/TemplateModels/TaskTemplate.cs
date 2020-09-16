﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.Models
{
    public class TaskTemplate: TemplateEntity
    {
        public int ProjectTemplateID { get; set; }
        public IEnumerable<ActivityTemplate> ActivityTemplates { get; set; }
    }
}
