using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConnectionLost()
        {
            return View();
        }
    }
}