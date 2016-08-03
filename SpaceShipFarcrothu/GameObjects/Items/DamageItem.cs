namespace SpaceShipFartrothu.GameObjects.Items
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceShipFartrothu.Interfaces;
    using Utils.Assets;

    public class DamageItem : Item
    {
        private const int DamageConst = 5;

        public DamageItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemDamageTexture;
            this.Damage = DamageConst;
        }
    }
}