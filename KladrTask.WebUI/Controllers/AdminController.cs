using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain;
using KladrTask.Domain.Abstract;
using KladrTask.WebUI.Models;

namespace KladrTask.WebUI.Controllers
{
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
            if (postedUsers != null && postedUsers.UserIds != null)
                Response.SaveFile("Letters.txt", String.Join("\n", GetLetters(postedUsers.UserIds, text)));
            
            return View(GetUserListModel(postedUsers));
        }

        private IEnumerable<string> GetLetters(IEnumerable<string> userIds, string text)
        {
            return userIds.Select(id => GetLetter(int.Parse(id), text));
        } 

        private string GetLetter(int userId, string text)
        {

            var user = repository.Users.First(u => u.Id == userId);

            return string.Format("Получатель: {0}\nАдрес : {1},{2},{3},{4}\nИндекс : {5} \n\n{6}\n\n", user.FirstName + ' ' + user.LastName, user.Address.Region, user.Address.Locality, user.Address.Road, user.Address.House, user.Address.Index, text);
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

            model.AvailableUsers = repository.Users.ToList().Select(user => new UserViewModel(){Id = user.Id, Login = user.Login, FirstName = user.FirstName, LastName = user.LastName});
            model.SelectedUsers = selectedUsers;

            return model;
        }

    }
}
