namespace SpaceShipFartrothu.GameObjects.Items
{
    using Core;
    using Effects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using SpaceShipFartrothu.Interfaces;
    using Utils.Assets;

    public class HealthItem : Item
    {
        private const int HealthConst = 5;

        public HealthItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemHealthTexture;
            this.ItemHealth = HealthConst;
        }
    }
}