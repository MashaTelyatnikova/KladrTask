using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IKladrRepository repository;
        public HomeController(IKladrRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index(string returnUrl)
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);

            return user.Role == Role.Admin ? RedirectToAction("Index", "Admin", new { returnUrl }) : RedirectToAction("Index", "Guest", new { returnUrl });
        }
    }
}
