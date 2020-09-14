using PSTS6.Models;
using System;
using System.Linq;
using PSTS6.Data;
using Microsoft.EntityFrameworkCore;

namespace PSTS6.StaticClasses
{
    public static class BackgroundCalculations
    {

        public static void UpdateBudget(PSTS6Context db, Activity entity)
        {


            var task= UpdateTaskBudget(db, entity);

            UpdateProjectBudget(db, task);
            

 
        
        }

        public static void UpdateSpent(MainEntity entity)
        { }

        public static void UpdatePrcCompleted(MainEntity entity)
        { }

        private static void UpdateProjectBudget(PSTS6Context db, Task task)
        {

          //  var task = db.Task.Where(x => x.ID == activity.TaskID).FirstOrDefault();

            var project = db.Project.Where(x => x.ID == task.ProjectID).Include(x => x.Tasks).FirstOrDefault();

            var projectBudgets = project.Tasks.Select(z => z.Budget).Sum();

            project.Budget = projectBudgets;

            

           
        }

        private static Task UpdateTaskBudget(PSTS6Context db, Activity activity)
        {

            var task = db.Task.Where(x => x.ID == activity.TaskID).Include(x => x.Activities).FirstOrDefault();

            var budgets = task.Activities.Select(z => z.Budget).Sum();

            task.Budget = budgets;

            return task;
        }


    }
}
