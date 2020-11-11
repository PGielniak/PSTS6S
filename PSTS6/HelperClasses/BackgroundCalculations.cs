using PSTS6.Models;
using System;
using System.Linq;
using PSTS6.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PSTS6.Configuration;
using Microsoft.Extensions.Options;
using PSTS6.Repository;

namespace PSTS6.HelperClasses
{
    public class BackgroundCalculations
    {
        private readonly ProjectSettings _settings;
        private readonly IRepository _repo;

        public BackgroundCalculations(IOptionsMonitor<ProjectSettings> settings, IRepository repo)
        {
            _settings = settings.CurrentValue;
            _repo = repo;
        }

        public void UpdateBudget( Activity entity)
        {

            var task= UpdateTaskTotals(entity);
            UpdateProjectTotals(task);

        }

        public void UpdateBudget( IEnumerable<Activity> activities)
        {

            foreach (var item in activities)
            {
                UpdateBudget(item);
            }
        
        }

        private void UpdateProjectTotals(Task task)
        {
            
            var project = _repo.GetProject(task.ProjectID).Result;

            var projectBudgets = project.Tasks.Select(z => z.Budget).Sum();
            var projectSpent = project.Tasks.Select(z => z.Spent).Sum();
            var projectPrcCompleted = project.Tasks.Select(z => z.PrcCompleted).Average();

            project.Budget = projectBudgets;
            project.Spent = projectSpent;
            project.PrcCompleted = (int)projectPrcCompleted;

            if (project.PrcCompleted == 100)
            {
                if (_settings.ActualEndDateMode.Equals("1"))
                {
                    project.ActualEndDate = DateTime.Today;
                }

            }
        }

        private Task UpdateTaskTotals(Activity activity)
        {
            var task = _repo.GetTask(activity.TaskID).Result;

            var budgets = task.Activities.Select(z => z.Budget).Sum();
            var spent = task.Activities.Select(z => z.Spent).Sum();
            var prcCompleted = task.Activities.Select(z => z.PrcCompleted).Average();

            task.Budget = budgets;
            task.Spent = spent;
            task.PrcCompleted = (int)prcCompleted;

            if (task.PrcCompleted==100)
            {
                if (_settings.ActualEndDateMode.Equals("1"))
                {
                    task.ActualEndDate = DateTime.Today;
                }
               
            }

            return task;
        }


       

    }
}
