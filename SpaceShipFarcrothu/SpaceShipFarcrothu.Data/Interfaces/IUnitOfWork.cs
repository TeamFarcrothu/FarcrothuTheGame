namespace SpaceShipFarcrothu.Data.Interfaces
{
    using Models.ModelEntities;

    public interface IUnitOfWork
    {
        IRepository<Asteroid> Asteroids { get; }

        IRepository<Enemy> Enemies { get; }

        IRepository<Game> Games { get; }

        IRepository<Player> Players { get; }

        IRepository<User> User { get; }

        void Commit();
    }
}