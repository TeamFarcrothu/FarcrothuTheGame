using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFarcrothu.Models.ModelEntities
{
    public class Game
    {
        private ICollection<Player> players;
        private ICollection<Enemy> enemies;
        private ICollection<Asteroid> asteroids;
        private ICollection<User> users;
        public Game()
        {
            this.users = new HashSet<User>();
            this.players = new HashSet<Player>();
            this.enemies = new HashSet<Enemy>();
            this.asteroids = new HashSet<Asteroid>();
        }
        public int Id { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public virtual ICollection<Player> Players
        {
            get { return this.players; }
            set { this.players = value; }
        }
        public virtual ICollection<Enemy> Enemies
        {
            get { return this.enemies; }
            set { this.enemies = value; }
        }
        public virtual ICollection<Asteroid> Asteroids
        {
            get { return this.asteroids; }
            set { this.asteroids = value; }
        }
    }
}
