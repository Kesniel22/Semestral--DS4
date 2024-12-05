using System.Web.Mvc;

namespace TaskManager___Semestral.Controllers
{
    public class HomeController : Controller
    {
        // Página de inicio del proyecto
        public ActionResult Index()
        {
            ViewBag.Title = "Gestor de Tareas - Inicio";
            return View();
        }

        // Vista de documentación de la API
        public ActionResult ApiDocumentation()
        {
            ViewBag.Title = "Documentación de la API";
            return View();
        }
    }
}
