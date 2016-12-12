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
        public virtual DbSet<PlayerModel> Players { get; set; }
        public virtual DbSet<AsteroidModel> Asteroids { get; set; }
        public virtual DbSet<EnemyModel> Enemies { get; set; }
        public virtual DbSet<GameModel> Games { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }
    }
}