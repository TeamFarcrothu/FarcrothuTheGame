using SpaceShipFartrothu.GameUsers;
using System.Collections.Generic;

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
