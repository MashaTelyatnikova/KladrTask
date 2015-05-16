using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IKladrRepository repository;
        public AdminController(IKladrRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var u = repository.GetUserByLogin(HttpContext.User.Identity.Name);
            if (u.Role == Role.Guest)
                return Redirect(Url.Action("Login", "Account"));

            return View(GetUsersInitialModel());
        }

        [HttpPost]
        public ActionResult Index(PostedUsers postedUsers, string text)
        {
            if (postedUsers == null || postedUsers.UserIds == null)
                return View(GetUserListModel(postedUsers));

            var letters = GetLetters(postedUsers.UserIds, text).ToArray();
            var document = new Document();
            var arialFontPath = string.Format(@"{0}Content\arial.ttf", AppDomain.CurrentDomain.BaseDirectory);
            var baseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, 12);

            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream).CloseStream = false;
            
            document.Open();
            foreach (var letter in letters)
            {
                document.NewPage();
                document.Add(new Paragraph(letter, font));
            }
            document.Close();

            var byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/pdf");
        }

        private IEnumerable<string> GetLetters(IEnumerable<string> userIds, string text)
        {
            return userIds.Select(id => GetLetter(int.Parse(id), text));
        }

        private string GetLetter(int userId, string text)
        {
            var user = repository.Users.First(u => u.Id == userId);

            return string.Format("\n\n\n\n\n\n\n\n\n\n\n\nПолучатель: {0}\nАдрес : {1},{2},{3},{4},{5},{6}\nИндекс : {7} \n\n\n\n\n\n{8}", user.FirstName + ' ' + user.LastName, user.Address.Region, user.Address.Locality, user.Address.Road, user.Address.House, user.Address.Housing, user.Address.Apartment, user.Address.Index, text);
        }

        private UserListViewModel GetUserListModel(PostedUsers postedUsers)
        {
            var model = new UserListViewModel();
            var selectedUsers = new List<UserViewModel>();
            var postedUsersIds = new string[0];
            if (postedUsers == null) postedUsers = new PostedUsers();


            if (postedUsers.UserIds != null && postedUsers.UserIds.Any())
            {
                postedUsersIds = postedUsers.UserIds;
            }

            var users = repository.Users.ToList().Select(user => new UserViewModel() { Id = user.Id, Login = user.Login, FirstName = user.FirstName, LastName = user.LastName }).ToList();
            if (postedUsersIds.Any())
            {
                selectedUsers = users.Where(x => postedUsersIds.Any(s => x.Id.ToString().Equals(s)))
                 .ToList();
            }

            model.AvailableUsers = users;
            model.SelectedUsers = selectedUsers;
            model.PostedUsers = postedUsers;

            return model;
        }

        private UserListViewModel GetUsersInitialModel()
        {
            var model = new UserListViewModel();
            var selectedUsers = new List<UserViewModel>();

            model.AvailableUsers = repository.Users.ToList().Select(user => new UserViewModel() { Id = user.Id, Login = user.Login, FirstName = user.FirstName, LastName = user.LastName });
            model.SelectedUsers = selectedUsers;

            return model;
        }

    }
}
