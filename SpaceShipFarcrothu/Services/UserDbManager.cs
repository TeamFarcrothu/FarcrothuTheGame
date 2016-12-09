using SpaceShipFarcrothu.Data;
using SpaceShipFarcrothu.Models.ModelEntities;
using SpaceShipFartrothu.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Services
{
    public class UserDbManager
    {
        public string RegisterUser(ICollection<string> forms)
        {
            var context = new SpaceShipFarcrothuContext();
            string username = string.Empty;
            string password = string.Empty;
            string confirmedPassword = string.Empty;

            foreach (var form in forms)
            {
                string[] userInfo = form.Split(new char[] { ' ' }, 2);

                if (userInfo[0] == "Username:")
                {
                    username = userInfo[1];
                }
                else if (userInfo[0] == "Password:")
                {
                    password = userInfo[1];
                }
                else if (userInfo[0] == "ConfirmPassword:")
                {
                    confirmedPassword = userInfo[1];
                }
            }

            bool isUsernameFree = !context.Users.Any(u => u.Username == username);

            if (password == confirmedPassword && isUsernameFree)
            {
                var user = new User
                {
                    Username = username,
                    Password = password
                };

                context.Users.Add(user);
                context.SaveChanges();
                return username;
            }
            return null;
        }

        public string LogIn(string username, string password)
        {
            var context = new SpaceShipFarcrothuContext();

            bool hasUserInDb = context.Users.Any(u => u.Username == username && u.Password == password);

            if (hasUserInDb)
            {
                return username;
            }

            return null;
        }
    }
}
