using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;
using KladrTask.WebUI.Models;
using PrefixTree;

namespace KladrTask.WebUI.Controllers
{
    public class GuestController : Controller
    {
        private readonly IKladrRepository repository;
        private readonly Tree regionsTree;
        public GuestController(IKladrRepository repository)
        {
            this.repository = repository;
            regionsTree = new Tree();
            FillRegionsTree();
        }

        public void FillRegionsTree()
        {
            foreach (var region in repository.Regions.OrderBy(region => region.Code))
            {
                regionsTree.Insert(region.Code, region.Name);
            }
        }

        public List<SelectListItem> GetRegions(string userRegionCode)
        {
            return repository.Regions
                                .ToList()
                                .Where(r => Tree.GetLevel(r.Code) == 1)
                                .Select(reg => new SelectListItem() { Value = reg.Code, Text = reg.Name, Selected = reg.Code == userRegionCode })
                                .ToList();
        }

        public List<SelectListItem> GetLocalities(string userRegionCode, string userLocalityCode)
        {
           var res = 
                regionsTree.GetChilds(userRegionCode)
                    .Select(
                        reg =>
                            new SelectListItem()
                            {
                                Value = reg.Code,
                                Text = reg.Name,
                                Selected = reg.Code == userLocalityCode
                            })
                    .ToList();

            return res;
        }

        public List<SelectListItem> GetRoads(string userLocalityCode, string userRoadCode)
        {
            var locality = userLocalityCode.Substring(0, 11);

            return
                repository.Roads.Where(road => road.Code.StartsWith(locality))
                    .Select(road => new SelectListItem() { Text = road.Name, Value = road.Code, Selected = road.Code == userRoadCode })
                    .ToList();
        }

        public List<SelectListItem> GetHouses(string userRoadCode, string userHouse)
        {
            var road = userRoadCode.Substring(0, 15);
            var houses = new List<SelectListItem>();

            foreach (var house in repository.Houses.Where(st => st.Code.StartsWith(road)))
            {
                var homes = house.Name.Split(',').ToList();
                houses.AddRange(homes.OrderBy(i => i).Select(home => new SelectListItem() { Text = home, Value = house.Code + "," + home, Selected = home == userHouse }));
            }

            return houses;
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
                HouseCode = user.Address.House,
                Login = user.Login,
                Password = user.Password
            };
            return View(currentUser);
        }

        [HttpGet]
        public ActionResult Redact()
        {
            var user = repository.GetUserByLogin(HttpContext.User.Identity.Name);

            ViewData["login"] = user.Login;
            ViewData["password"] = user.Password;
            ViewData["firstName"] = user.FirstName;
            ViewData["lastName"] = user.LastName;
            ViewData["birthday"] = user.Birthday.ToShortDateString();
            ViewData["regions"] = GetRegions(user.Address.RegionCode);
            ViewData["localities"] = GetLocalities(user.Address.RegionCode, user.Address.LocalityCode);
            ViewData["roads"] = GetRoads(user.Address.LocalityCode, user.Address.RoadCode);
            ViewData["houses"] = GetHouses(user.Address.RoadCode, user.Address.House);
            return View();
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
            var house = repository.GetHouseByCode(user.HouseCode.Split(',').First());


            var address = new Address() { Region = region.Name, Locality = locality.Name, Road = road.Name, House = user.HouseCode.Split(',').Last(), Index = house.Index, RegionCode = region.Code, LocalityCode = locality.Code, HouseCode = house.Code, RoadCode = road.Code};
            address = repository.GetAddress(address);

            currentUser.Login = user.Login;
            currentUser.Password = user.Password;
            currentUser.Address = address;
            currentUser.FirstName = user.FirstName;
            currentUser.Birthday = user.Birthday;
            currentUser.LastName = user.LastName;
            currentUser.Role = Role.Guest;
            repository.SaveChanges();
        }
    }
}
