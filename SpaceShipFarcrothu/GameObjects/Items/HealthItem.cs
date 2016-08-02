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
        private const int HealthPower = 5; // This can be changed later.
        private int health;

        /* By setting damage to ZERO, we make sure not to increase player damage
           when adding health item */
        private const int ZeroDamagePower = 0;

        private static Random random = new Random();

        public static List<GameObject> HealthItems = new List<GameObject>();

        public override int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                this.health = value;
            }
        }

        public HealthItem(Texture2D texture, Vector2 position) 
            : base(texture, position)
        {
            this.Health = HealthPower;
        }

        public override void Update(GameTime gameTime)
        {
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.Globals.MAIN_SCREEN_HEIGHT)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - 75);
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

        //public static void Update()
        //{
        //
        //    for (int i = 0; i < HealthItems.Count; i++)
        //    {
        //        if (!HealthItems[i].IsVisible)
        //        {
        //            HealthItems.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //}

        public static void LoadItems(Vector2 position)
        {
            var newRand = new Random();
            int nextRandom = newRand.Next(1, 10);

            if (nextRandom < 2)
            {
                if (HealthItems.Count < 1)
                {
                    HealthItems.Add(new HealthItem(GameEngine.itemTexture, position));
                }
            }
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            if (target is Player)
            {
                //(target as Player).Health += this.Health;
                (target as Player).AddItem(this);
            }
            this.IsVisible = false;
        }     
    }
}