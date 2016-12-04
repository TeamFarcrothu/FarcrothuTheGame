namespace SpaceShipFarcrothu.Data
{
    using Models.ModelEntities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SpaceShipFarcrothuContext : DbContext
    {
        public SpaceShipFarcrothuContext()
            : base("name=SpaceShipFarcrothuContext")
        {
        }
        public virtual DbSet<PlayerModel> Players { get; set; }
        public virtual DbSet<AsteroidModel> Asteroids { get; set; }
        public virtual DbSet<EnemyModel> Enemies { get; set; }
        public virtual DbSet<GameModel> Games { get; set; }
    }
}