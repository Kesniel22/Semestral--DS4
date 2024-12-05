using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TaskManager___Semestral.Models
{
    public class TaskView
    {
        public List<Task> PendingTasks { get; set; }
        public List<Task> InProcessTasks { get; set; }
        public List<Task> CompletedTasks { get; set; }
    }
}