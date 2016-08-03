using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShipFartrothu.Utils.Assets;

namespace SpaceShipFartrothu.GameObjects.Items
{
    public class ShipSpeedItem : Item
    {
        private const int DefaultSpeed = 5;

        public ShipSpeedItem(Vector2 position) : base(position)
        {
            this.Texture = TexturesManager.ItemShipSpeedTexture;
            this.ItemShipSpeed = DefaultSpeed;
        }
    }
}
