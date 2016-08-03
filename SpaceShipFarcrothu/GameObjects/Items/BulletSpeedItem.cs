using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShipFartrothu.Utils.Assets;

namespace SpaceShipFartrothu.GameObjects.Items
{
    public class BulletSpeedItem : Item
    {
        private const int BulletSpeedConst = 5;

        public BulletSpeedItem(Vector2 position) : base(position)
        {
            this.Texture = TexturesManager.ItemBulletSpeedTexture;
            ItemBulletSpeed = BulletSpeedConst;
        }
    }
}
