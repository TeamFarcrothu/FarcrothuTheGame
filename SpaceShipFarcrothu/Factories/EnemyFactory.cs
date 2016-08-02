namespace SpaceShipFartrothu.Factories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GameObjects;
    using Interfaces;

    public static class EnemyFactory
    {
        public static void CreateEnemies(IList<IGameObject> enemies, Random random, Texture2D enemyTexture, Texture2D bulletTexture)
        {          
            int randomX = random.Next(0, 1200) - random.Next(0, 20);
            int randomY = random.Next(-700, -50) + random.Next(0, 100);

            if (enemies.Count < 5)
            {
                enemies.Add(new Enemy(enemyTexture, new Vector2(randomX, randomY), bulletTexture));
            }
        }
    }
}
