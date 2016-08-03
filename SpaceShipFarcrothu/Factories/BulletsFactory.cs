namespace SpaceShipFartrothu.Factories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using GameObjects;
    using Interfaces;
    using Utils.Assets;
    using Utils.Globals;

    public static class BulletsFactory
    {
        public static void EnemyShoot(IList<IBullet> bullets, IEnemy enemy)
        {
            if (enemy.BulletDelay >= 0)
            {
                enemy.BulletDelay--;
            }

            if (enemy.BulletDelay <= 0)
            {
                var newBulletPosition = new Vector2(enemy.Position.X + enemy.Texture.Width / 2 - TexturesManager.BulletTexture.Width / 2, enemy.Position.Y + TexturesManager.BulletTexture.Height);

                Bullet newBullet = new Bullet(newBulletPosition, enemy.ShooterId, enemy.BulletDamage);
                bullets.Add(newBullet);

                if (enemy.BulletDelay == 0)
                {
                    enemy.BulletDelay = Globals.DefaultEnemyBulletDelay;
                }
            }
        }


        public static void PlayerShoot(IList<IBullet> bullets, Player player)
        {
            if (player.BulletDelay >= 0)
            {
                player.BulletDelay--;
            }

            if (player.BulletDelay <= 0)
            {
                SoundManager.PlayerShootSound.Play();

                var newBulletPosition = new Vector2(player.Position.X + 32 - TexturesManager.BulletTexture.Width / 2, player.Position.Y + 10);

                Bullet newBullet = new Bullet(newBulletPosition, player.Id, player.BulletDamage);

                if (bullets.Where(b => b.ShooterId == player.Id).ToList().Count < 20)
                {
                    bullets.Add(newBullet);
                }
            }

            if (player.BulletDelay == 0)
            {
                player.BulletDelay = Globals.DefaultPlayerBulletDelay;
            }
        }
    }
}
