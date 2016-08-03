namespace SpaceShipFartrothu.Data
{
    using Interfaces;

    public class GameDatabase
    {
        public GameDatabase()
        {
            this.Asteroids = new Repository<IAsteroid>();
            this.Bullets = new Repository<IBullet>();
            this.Enemies = new Repository<IEnemy>();
            this.Players = new Repository<IPlayer>();
            this.Explosions = new Repository<IExplosion>();
            this.Items = new Repository<IItem>();
        }

        public IRepository<IAsteroid> Asteroids { get; private set; }

        public IRepository<IBullet> Bullets { get; private set; }

        public IRepository<IEnemy> Enemies { get; private set; }

        public IRepository<IPlayer> Players { get; private set; }

        public IRepository<IExplosion> Explosions { get; private set; }

        public IRepository<IItem> Items { get; private set; }
    }
}
