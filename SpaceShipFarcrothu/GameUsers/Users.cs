using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.GameUsers
{
    public class Users
    {
        private ICollection<string> users;

        public Users()
        {
            this.users = new HashSet<string>();
        }

        public void AddUser(string user)
        {
            if (users.Count < 2 && !users.Contains(user))
            {
                this.users.Add(user);
            }
        }
    }
}
