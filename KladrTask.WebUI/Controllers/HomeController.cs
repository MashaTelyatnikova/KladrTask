using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private readonly IKladrRepository repository;
        private UserViewModel currentUser;
        public HomeController(IKladrRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index(string returnUrl)
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);

            return user.Role == Role.Admin ? RedirectToAction("Admin", new { returnUrl }) : RedirectToAction("Guest", new { returnUrl });
        }

        public ActionResult Guest()
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);
            currentUser = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                RegionCode = user.Address.Region,
                LocalityCode = user.Address.Locality,
                RoadCode = user.Address.Road,
                HouseCode = user.Address.House,
                Login = user.Login,
                Password = user.Password
            };
            return View(currentUser);
        }

        public ActionResult Admin()
        {
            var u = repository.GetUserByLogin(HttpContext.User.Identity.Name);
            if (u.Role == Role.Guest)
                return Redirect(Url.Action("Login", "Account"));
            
            var result = repository.Users.ToList().Select(user => new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                RegionCode = user.Address.Region,
                LocalityCode = user.Address.Locality,
                RoadCode = user.Address.Road,
                HouseCode = user.Address.House,
                Login = user.Login,
                Password = user.Password
            }).ToList();
            return View(new UsersListViewModel() { Users = result });
        }
    }
}
