namespace SpaceShipFartrothu.Factories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Core;
    using GameObjects;
    using Interfaces;
    using Microsoft.Xna.Framework.Graphics;

    public static class AsteroidFactory
    {
        public static void CreateAsteroids(IList<IGameObject> asteroids, Random random, Texture2D asteroidTexture)
        {
            int randomX = random.Next(0, 1200) - random.Next(0, 20);
            int randomY = random.Next(-700, -50) + random.Next(0, 100);

            if (asteroids.Count < 15)
            {
                asteroids.Add(new Asteroid(asteroidTexture, new Vector2(randomX, randomY)));
            }

            for (int i = 0; i < asteroids.Count; i++)
            {
                if (!asteroids[i].IsVisible)
                {
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
