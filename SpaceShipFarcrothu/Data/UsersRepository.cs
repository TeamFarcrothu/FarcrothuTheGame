using SpaceShipFarcrothu.Data;
using SpaceShipFartrothu.Factories;
using SpaceShipFartrothu.GameUsers;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShipFartrothu.Data
{
    public class UsersRepository
    {
        private ICollection<User> users;
        private SpaceShipFarcrothuContext context;

        public UsersRepository()
        {
            this.users = new HashSet<User>();
            this.context = new SpaceShipFarcrothuContext();
        }

        public ICollection<string> GetAll()
        {
            return this.users.Select(u => u.Username).ToList();
        }

        public void AddUser(FormFactory forms)
        {
            if (users.Count < 2)
            {
                string username = string.Empty;
                string password = string.Empty;

                foreach (var form in forms.GetAllForms())
                {
                    string[] formInfo = form.Split(new char[] { ' ' }, 2);

                    if (formInfo[0] == "Username:")
                    {
                        username = formInfo[1];
                    }
                    else if (formInfo[0] == "Password:")
                    {
                        password = formInfo[1];
                    }
                }

                bool hasUserInDb = context.Users.Any(u => u.Username == username && u.Password == password);
                if (hasUserInDb)
                {
                    users.Add(new User { Username = username });
                }

            }
        }
    }
}
