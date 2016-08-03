namespace SpaceShipFartrothu.Handlers
{
    using Interfaces;

    public static class EntityCleanerHandler
    {
        public static void ClearEnemyBullets(IRepository<IBullet> bullets)
        {
            for (int i = 0; i < bullets.GetCount(); i++)
            {
                if (!bullets.GetAll()[i].IsVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearEnemies(IRepository<IEnemy> enemies)
        {
            for (int i = 0; i < enemies.GetCount(); i++)
            {
                if (!enemies.GetAll()[i].IsVisible)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearAsteroids(IRepository<IAsteroid> asteroids)
        {
            for (int i = 0; i < asteroids.GetCount(); i++)
            {
                if (!asteroids.GetAll()[i].IsVisible)
                {
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearPlayers(IRepository<IPlayer> players)
        {
            for (int i = 0; i < players.GetCount(); i++)
            {
                IPlayer currentPlayer = players.GetAll()[i];
                if (!currentPlayer.IsAlive)
                {
                    players.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearExplosion(IRepository<IExplosion> explosions)
        {
            for (int i = 0; i < explosions.GetCount(); i++)
            {
                if (!explosions.GetAll()[i].IsVisible)
                {
                    explosions.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
