namespace SpaceShipFarcrothu.Data
{
    using Models.ModelEntities;
    using System.Data.Entity;

    public class SpaceShipFarcrothuContext : DbContext
    {
        public SpaceShipFarcrothuContext()
            : base("name=SpaceShipFarcrothuContext")
        {
        }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Asteroid> Asteroids { get; set; }
        public virtual DbSet<Enemy> Enemies { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}