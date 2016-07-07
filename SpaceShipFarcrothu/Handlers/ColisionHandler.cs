namespace SpaceShipFartrothu.Handlers
{
    using GameObjects;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public static class ColisionHandler
    {
        //check players for gameobjects collisions
        public static void CheckForCollision(List<Player> intersectors, List<GameObject> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                GameObject currentTarget = targets[i];

                foreach (var player in Player.Players)
                {
                    if (player.BoundingBox.Intersects(currentTarget.BoundingBox))
                    {
                        player.Score += (currentTarget as EnemyEntity).ScorePoints;

                        player.ReactOnColission(currentTarget);
                        currentTarget.ReactOnColission();
                    }
                }
            }
        }

        //check if player bullets collides with gameobjects
        public static void CheckPlayerBulletsCollisions(Player player, List<GameObject> targets)
        {
            var playerBullets = Bullet.Bullets.Where(b => b.ShooterId == player.Id).ToList();

            for (int i = 0; i < targets.Count; i++)
            {
                GameObject currentTarget = targets[i];

                foreach (var playerBullet in playerBullets)
                {
                    if (currentTarget.BoundingBox.Intersects(playerBullet.BoundingBox))
                    {
                        //player.ReactOnColission(currentTarget);
                        //TODO: increase player points
                        player.Score += (currentTarget as EnemyEntity).ScorePoints;

                        playerBullet.ReactOnColission();
                        currentTarget.ReactOnColission();
                    }
                }
            }
        }

        //check enemies bullets for collision with palyer
        public static void CheckEnemiesBulletsCollisions(List<GameObject> enemies)
        {
            var enemiesBullets = Bullet.Bullets.Where(b => b.ShooterId == 0).ToList();

            foreach (Player player in Player.Players)
            {
                for (int i = 0; i < enemiesBullets.Count; i++)
                {
                    var currentBullet = enemiesBullets[i];

                    if (player.BoundingBox.Intersects(currentBullet.BoundingBox))
                    {
                        player.ReactOnColission(currentBullet);
                        currentBullet.ReactOnColission();
                    }
                }
            }
        }

        //check boss bullets for collision with players
        public static void CheckBossBulletsCollisions()
        {
            var bossBullets = Bullet.Bullets.Where(b => b.ShooterId == 3).ToList();

            foreach (Player player in Player.Players)
            {
                for (int i = 0; i < bossBullets.Count; i++)
                {
                    var currentBullet = bossBullets[i];

                    if (player.BoundingBox.Intersects(currentBullet.BoundingBox))
                    {
                        player.ReactOnColission(currentBullet);
                        currentBullet.ReactOnColission();
                    }
                }
            }
        }
    }
}


