namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Globals;
    using System;
    using System.Collections.Generic;
    using Core;
    using SpaceShipFartrothu.Interfaces;

    public abstract class Item : FallingObject, ICollectable
    {
        private const int DefaultSpeed = 4;

        protected Item(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Speed = DefaultSpeed;
        }

        public int ItemHealth { get; protected set; }
        public int ItemDamage { get; protected set; }
        public int ItemArmor { get; protected set; }
        public int ItemBulletSpeed { get; protected set; }

        public override void Update(GameTime gameTime)
        {
            this.BoundingBox = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Texture.Width,
                    this.Texture.Height
                );

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.MAIN_SCREEN_HEIGHT)
            {
                this.IsVisible = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);
            }
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            this.IsVisible = true;
        }
    }
}