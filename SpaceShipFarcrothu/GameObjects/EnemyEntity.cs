namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Interfaces;

    public abstract class EnemyEntity : GameObject, IEnemy
    {
        protected EnemyEntity(Vector2 position)
            : base(position)
        {
        }

        public int BulletDamage { get; set; }

        public int ShooterId { get; set; }

        public int ScorePoints { get; set; }

        public int BulletDelay { get; set; }
    }
}
