using System.Web.Mvc;
using KladrTask.Domain.Entities;

namespace KladrTask.WebUI.Binders
{
    public class MailboxModelBinder : IModelBinder
    {
        private const string SessionKey = "Mailbox";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var mailbox = (Mailbox)controllerContext.HttpContext.Session[SessionKey];
            if (mailbox != null) return mailbox;

            mailbox = new Mailbox();
            controllerContext.HttpContext.Session[SessionKey] = mailbox;

            return mailbox;
        }
    }
}