namespace SpaceShipFartrothu.Factories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Core;
    using GameObjects;
    using Interfaces;

    public static class AsteroidFactory
    {
        public static void CreateAsteroids(IRepository<IAsteroid> asteroids, Random random)
        {
            int randomX = random.Next(0, 1200) - random.Next(0, 20);
            int randomY = random.Next(-700, -50) + random.Next(0, 100);

            if (asteroids.GetCount() < 15)
            {
                asteroids.AddEntity(new Asteroid(new Vector2(randomX, randomY)));
                //asteroids.Add(new Asteroid(new Vector2(randomX, randomY)));
            }

            for (int i = 0; i < asteroids.GetCount(); i++)
            {
                if (!asteroids.GetAll()[i].IsVisible)
                {
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
