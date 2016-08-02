﻿namespace SpaceShipFartrothu.GameObjects.Items
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceShipFartrothu.Interfaces;

    public class DamageItem : Item
    {
        private const int DamagePower = 5; // This can be changed later.

        /* By setting health to ZERO, we make sure not to increase player health
           when adding damage item */
        private const int ZeroHealthPower = 0;

        public override int Health
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DamageItem(Texture2D texture, Vector2 position) 
            : base(texture, position)
        {
            this.Damage = DamagePower;
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            throw new System.NotImplementedException();
        }
    }
}