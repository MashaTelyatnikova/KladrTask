using System.Linq;
using System.Web.Security;
using KladrTask.Domain.Concrete;
using KladrTask.WebUI.Infrastructure.Abstract;

namespace KladrTask.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {

        public bool Authenticate(string username, string password)
        {
            bool result = Check(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }

        private bool Check(string username, string password)
        {
            var repository = new DbKladrRepository();

            var res = repository.Users.FirstOrDefault(user => user.Login == username && user.Password == password);

            return res != null;
        }
    }
}