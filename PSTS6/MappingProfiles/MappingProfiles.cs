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

            CreateMap<PSTS6.Models.Activity, ActivityEditViewModel>().ForMember(x=>x.Owner,
                opt=>opt.Ignore());

            CreateMap<PSTS6.Models.ProjectTemplate, ProjectTemplateViewModel>();

            CreateMap<PSTS6.Models.TaskTemplate, TaskTemplateViewModel>();

            CreateMap<PSTS6.Models.ProjectTemplate, PSTS6.Models.Project>().ForMember(x=>x.ID, opt=>opt.Ignore());

            CreateMap<PSTS6.Models.TaskTemplate, PSTS6.Models.Task>().ForMember(x => x.ID, opt => opt.Ignore());

            CreateMap<PSTS6.Models.ActivityTemplate, PSTS6.Models.Activity>().ForMember(x => x.ID, opt => opt.Ignore());

        }
    }
}
