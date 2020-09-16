using AutoMapper;
using Microsoft.CodeAnalysis;
using PSTS6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.MappingProfiles
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<PSTS6.Models.Project, ProjectEditViewModel>().ForMember(x=>x.ProjectManager,
                opt=>opt.Ignore());

            CreateMap<PSTS6.Models.Task, TaskEditViewModel>();

            CreateMap<PSTS6.Models.Activity, ActivityEditViewModel>();

            CreateMap<PSTS6.Models.ProjectTemplate, ProjectTemplateViewModel>();

        }
    }
}
