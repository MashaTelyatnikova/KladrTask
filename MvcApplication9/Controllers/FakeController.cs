using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcApplication9.Infrastructure;

namespace MvcApplication9.Controllers
{
    public class FakeController : Controller
    {
        //
        // GET: /Fake/
        private List<MyRegion> regions;
        private Tree tree;
        private MashaEntities context;
        public FakeController()
        {
            tree = new Tree();
            context = new MashaEntities();

            regions = context.MyRegions.OrderBy(i => i.Code).ToList();
            foreach (var region in regions)
            {
                tree.Insert(region.Code, region.FulName);
            }

        }
        public ActionResult Index()
        {
            return View();
        }

        public SelectList GetLines()
        {
            return new SelectList(new[] { "Masha", "Dasha" });
        }

        public ActionResult LoadCountries()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem() { Text = "Select", Value = "0" });
            foreach (var region in regions.Where(r => Tree.GetLevel(r.Code) == 1))
            {
                li.Add(new SelectListItem() { Text = region.FulName, Value = region.Code });
            }
            //li.Add(new SelectListItem { Text = "Select", Value = "0" });
            //li.Add(new SelectListItem { Text = regions.First().FulName, Value = "1" });
            //li.Add(new SelectListItem { Text = "Srilanka", Value = "2" });
            //li.Add(new SelectListItem { Text = "China", Value = "3" });
            //li.Add(new SelectListItem { Text = "Austrila", Value = "4" });
            //li.Add(new SelectListItem { Text = "USA", Value = "5" });
            //li.Add(new SelectListItem { Text = "UK", Value = "6" });
            ViewData["country"] = li;
            return View();
        }

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem() { Text = "Select", Value = "0" });
            foreach (var child in tree.GetChilds(id))
            {
                states.Add(new SelectListItem() { Text = child.FulName, Value = child.Code });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetCity(string id)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            City.Add(new SelectListItem() { Text = "Select", Value = "0" });
            City.Add(new SelectListItem() { Text = "", Value = "" });
            var code = id.Substring(0, 11);
            foreach (var street in context.StreetTables.Where(st => st.Code.StartsWith(code)))
            {
                City.Add(new SelectListItem() { Text = street.Name, Value = street.Code });
            }
            return Json(new SelectList(City, "Value", "Text"));
        }

        public JsonResult GetHomes(string id)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            City.Add(new SelectListItem() { Text = "Select", Value = "0" });


            if (id != "")
            {
                var code = id.Substring(0, 15);
                foreach (var street in context.Houses.Where(st => st.Code.StartsWith(code)))
                {
                    var homes = street.Name.Split(',').ToList();
                    foreach (var home in homes.OrderBy(i => i))
                    {
                        City.Add(new SelectListItem() { Text = home, Value = street.Code + "," + street.Index + ","+home });
                    }

                }
            }

            return Json(new SelectList(City, "Value", "Text"));
        }
    }
}
