using System.Collections.Generic;
using System.Linq;

namespace KladrTask.Domain.Entities
{
    public class Mailbox
    {
        public HashSet<User> Users { get; private set; }
        public Mailbox()
        {
            Users = new HashSet<User>();
        }

        public void AddUser(User user)
        {
            if (Users.All(u => u.Id != user.Id))
                Users.Add(user);
        }

        public void RemoveUser(User user)
        {

            Users.RemoveWhere(u => u.Id == user.Id);
        }

        public void Clear()
        {
            Users.Clear();
        }
    }
}
