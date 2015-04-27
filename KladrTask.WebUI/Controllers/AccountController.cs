using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Concrete;
using KladrTask.Domain.Entities;
using KladrTask.WebUI.Infrastructure.Abstract;
using KladrTask.WebUI.Models;
using PrefixTree;

namespace KladrTask.WebUI.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private const string SverdlovskRegionCode = "6600000000000";
        private readonly IAuthProvider provider;
        private readonly DbKladrRepository kladrRepository;
        private readonly Tree regionsTree;

        public AccountController(IAuthProvider provider)
        {
            kladrRepository = new DbKladrRepository();

            regionsTree = new Tree();
            FillRegionsTree();

            this.provider = provider;
        }

        public void FillRegionsTree()
        {
            foreach (var region in kladrRepository.Regions.OrderBy(region => region.Code))
            {
                regionsTree.Insert(region.Code, region.Name);
            }
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
            ViewData["regions"] = GetRegions();
            return View();
        }

        public List<SelectListItem> GetRegions()
        {
            var regions = kladrRepository.Regions
                                           .ToList().Where(r => Tree.GetLevel(r.Code) == 1)
                                           .Select(region => new SelectListItem() { Text = region.Name, Value = region.Code,Selected = region.Code == SverdlovskRegionCode})
                                           .ToList();
            return regions;
        }


        public JsonResult GetLocalities(string id)
        {
            var states = new List<SelectListItem> { new SelectListItem() { Text = "Select", Value = "0" } };
            states.AddRange(regionsTree.GetChilds(id).Select(child => new SelectListItem() { Text = child.Name, Value = child.Code }));

            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetStreets(string id)
        {
            var streets = new List<SelectListItem>();
            var code = id.Substring(0, 11);
            foreach (var street in kladrRepository.Roads.Where(st => st.Code.StartsWith(code)))
            {
                streets.Add(new SelectListItem() { Text = street.Name, Value = street.Code });
            }

            if (!streets.Any())
                streets.Add(new SelectListItem() {Text = "", Value = ""});
            return Json(new SelectList(streets, "Value", "Text"));
        }

        public JsonResult GetHouses(string id)
        {
            var houses = new List<SelectListItem> { new SelectListItem() { Text = "Select", Value = "0" } };

            if (id == "") return Json(new SelectList(houses, "Value", "Text"));
            var code = id.Substring(0, 15);
            foreach (var house in kladrRepository.Houses.Where(st => st.Code.StartsWith(code)))
            {
                var homes = house.Name.Split(',').ToList();
                houses.AddRange(homes.OrderBy(i => i).Select(home => new SelectListItem() { Text = home, Value = house.Code + "," + home }));
            }

            return Json(new SelectList(houses, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!kladrRepository.ContainsUser(user.Login))
                {
                    AddUserToDb(user);
                    provider.Authenticate(user.Login, user.Password);
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                ModelState.AddModelError("", "Такой пользователь уже существует");
            }
            return View();
        }

        public void AddUserToDb(UserViewModel user)
        {
            var region = kladrRepository.GetRegionByCode(user.RegionCode);
            var locality = kladrRepository.GetRegionByCode(user.LocalityCode);
            var road = kladrRepository.GetRoadByCode(user.RoadCode);
            var house = kladrRepository.GetHouseByCode(user.HouseCode.Split(',').First());


            var address = new Address() { Region = region.Name, Locality = locality.Name, Road = road.Name, House = user.HouseCode.Split(',').Last(), RegionCode = user.RegionCode, LocalityCode = user.LocalityCode, RoadCode = user.RoadCode, HouseCode = user.HouseCode.Split(',').First(), Index = house.Index };
            kladrRepository.GetAddress(address);

            var guest = new User() { Login = user.Login, Password = user.Password, Address = address, FirstName = user.FirstName, Birthday = user.Birthday, LastName = user.LastName, Role = Role.Guest };
            kladrRepository.AddUser(guest);
        }
    }
}
