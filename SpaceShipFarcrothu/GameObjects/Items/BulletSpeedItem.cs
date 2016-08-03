using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShipFartrothu.GameObjects.Items
{
    public class BulletSpeedItem : Item
    {
        private const int BulletSpeedConst = 5;

        public BulletSpeedItem(Texture2D texture, Vector2 position) : base(texture, position)
        {
            ItemBulletSpeed = BulletSpeedConst;
        }
    }
}
