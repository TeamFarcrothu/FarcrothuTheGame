namespace SpaceShipFartrothu.Factories
{
    using System;
    using Microsoft.Xna.Framework;
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
            }
        }
    }
}
