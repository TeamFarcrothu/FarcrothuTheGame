using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShipFartrothu.GameObjects.Items
{
    using Utils.Assets;
    public class ArmorItem : Item
    {
        private const int ArmorConst = 5;

        public ArmorItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemArmorTexture;
            this.ItemArmor = ArmorConst;
        }
    }
}
