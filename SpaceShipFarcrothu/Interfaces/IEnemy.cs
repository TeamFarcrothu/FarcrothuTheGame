namespace SpaceShipFartrothu.Interfaces
{
    using Microsoft.Xna.Framework.Graphics;

    public interface IEnemy : IGameObject, IExplodable
    {
        Texture2D BulletTexture { get; set; }

        int BulletDamage { get; }

        int ShooterId { get;  }

        int ScorePoints { get; }

        int BulletDelay { get; set; }
    }
}
