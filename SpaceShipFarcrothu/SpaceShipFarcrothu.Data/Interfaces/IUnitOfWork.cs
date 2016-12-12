namespace SpaceShipFarcrothu.Data.Interfaces
{
    using Models.ModelEntities;

    public interface IUnitOfWork
    {
        IRepository<AsteroidModel> Asteroids { get; }

        IRepository<EnemyModel> Enemies { get; }

        IRepository<GameModel> Games { get; }

        IRepository<PlayerModel> Players { get; }

        IRepository<UserModel> User { get; }

        void Commit();
    }
}