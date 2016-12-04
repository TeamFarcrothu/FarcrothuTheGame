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
        public DbSet<PlayerModel> Players { get; set; }
    }
}