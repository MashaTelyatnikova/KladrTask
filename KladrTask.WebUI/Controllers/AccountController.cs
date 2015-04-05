using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KladrTask.WebUI.Infrastructure.Abstract;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private readonly IAuthProvider provider;
        public AccountController(IAuthProvider provider)
        {
            this.provider = provider;
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
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

    }
}
