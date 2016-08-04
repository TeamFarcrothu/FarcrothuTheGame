namespace SpaceShipFartrothu.Handlers
{
    using GameObjects;
    using GameObjects.Items;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public static class CollisionHandler
    {
        //check players for gameobjects collisions
        public static void CheckForCollision(IList<IGameObject> targets, IList<IPlayer> players, IList<IExplosion> explosions)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                IGameObject currentTarget = targets[i];

                foreach (var player in players)
                {
                    if (player.BoundingBox.Intersects(currentTarget.BoundingBox))
                    {

                        player.Score += (currentTarget as EnemyEntity).ScorePoints;

                        player.ReactOnColission(currentTarget);
                        currentTarget.ReactOnColission(player);
                        //(currentTarget as EnemyEntity).ColideAndExplode(explosions);
                        if (currentTarget is Item)
                        {
                            if (currentTarget.IsVisible)
                            {
                                currentTarget.ReactOnColission(player);
                            }
                        }
                    }
                }
            }
        }

        //check if player bullets collides with gameobjects
        public static void CheckPlayerBulletsCollisions(IList<IGameObject> targets, IList<IBullet> bullets, IList<IPlayer> players, IList<IExplosion> explosions)
        {
            foreach (var player in players)
            {
                var playerBullets = bullets.Where(b => b.ShooterId == player.Id).ToList();

                for (int i = 0; i < targets.Count; i++)
                {
                    IGameObject currentTarget = targets[i];

                    foreach (var playerBullet in playerBullets)
                    {
                        if (currentTarget.BoundingBox.Intersects(playerBullet.BoundingBox))
                        {
                            //player.ReactOnColission(currentTarget);
                            //TODO: increase player points
                            player.Score += (currentTarget as EnemyEntity).ScorePoints;

                            playerBullet.ReactOnColission();
                            currentTarget.ReactOnColission(player);
                            //(currentTarget as EnemyEntity).ColideAndExplode(explosions);

                        }
                    }
                }
            }

        }

        //check enemies bullets for collision with palyer
        public static void CheckEnemiesBulletsCollisions(IList<IBullet> bullets, IList<IPlayer> players)
        {
            var enemiesBullets = bullets.Where(b => b.ShooterId == 0).ToList();

            foreach (Player player in players)
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
        public static void CheckBossBulletsCollisions(IList<IBullet> bullets, IList<IPlayer> players)
        {
            var bossBullets = bullets.Where(b => b.ShooterId == 3).ToList();

            foreach (Player player in players)
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

        public static void CheckPlayerItemCollisions(IList<IItem> items, IList<IPlayer> players)
        {
            foreach (Player player in players)
            {
                foreach (var item in items)
                {
                    if (player.BoundingBox.Intersects(item.BoundingBox))
                    {
                        player.ReactOnColission(item);
                        item.ReactOnColission();
                    }
                }
            }
        }
    }
}


