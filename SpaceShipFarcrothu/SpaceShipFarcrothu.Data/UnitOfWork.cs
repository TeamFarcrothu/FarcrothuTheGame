namespace SpaceShipFarcrothu.Data
{
    using Interfaces;
    using Models.ModelEntities;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpaceShipFarcrothuContext context;
        private IRepository<Asteroid> asteroids;
        private IRepository<Enemy> enemies;
        private IRepository<Game> games;
        private IRepository<Player> players;
        private IRepository<User> users;

        public UnitOfWork()
        {
            this.context = new SpaceShipFarcrothuContext();
        }

        public IRepository<Asteroid> Asteroids
            => this.asteroids ?? (this.asteroids = new Repository<Asteroid>(context.Asteroids));
        public IRepository<Enemy> Enemies
            => this.enemies ?? (this.enemies = new Repository<Enemy>(context.Enemies));
        public IRepository<Game> Games
            => this.games ?? (this.games = new Repository<Game>(context.Games));
        public IRepository<Player> Players
            => this.players ?? (this.players = new Repository<Player>(context.Players));
        public IRepository<User> User
            => this.users ?? (this.users = new Repository<User>(context.Users));

        public void Commit()
        {
            this.context.SaveChanges();
        }
    }
}