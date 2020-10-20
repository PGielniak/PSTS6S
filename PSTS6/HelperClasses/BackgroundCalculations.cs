using PSTS6.Models;
using System;
using System.Linq;
using PSTS6.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PSTS6.Configuration;
using Microsoft.Extensions.Options;

namespace PSTS6.HelperClasses
{
    public class BackgroundCalculations
    {
        private readonly ProjectSettings _settings;
        public BackgroundCalculations(IOptionsMonitor<ProjectSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        public void UpdateBudget(PSTS6Context db, Activity entity)
        {

            var task= UpdateTaskTotals(db, entity);
            UpdateProjectTotals(db, task);

        }

        public void UpdateBudget(PSTS6Context db, IEnumerable<Activity> activities)
        {

            foreach (var item in activities)
            {
                UpdateBudget(db, item);
            }
        
        }

        private void UpdateProjectTotals(PSTS6Context db, Task task)
        {
            var project = db.Project.Where(x => x.ID == task.ProjectID).Include(x => x.Tasks).FirstOrDefault();

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

        private Task UpdateTaskTotals(PSTS6Context db, Activity activity)
        {
            var task = db.Task.Where(x => x.ID == activity.TaskID).Include(x => x.Activities).FirstOrDefault();

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
