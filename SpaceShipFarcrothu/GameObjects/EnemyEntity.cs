namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Interfaces;
    using Effects;
    using Factories;

    public abstract class EnemyEntity : GameObject, IEnemy
    {
        protected EnemyEntity(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        public Texture2D BulletTexture { get; set; }

        // ???
        public Texture2D ExplosionTexture { get; set; }


        public int BulletDamage { get; set; }

        public int ShooterId { get; set; }

        public int ScorePoints { get; set; }

        public int BulletDelay { get; set; }


        // NEed fix
        public void ColideAndExplode(IList<IExplosion> explosions)
        {
           // IExplosion newExplosion = ExplosionFactory.CreateExplosion(this.ExplosionTexture, this.Position);

           // explosions.Add(newExplosion);
        }
    }
}
