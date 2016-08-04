namespace SpaceShipFartrothu.Factories
{
    using System;
    using Microsoft.Xna.Framework;
    using GameObjects;
    using Interfaces;

    public static class EnemyFactory
    {
        public static void CreateEnemies(IRepository<IEnemy> enemies, Random random)
        {          
            int randomX = random.Next(0, 1200) - random.Next(0, 20);
            int randomY = random.Next(-700, -50) + random.Next(0, 100);

            if (enemies.GetCount() < 5)
            {
                enemies.AddEntity(new Enemy(new Vector2(randomX, randomY)));
            }
        }
    }
}
