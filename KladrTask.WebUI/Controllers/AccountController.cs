using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain.Concrete;
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
            
            ViewData["regions"] = GetRegions();
        }

        public void FillRegionsTree()
        {
            foreach (var region in kladrRepository.Regions.OrderBy(region => region.Code))
            {
                regionsTree.Insert(region.Code, region.Name);
            }
        }

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
                    return Redirect(returnUrl ?? Url.Action("Index", "Registration"));
                }
                ModelState.AddModelError("", "Неправильный логин или пароль");
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public List<SelectListItem> GetRegions()
        {
            var regions = kladrRepository.Regions
                                            .ToList()
                                            .Where(r => Tree.GetLevel(r.Code) == 1)
                                            .Select(region => new SelectListItem() { Text = region.Name, Value = region.Code })
                                            .ToList();
            var sverdlovskRegionIndex =
                regions.Select((reg, i) => Tuple.Create(i, reg.Value)).First(tuple => tuple.Item2 == SverdlovskRegionCode).Item1;

            regions.Swap(sverdlovskRegionIndex, 0);
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
            var streets = new List<SelectListItem>
            {
                new SelectListItem() {Text = "", Value = ""}
            };

            var code = id.Substring(0, 11);
            foreach (var street in kladrRepository.Roads.Where(st => st.Code.StartsWith(code)))
            {
                streets.Add(new SelectListItem() { Text = street.Name, Value = street.Code });
            }
            return Json(new SelectList(streets, "Value", "Text"));
        }

        public JsonResult GetHouses(string id)
        {
            var houses = new List<SelectListItem> { new SelectListItem() { Text = "Select", Value = "0" } };

            if (id == "") return Json(new SelectList(houses, "Value", "Text"));
            var code = id.Substring(0, 15);
            foreach (var street in kladrRepository.Houses.Where(st => st.Code.StartsWith(code)))
            {
                var homes = street.Name.Split(',').ToList();
                houses.AddRange(homes.OrderBy(i => i).Select(home => new SelectListItem() { Text = home, Value = street.Code + "," + home }));
            }

            return Json(new SelectList(houses, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Registration"));

            }
            return View();
        }
    }
}
