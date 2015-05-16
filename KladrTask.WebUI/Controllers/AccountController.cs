using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Concrete;
using KladrTask.Domain.Entities;
using KladrTask.WebUI.Infrastructure.Abstract;
using KladrTask.WebUI.Models;
using PrefixTree;

namespace KladrTask.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private const string SverdlovskRegionCode = "6600000000000";
        private const string EkaterinburgCode = "6600000100000";
        private const string LunacharskCode = "66000001000063300";

        private readonly IAuthProvider provider;
        private readonly KladrRepositoryHelper kladrRepositoryHelper;
        private readonly IKladrRepository repository;
        public AccountController(IAuthProvider provider, IKladrRepository repository)
        {
            this.repository = repository;
            kladrRepositoryHelper = new KladrRepositoryHelper(repository);

            this.provider = provider;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (provider.Authenticate(model.Name, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                ModelState.AddModelError("", "Неправильный логин или пароль");
                return View();
            }
            return View();
        }

        public ActionResult Logout()
        {
            provider.LogOut();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewData["regions"] = kladrRepositoryHelper.GetRegions(SverdlovskRegionCode);
            ViewData["localities"] = kladrRepositoryHelper.GetLocalities(SverdlovskRegionCode, EkaterinburgCode);
            ViewData["streets"] = kladrRepositoryHelper.GetStreets(EkaterinburgCode, LunacharskCode);
            ViewData["indexes"] = kladrRepositoryHelper.GetIndexes(SverdlovskRegionCode, EkaterinburgCode, LunacharskCode);

            return View();
        }

        public JsonResult GetIndex(string regionCode, string localityCode, string roadCode)
        {
            return Json(kladrRepositoryHelper.GetIndexes(regionCode, localityCode, roadCode));
        }

        public List<SelectListItem> GetRegions()
        {
            var regions = repository.Regions
                                           .ToList().Where(r => Tree.GetLevel(r.Code) == 1)
                                           .Select(region => new SelectListItem() { Text = region.Name, Value = region.Code, Selected = region.Code == SverdlovskRegionCode })
                                           .ToList();
            return regions;
        }

        public JsonResult GetLocalities(string id)
        {
            return Json(kladrRepositoryHelper.GetLocalities(id));
        }

        public JsonResult GetStreets(string id)
        {
            return Json(kladrRepositoryHelper.GetStreets(id));
        }

        public List<SelectListItem> GetStreets1(string id)
        {
            var streets = new List<SelectListItem>();
            var code = id.Substring(0, 11);
            foreach (var street in repository.Roads.Where(st => st.Code.StartsWith(code)))
            {
                streets.Add(new SelectListItem() { Text = street.Name, Value = street.Code });
            }

            if (streets.Any())
                streets[0].Selected = true;

            return streets;
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!repository.ContainsUser(user.Login))
                {
                    AddUserToDb(user);
                    provider.Authenticate(user.Login, user.Password);
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                ModelState.AddModelError("", "Такой пользователь уже существует");
            }

            ViewData["regions"] = kladrRepositoryHelper.GetRegions(user.RegionCode);
            ViewData["localities"] = kladrRepositoryHelper.GetLocalities(user.RegionCode, user.LocalityCode);
            ViewData["streets"] = kladrRepositoryHelper.GetStreets(user.LocalityCode, user.RoadCode);
            ViewData["indexes"] = kladrRepositoryHelper.GetIndexes(user.RegionCode, user.LocalityCode, user.RoadCode);
            return View(user);
        }

        public void AddUserToDb(UserViewModel user)
        {
            var region = repository.GetRegionByCode(user.RegionCode);
            var locality = repository.GetRegionByCode(user.LocalityCode);
            var road = repository.GetRoadByCode(user.RoadCode);

            var address = new Address()
            {
                Region = region.Name, 
                Locality = locality.Name, 
                Road = road.Name, 
                House = user.HouseNumber, 
                RegionCode = user.RegionCode, 
                LocalityCode = user.LocalityCode, 
                RoadCode = user.RoadCode, 
                Index = user.Index,
                Apartment =  user.ApartamentNumber,
                Housing = user.Housing,
                
            };
     
            repository.GetAddress(address);

            var guest = new User()
            {
                Login = user.Login, Password = user.Password, Address = address,
                FirstName = user.FirstName, Birthday = user.Birthday,
                LastName = user.LastName, Role = Role.Guest
            };
            repository.AddUser(guest);
        }
    }
}
