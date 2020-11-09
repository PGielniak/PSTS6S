using JsonHelper;
using PSTS6.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSTS6.HelperClasses
{
    public class ListMapper : IGoogleVisualizable
    {
        private readonly IRepository _repo;

        public ListMapper(IRepository repo)
        {
            _repo = repo;
        }

        public int ID { get;set; }
        public string Resource { get; set; }
        public string TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int PrcComplete { get; set; }
        public string Dependencies { get; set; }

        public List<IGoogleVisualizable> ConvertLists(int id)
        {
            var project = _repo.GetProject(id).Result;

            var tasks = _repo.GetTasks().Where(x => x.ProjectID == id).ToList();

            

            var activities = _repo.GetActivities(false,false).Where(x => x.Task.ProjectID==id).ToList();

            List<IGoogleVisualizable> mappedList = new List<IGoogleVisualizable>();

            mappedList.Add(new ListMapper(_repo)
            {
                ID = project.ID,
                Resource = "projectResource",
                TaskName = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EstimatedEndDate,
                PrcComplete = project.PrcCompleted,
                Dependencies = String.Empty
            });
            

            foreach (var item in tasks)
            {
                 mappedList.Add(new ListMapper(_repo)
            {
               ID=item.ID,
               Resource="taskResource",
               TaskName=item.Name,
               StartDate=item.StartDate,
               EndDate=item.EstimatedEndDate,
               PrcComplete=item.PrcCompleted,
               Dependencies= $"{item.Project.Name}{item.ProjectID}"
            });
            }

            foreach (var item in activities)
            {
                mappedList.Add(new ListMapper(_repo)
                {
                    ID = item.ID,
                    Resource = "activityResource",
                    TaskName = item.Name,
                    StartDate = item.StartDate,
                    EndDate = item.EstimatedEndDate,
                    PrcComplete = item.PrcCompleted,
                    Dependencies = $"{item.Task.Name}{item.TaskID}"
                });
            }

            return mappedList;
        }

        public List<IGoogleVisualizable> ConvertLists()
        {
            throw new NotImplementedException();
        }
    }
}
