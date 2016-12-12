using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceShipFarcrothu.Models.ModelEntities
{
    public class UserModel
    {
        private ICollection<GameModel> games;

        public UserModel()
        {
            this.games = new HashSet<GameModel>();
        }

        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<GameModel> Games
        {
            get { return this.games; }
            set { this.games = value; }
        }
    }
}
