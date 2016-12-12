using Microsoft.Xna.Framework;
using SpaceShipFarcrothu.Data;
using SpaceShipFarcrothu.Models.ModelEntities;
using SpaceShipFartrothu.Data;
using SpaceShipFartrothu.GameObjects;
using SpaceShipFartrothu.Handlers;
using SpaceShipFartrothu.Interfaces;
using SpaceShipFartrothu.Utils.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShipFartrothu.Services
{
    public class SaveAndLoadDbManager
    {
        private readonly SpaceShipFarcrothuContext context;

        public SaveAndLoadDbManager()
        {
            this.context = new SpaceShipFarcrothuContext();
        }
        public void SaveGame(IRepository<IPlayer> players, IRepository<IEnemy> enemies,
            IRepository<IAsteroid> asteroids, UsersRepository users)
        {
            var game = new GameModel();

            foreach (var user in users.GetAll())
            {
                game.Users.Add(this.context.Users.FirstOrDefault(u => u.Username == user));
            }

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

        public State LoadGame(IRepository<IPlayer> players, IRepository<IEnemy> enemies,
            IRepository<IAsteroid> asteroids, InputHandler handler, UsersRepository users, int gameId)
        {
            if (gameId == 0)
            {
                return State.LoadGameMenu;
            }
            var context = new SpaceShipFarcrothuContext();
            // var game = context.Users.Where(u => users.GetAll().Contains(u.Username)).Select(u => u.Games);

            var game = context.Games.FirstOrDefault(g => g.Id == gameId);
            
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
                
                playerEntity.Health = player.Health - 100;

                players.AddEntity(playerEntity);
            }
            foreach (var enemy in game.Enemies)
            {
                IEnemy enemyEntity = new GameObjects.Enemy(new Vector2(enemy.PositionX, enemy.PositionY));
                enemies.AddEntity(enemyEntity);
            }
            foreach (var asteroid in game.Asteroids)
            {
                IAsteroid asteroidEntity = new GameObjects.Asteroid(new Vector2(asteroid.PositionX,
                    asteroid.PositionY));
                asteroids.AddEntity(asteroidEntity);
            }
            return State.OnePlayer;
        }

        public ICollection<int> GetUserGamesId(ICollection<string> users)
        {
            var gameIds = context.Users.Where(u => users.Contains(u.Username))
                .Select(u => u.Games);

            var listOfIds = new List<int>();

            foreach (var game in gameIds)
            {
                IEnumerable<int> ids = game.Select(g => g.Id).OrderByDescending(x => x).Take(5);

                foreach (var id in ids)
                {
                    listOfIds.Add(id);
                }
            }
            return listOfIds;
        }
    }
}
