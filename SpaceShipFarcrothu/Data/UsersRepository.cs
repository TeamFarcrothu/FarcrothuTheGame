using SpaceShipFartrothu.GameUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Data
{
    public class UsersRepository
    {
        private ICollection<Users> users;

        public UsersRepository()
        {
            this.users = new HashSet<Users>();
        }

        public ICollection<Users> GetAll()
        {
            return this.users;
        }

        public void AddUser(Users user)
        {
            if (users.Count < 2)
            {
                users.Add(user);
            }
        }
    }
}
