using SpaceShipFarcrothu.Data;
using SpaceShipFarcrothu.Models.ModelEntities;
using SpaceShipFartrothu.Interfaces;

namespace SpaceShipFartrothu.Services
{
    public class DatabaseManager
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
    }
}
