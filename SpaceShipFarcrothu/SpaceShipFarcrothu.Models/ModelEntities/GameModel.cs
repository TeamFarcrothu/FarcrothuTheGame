using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFarcrothu.Models.ModelEntities
{
    public class GameModel
    {
        private ICollection<PlayerModel> players;
        private ICollection<EnemyModel> enemies;
        private ICollection<AsteroidModel> asteroids;
        private ICollection<UserModel> users;
        public GameModel()
        {
            this.users = new HashSet<UserModel>();
            this.players = new HashSet<PlayerModel>();
            this.enemies = new HashSet<EnemyModel>();
            this.asteroids = new HashSet<AsteroidModel>();
        }
        public int Id { get; set; }

        public virtual ICollection<UserModel> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public virtual ICollection<PlayerModel> Players
        {
            get { return this.players; }
            set { this.players = value; }
        }
        public virtual ICollection<EnemyModel> Enemies
        {
            get { return this.enemies; }
            set { this.enemies = value; }
        }
        public virtual ICollection<AsteroidModel> Asteroids
        {
            get { return this.asteroids; }
            set { this.asteroids = value; }
        }
    }
}
