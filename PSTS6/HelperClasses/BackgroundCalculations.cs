using PSTS6.Models;
using System;
using System.Linq;
using PSTS6.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PSTS6.HelperClasses
{
    public static class BackgroundCalculations
    {

        public static void UpdateBudget(PSTS6Context db, Activity entity)
        {

            var task= UpdateTaskTotals(db, entity);
            UpdateProjectTotals(db, task);

        }

        public static void UpdateBudget(PSTS6Context db, IEnumerable<Activity> activities)
        {



            foreach (var item in activities)
            {
                UpdateBudget(db, item);
            }
        
        }

        private static void UpdateProjectTotals(PSTS6Context db, Task task)
        {
            var project = db.Project.Where(x => x.ID == task.ProjectID).Include(x => x.Tasks).FirstOrDefault();

            var projectBudgets = project.Tasks.Select(z => z.Budget).Sum();
            var projectSpent = project.Tasks.Select(z => z.Spent).Sum();
            var projectPrcCompleted = project.Tasks.Select(z => z.PrcCompleted).Average();

            project.Budget = projectBudgets;
            project.Spent = projectSpent;
            project.PrcCompleted = (int)projectPrcCompleted;
        }

        private static Task UpdateTaskTotals(PSTS6Context db, Activity activity)
        {
            var task = db.Task.Where(x => x.ID == activity.TaskID).Include(x => x.Activities).FirstOrDefault();

            var budgets = task.Activities.Select(z => z.Budget).Sum();
            var spent = task.Activities.Select(z => z.Spent).Sum();
            var prcCompleted = task.Activities.Select(z => z.PrcCompleted).Average();

            task.Budget = budgets;
            task.Spent = spent;
            task.PrcCompleted = (int)prcCompleted;

            return task;
        }
       

    }
}
