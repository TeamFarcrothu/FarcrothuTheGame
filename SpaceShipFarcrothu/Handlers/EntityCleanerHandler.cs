namespace SpaceShipFartrothu.Handlers
{
    using System.Collections.Generic;
    using SpaceShipFartrothu.GameObjects;
    using SpaceShipFartrothu.Interfaces;

    public static class EntityCleanerHandler
    {
        public static void ClearEnemyBullets(IList<IBullet> bullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].IsVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearEnemies(IList<IGameObject> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].IsVisible)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void ClearExplosion(IList<IExplosion> explosions)
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                if (!explosions[i].IsVisible)
                {
                    explosions.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
