using System.Web.Mvc;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;

namespace KladrTask.WebUI.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IKladrRepository kladrRepository;

        public MailboxController(IKladrRepository kladrRepository)
        {
            this.kladrRepository = kladrRepository;
        }

        public RedirectToRouteResult AddToMailbox(Mailbox mailbox, string login, string returnUrl)
        {
            mailbox.AddUser(kladrRepository.GetUserByLogin(login));
            return RedirectToAction("Index", "Home", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromMailbox(Mailbox mailbox, string login, string returnUrl)
        {
            mailbox.RemoveUser(kladrRepository.GetUserByLogin(login));
            return RedirectToAction("Index", "Home", new { returnUrl });
        }
    }
}
