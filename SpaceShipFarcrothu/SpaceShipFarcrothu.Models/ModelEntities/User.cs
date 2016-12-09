using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceShipFarcrothu.Models.ModelEntities
{
    public class User
    {
        private ICollection<Game> games;

        public User()
        {
            this.games = new HashSet<Game>();
        }

        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Game> Games
        {
            get { return this.games; }
            set { this.games = value; }
        }
    }
}
