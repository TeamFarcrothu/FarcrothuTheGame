namespace SpaceShipFartrothu.GameObjects.Items
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceShipFartrothu.Interfaces;

    public class DamageItem : Item
    {
        private const int DamageConst = 5;

        public DamageItem(Texture2D texture, Vector2 position) 
            : base(texture, position)
        {
            this.Damage = DamageConst;
        }
    }
}