using Microsoft.Xna.Framework;
using SpaceShipFarcrothu.Data;
using SpaceShipFarcrothu.Models.ModelEntities;
using SpaceShipFartrothu.GameObjects;
using SpaceShipFartrothu.Handlers;
using SpaceShipFartrothu.Interfaces;

namespace SpaceShipFartrothu.Services
{
    public class SaveAndLoadDbManager
    {
        public void SaveGame(IRepository<IPlayer> players, IRepository<IEnemy> enemies,
            IRepository<IAsteroid> asteroids)
        {
            var context = new SpaceShipFarcrothuContext();
            var game = new GameModel();
            foreach (var player in players.GetAll())
            {
                var playerModel = new PlayerModel()
                {
                    Armor = player.Armor,
                    BulletDamage = player.BulletDamage,
                    BulletDelay = player.BulletDelay,
                    BulletSpeed = player.BulletSpeed,
                    IsAlive = player.IsAlive,
                    Level = player.Level,
                    MaxHealth = player.MaxHealth,
                    PlayerIdentity = player.Id,
                    PositionX = player.Position.X,
                    PositionY = player.Position.Y,
                    Score = player.Score,
                    Speed = player.Speed,
                    Health = player.Health
                };
                game.Players.Add(playerModel);
            }
            foreach (var enemy in enemies.GetAll())
            {
                var enemyModel = new EnemyModel()
                {
                    PositionX = enemy.Position.X,
                    PositionY = enemy.Position.Y
                };
                game.Enemies.Add(enemyModel);
            }
            foreach (var asteroid in asteroids.GetAll())
            {
                var asteroidModel = new AsteroidModel()
                {
                    PositionX = asteroid.Position.X,
                    PositionY = asteroid.Position.Y
                };
                game.Asteroids.Add(asteroidModel);
            }
            context.Games.Add(game);
            context.SaveChanges();
        }

        public void LoadGame(IRepository<IPlayer> players, IRepository<IEnemy> enemies,
            IRepository<IAsteroid> asteroids, InputHandler handler)
        {
            var context = new SpaceShipFarcrothuContext();
            var game = context.Games.Find(2);
            foreach (var player in game.Players)
            {
                IPlayer playerEntity = new Player(new Vector2(player.PositionX,
                    player.PositionY), handler, player.PlayerIdentity)
                {
                    Armor = player.Armor,
                    BulletDamage = player.BulletDamage,
                    BulletDelay = player.BulletDelay,
                    BulletSpeed = player.BulletSpeed,
                    MaxHealth = player.MaxHealth,
                    IsAlive = player.IsAlive,
                    Score = player.Score,
                    Speed = player.Speed,
                    Level = player.Level
                };
                
                playerEntity.Health = player.Health;

                players.AddEntity(playerEntity);
            }
            foreach (var enemy in game.Enemies)
            {
                IEnemy enemyEntity = new Enemy(new Vector2(enemy.PositionX, enemy.PositionY));
                enemies.AddEntity(enemyEntity);
            }
            foreach (var asteroid in game.Asteroids)
            {
                IAsteroid asteroidEntity = new Asteroid(new Vector2(asteroid.PositionX,
                    asteroid.PositionY));
                asteroids.AddEntity(asteroidEntity);
            }
        }
    }
}
