using System.Web.Mvc;

namespace TaskManager___Semestral.Controllers
{
    public class HomeController : Controller
    {
        // Página de inicio del proyecto
        public ActionResult Index()
        {
            ViewBag.Title = "Pagina Inicio";
            return View();
        }
        public ActionResult CreateTask()
        {
            ViewBag.Title = "Nueva Tarea";
            return View();
        }

        public ActionResult DeleteTask()
        {
            ViewBag.Title = "Eliminar Tarea";
            return View();
        }
    }
}
