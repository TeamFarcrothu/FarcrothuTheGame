using SpaceShipFarcrothu.Data;
using SpaceShipFarcrothu.Models.ModelEntities;
using SpaceShipFartrothu.Interfaces;

namespace SpaceShipFartrothu.Services
{
    public class Save
    {
        public void SavePlayer(IRepository<IPlayer> players)
        {
            var context = new SpaceShipFarcrothuContext();
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
                context.Players.Add(playerModel);
            }
            context.SaveChanges();
        }
    }
}
