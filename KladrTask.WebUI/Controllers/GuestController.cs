using System.Web.Mvc;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    public class GuestController : Controller
    {
        private readonly IKladrRepository repository;
        private readonly KladrRepositoryHelper kladrRepositoryHelper;
        public GuestController(IKladrRepository repository)
        {
            this.repository = repository;
            kladrRepositoryHelper = new KladrRepositoryHelper(repository);
        }

        public ActionResult Index()
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);
            var currentUser = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                RegionCode = user.Address.Region,
                LocalityCode = user.Address.Locality,
                RoadCode = user.Address.Road,
                Login = user.Login,
                Password = user.Password,
                Index = user.Address.Index
            };
            return View(currentUser);
        }

        [HttpGet]
        public ActionResult Redact()
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);

            ViewData["regions"] = kladrRepositoryHelper.GetRegions(user.Address.RegionCode);
            ViewData["localities"] = kladrRepositoryHelper.GetLocalities(user.Address.RegionCode, user.Address.LocalityCode);
            ViewData["roads"] = kladrRepositoryHelper.GetStreets(user.Address.LocalityCode, user.Address.RoadCode);
            ViewData["indexes"] = kladrRepositoryHelper.GetIndexes(user.Address.RegionCode, user.Address.LocalityCode,
                user.Address.RoadCode);

            return View(new UserViewModel()
            {
                Login = user.Login,
                Password = user.Password,
                Housing = user.Address.Housing, 
                Index = user.Address.Index,
                ApartamentNumber = user.Address.Apartment,
                HouseNumber = user.Address.House, 
                Birthday = user.Birthday,
                FirstName = user.FirstName,
                Id = user.Id, 
                LastName = user.LastName,
                LocalityCode = user.Address.LocalityCode,
                RegionCode = user.Address.RegionCode, 
                RoadCode = user.Address.RoadCode
            });
        }

        [HttpPost]
        public ActionResult Redact(UserViewModel user)
        {
            UpdateUserInDb(user);
            return RedirectToAction("Index");
        }

        public void UpdateUserInDb(UserViewModel user)
        {
            var currentUser = repository.GetUserByLogin(HttpContext.User.Identity.Name);
            var region = repository.GetRegionByCode(user.RegionCode);
            var locality = repository.GetRegionByCode(user.LocalityCode);
            var road = repository.GetRoadByCode(user.RoadCode);

            var address = new Address()
            {
                Region = region.Name,
                Locality = locality.Name,
                Road = road.Name,
                Index = user.Index,
                RegionCode = region.Code, 
                LocalityCode = locality.Code, 
                RoadCode = road.Code,
                Apartment = user.ApartamentNumber,
                Housing =  user.Housing,
                House = user.HouseNumber
            };
            address = repository.GetAddress(address);

            currentUser.Login = user.Login;
            currentUser.Password = user.Password;
            currentUser.Address = address;
            currentUser.FirstName = user.FirstName;
            currentUser.Birthday = user.Birthday;
            currentUser.LastName = user.LastName;
            repository.SaveChanges();
        }
    }
}
