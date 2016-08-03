using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShipFartrothu.GameObjects.Items
{
    public class ArmorItem : Item
    {
        private const int ArmorConst = 5;

        public ArmorItem(Texture2D texture, Vector2 position) 
            : base(texture, position)
        {
            this.ItemArmor = ArmorConst;
        }
    }
}
