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
        public static void EnemyShoot(IRepository<IBullet> bullets, IEnemy enemy)
        {
            if (enemy.BulletDelay >= 0)
            {
                enemy.BulletDelay--;
            }

            if (enemy.BulletDelay <= 0)
            {
                var newBulletPosition = new Vector2(enemy.Position.X + enemy.Texture.Width / 2 - TexturesManager.BulletTexture.Width / 2, enemy.Position.Y + TexturesManager.BulletTexture.Height);

                Bullet newBullet = new Bullet(newBulletPosition, enemy.ShooterId, enemy.BulletDamage);
                bullets.AddEntity(newBullet);

                if (enemy.BulletDelay == 0)
                {
                    enemy.BulletDelay = Globals.DefaultEnemyBulletDelay;
                }
            }
        }


        public static void PlayerShoot(IRepository<IBullet> bullets, IRepository<IPlayer> players, int playerId)
        {
            var currentPlayer = players.GetAll().FirstOrDefault(p => p.Id == playerId);
            if (currentPlayer != null)
            {
                if (currentPlayer.BulletDelay >= 0)
                {
                    currentPlayer.BulletDelay--;
                }

                if (currentPlayer.BulletDelay <= 0)
                {
                    SoundManager.PlayerShootSound.Play();

                    var newBulletPosition = new Vector2(currentPlayer.Position.X + 32 - TexturesManager.BulletTexture.Width / 2, currentPlayer.Position.Y + 10);

                    Bullet newBullet = new Bullet(newBulletPosition, currentPlayer.Id, currentPlayer.BulletDamage, currentPlayer.BulletSpeed);

                    if (bullets.GetAll().Where(b => b.ShooterId == currentPlayer.Id).ToList().Count < 20)
                    {
                        bullets.AddEntity(newBullet);
                    }
                }

                if (currentPlayer.BulletDelay == 0)
                {
                    currentPlayer.BulletDelay = Globals.DefaultPlayerBulletDelay;
                }

            }

        }
    }
}
