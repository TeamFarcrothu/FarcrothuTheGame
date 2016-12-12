using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFarcrothu.Models.ModelEntities
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int BulletDamage { get; set; }
        public int BulletDelay { get; set; }
        public int BulletSpeed { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Armor { get; set; }
        public int Score { get; set; }
        public float PositionX  { get; set; }
        public float PositionY { get; set; }
        public int PlayerIdentity { get; set; }
        public bool IsAlive { get; set; }
    }
}
