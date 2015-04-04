using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication9.Controllers
{
    public class AddressController :Controller
    {
        [HttpPost]
        public ActionResult Index(string country, string state, string city, string home)
        {
            var context = new MashaEntities();
            var regionName = context.MyRegions.FirstOrDefault(reg => reg.Code == country);
            var locality = context.MyRegions.FirstOrDefault(reg => reg.Code == state);
            var street = context.StreetTables.FirstOrDefault(reg => reg.Code == city);
            var chunks = home.Split(',').ToList();
            
            return View(new Address(){Region = regionName == null ? "" : regionName.FulName, Locality = locality == null ? "" : locality.FulName, Street = street == null ? "" : street.Name, House = chunks[2], Index = chunks[1]});
        }
    }
}