namespace SpaceShipFartrothu.Factories
{
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
                enemy.BulletDelay --;
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

        public static void BossShoot(IRepository<IBullet> bullets, Boss boss)
        {
            if (boss.BulletDelay >= 0)
            {
                boss.BulletDelay--;
            }

            if (boss.BulletDelay <= 0)
            {
                if (bullets.GetAll().Where(b => b.ShooterId == 3).ToList().Count < 20)
                {
                    var newBulletPosition = new Vector2(
                        boss.Position.X + boss.Texture.Width / 2 - TexturesManager.BulletTexture.Width / 2,
                        boss.Position.Y + boss.Texture.Height - 100);
                    Bullet newBullet = new Bullet(newBulletPosition, 3, boss.BulletDamage);

                    bullets.AddEntity(newBullet);

                    var newLeftBulletPosition = new Vector2(
                       boss.Position.X + boss.Texture.Width / 2 - TexturesManager.BulletTexture.Width / 2 - 130,
                        boss.Position.Y + boss.Texture.Height - 210);
                    Bullet newLeftBullet = new Bullet(newLeftBulletPosition, 3, boss.BulletDamage);
                    bullets.AddEntity(newLeftBullet);

                    var newRightBulletPosition = new Vector2(
                       boss.Position.X + boss.Texture.Width / 2 - TexturesManager.BulletTexture.Width / 2 + 130,
                        boss.Position.Y + boss.Texture.Height - 210);
                    Bullet newRightBullet = new Bullet(newRightBulletPosition, 3, boss.BulletDamage);
                    bullets.AddEntity(newRightBullet);
                }

                if (boss.BulletDelay == 0)
                {
                    boss.BulletDelay = Globals.DefaultBossBulletDelay;
                }
            }
        }
    }
}
