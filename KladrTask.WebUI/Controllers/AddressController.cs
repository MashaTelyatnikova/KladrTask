using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain.Concrete;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    public class AddressController : Controller
    {
        public ActionResult Show(string regions, string localities, string streets, string houses)
        {
            var kladrRepository = new DbKladrRepository();
            var regionName = kladrRepository.Regions.FirstOrDefault(region => region.Code == regions);
            var localityName = kladrRepository.Regions.FirstOrDefault(locality => locality.Code == localities);
            var streetName = kladrRepository.Roads.FirstOrDefault(road => road.Code == streets);

            var houseChunk = houses.Split(',').ToList();
            var houseCode = houseChunk[0];
            var house = kladrRepository.Houses.FirstOrDefault(h => h.Code == houseCode);

            return View(new AddressViewModel(){Region = regionName.Name, Locality = localityName.Name, Street = streetName.Name, House = houseChunk[1], Index = house.Index});
        }
    }
}
