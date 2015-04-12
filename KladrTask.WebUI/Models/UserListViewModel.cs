using System.Collections.Generic;

namespace KladrTask.WebUI.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> AvailableUsers { get; set; }
        public IEnumerable<UserViewModel> SelectedUsers { get; set; }
        public PostedUsers PostedUsers { get; set; }
    }
}