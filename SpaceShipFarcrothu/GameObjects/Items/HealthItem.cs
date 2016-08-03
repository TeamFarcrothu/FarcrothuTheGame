namespace SpaceShipFartrothu.GameObjects.Items
{
    using Core;
    using Effects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using SpaceShipFartrothu.Interfaces;

    public class HealthItem : Item
    {
        private const int HealthConst = 5;

        public HealthItem(Texture2D texture, Vector2 position) 
            : base(texture, position)
        {
            this.ItemHealth = HealthConst;
        }
    }
}