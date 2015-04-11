using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using KladrTask.Domain.Entities;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    public class LettersRedactorController : Controller
    {
        //
        // GET: /LettersRedactor/

        [HttpPost]
        public ActionResult Index(Mailbox mailbox, string letter)
        {
            var letters = new List<LetterViewModel>();
            foreach (var user in mailbox.Users)
            {
                letters.Add(new LetterViewModel() { Text = letter, Receiver = String.Format("{0} {1}", user.FirstName, user.LastName), Index = user.Address.Index, Address = String.Format("{0}, {1}, {2} {3}", user.Address.Region, user.Address.Locality, user.Address.Road, user.Address.House) });
            }
            return View(new LettersListViewModel() { Letters = letters });
        }

    }
}
