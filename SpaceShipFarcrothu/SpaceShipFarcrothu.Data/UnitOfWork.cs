namespace SpaceShipFarcrothu.Data
{
    using Interfaces;
    using Models.ModelEntities;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpaceShipFarcrothuContext context;
        private IRepository<AsteroidModel> asteroids;
        private IRepository<EnemyModel> enemies;
        private IRepository<GameModel> games;
        private IRepository<PlayerModel> players;
        private IRepository<UserModel> users;

        public UnitOfWork()
        {
            this.context = new SpaceShipFarcrothuContext();
        }

        public IRepository<AsteroidModel> Asteroids
            => this.asteroids ?? (this.asteroids = new Repository<AsteroidModel>(context.Asteroids));
        public IRepository<EnemyModel> Enemies
            => this.enemies ?? (this.enemies = new Repository<EnemyModel>(context.Enemies));
        public IRepository<GameModel> Games
            => this.games ?? (this.games = new Repository<GameModel>(context.Games));
        public IRepository<PlayerModel> Players
            => this.players ?? (this.players = new Repository<PlayerModel>(context.Players));
        public IRepository<UserModel> User
            => this.users ?? (this.users = new Repository<UserModel>(context.Users));

        public void Commit()
        {
            this.context.SaveChanges();
        }
    }
}