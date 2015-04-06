using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KladrTask.Domain.Concrete;
using PrefixTree;

namespace KladrTask.WebUI.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly DbKladrRepository kladrRepository;
        private readonly Tree tree;
        public RegistrationController()
        {
            kladrRepository = new DbKladrRepository();
            tree = new Tree();

            foreach (var region in kladrRepository.Regions.OrderBy(region => region.Code))
            {
                tree.Insert(region.Code, region.Name);
            }
        }

        public ActionResult Index()
        {
            //var cookie = HttpContext.User.Identity;
            var regions = GetRegions();
            ViewData["regions"] = regions;
            return View();
        }

        public List<SelectListItem> GetRegions()
        {
            List<SelectListItem> regions = new List<SelectListItem>
            {
                new SelectListItem() {Text = "Select", Value = "0"}
            };
            regions.AddRange(kladrRepository.Regions.ToList().Where(r => Tree.GetLevel(r.Code) == 1).Select(region => new SelectListItem() { Text = region.Name, Value = region.Code }));

            return regions;
        }


        public JsonResult GetLocalities(string id)
        {
            var states = new List<SelectListItem> { new SelectListItem() { Text = "Select", Value = "0" } };
            states.AddRange(tree.GetChilds(id).Select(child => new SelectListItem() { Text = child.Name, Value = child.Code }));

            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetStreets(string id)
        {
            var streets = new List<SelectListItem>
            {
                new SelectListItem() {Text = "Select", Value = "0"},
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
                houses.AddRange(homes.OrderBy(i => i).Select(home => new SelectListItem() { Text = home, Value = street.Code +"," + home }));
            }

            return Json(new SelectList(houses, "Value", "Text"));
        }
    }
}
