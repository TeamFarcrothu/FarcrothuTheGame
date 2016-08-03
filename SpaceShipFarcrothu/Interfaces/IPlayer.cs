namespace SpaceShipFartrothu.Interfaces
{
    using Handlers;

    public interface IPlayer : IGameObject
    {
        int Id { get; }

        bool IsAlive { get; set; }

        int BulletDamage { get; set; }

        int Score { get; set; }

        int Health { get; set; }

        int BulletDelay { get; set; }

        int BulletSpeed { get; set; }

        int MaxHealth { get; set; }

        int Level { get; set; }

        InputHandler InputHandler { get; set; }
    }
}
