﻿namespace SpaceShipFartrothu.Factories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GameObjects;
    using Interfaces;
    using Multimedia;
    using  Globals;

    public static class BulletsFactory
    {
        public static void EnemyShoot(IList<IBullet> bullets, IEnemy enemy, Texture2D bullletTexture)
        {
            if (enemy.BulletDelay >= 0)
            {
                enemy.BulletDelay--;
            }

            if (enemy.BulletDelay <= 0)
            {
                var newBulletPosition = new Vector2(enemy.Position.X + enemy.Texture.Width / 2 - enemy.BulletTexture.Width / 2, enemy.Position.Y + enemy.BulletTexture.Height);

                Bullet newBullet = new Bullet(bullletTexture, newBulletPosition, enemy.ShooterId, enemy.BulletDamage);
                bullets.Add(newBullet);

                if (enemy.BulletDelay == 0)
                {
                    enemy.BulletDelay = Globals.DefaultEnemyBulletDelay;
                }
            }
        }


        public static void PlayerShoot(IList<IBullet> bullets, Player player, Texture2D bullletTexture, SoundManager soundManager)
        {
            if (player.BulletDelay >= 0)
            {
                player.BulletDelay--;
            }

            if (player.BulletDelay <= 0)
            {
                soundManager.playerShootSound.Play();

                var newBulletPosition = new Vector2(player.Position.X + 32 - player.BulletTexture.Width / 2, player.Position.Y + 10);

                Bullet newBullet = new Bullet(player.BulletTexture, newBulletPosition, player.Id, player.BulletDamage);

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