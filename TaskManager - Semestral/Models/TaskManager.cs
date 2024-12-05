using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager___Semestral.Models
{
    public class TaskManager
    {
        public int TaskID { get; set; }
        [Required][StringLength(100)] public string Title { get; set; }
        [Required][StringLength(500)] public string Description { get; set; }
        [Required] public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required][RegularExpression("Pendiente|En Proceso|Completado", ErrorMessage = "Estado Inválido")] public string Status { get; set; }
    }
}