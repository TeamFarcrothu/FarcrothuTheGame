namespace SpaceShipFartrothu.Interfaces
{
    public interface IEnemy : IGameObject//, IExplodable
    {
        int BulletDamage { get; }

        int ShooterId { get;  }

        int ScorePoints { get; }

        int BulletDelay { get; set; }
    }
}
