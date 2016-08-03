namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
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


        // NEed fix
       // public void ColideAndExplode(IList<IExplosion> explosions)
       // {
           // IExplosion newExplosion = ExplosionFactory.CreateExplosion(this.ExplosionTexture, this.Position);

           // explosions.Add(newExplosion);
       // }
    }
}
