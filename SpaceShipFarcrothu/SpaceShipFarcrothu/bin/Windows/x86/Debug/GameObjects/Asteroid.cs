﻿namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Asteroid
    {
        public Rectangle BoundingBox;
        public Texture2D Texture;
        public Vector2 Position;

        // Rotation is not working at the moment in order to get proper collision
        // public Vector2 Origin;
        //public float RotationAngle;
        public int Speed;
        public bool IsVisible;

        public float RandomX;
        public float RandomY;

        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            this.Position = newPosition;
            this.Texture = newTexture;
            this.Speed = 4;
            this.IsVisible = true;

            // Rotation is not working at the moment in order to get proper collision
            //this.Origin.X = Texture.Width / 2;
            //this.Origin.Y = Texture.Height / 2;
        }

        //public void LoadContent(ContentManager content)
        //{
        //}

        public void Update(GameTime gemeTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            Position.Y += Speed;
            if (Position.Y >= 768)
            {
                Position.Y = -75;
            }

            // Rotation is not working at the moment in order to get proper collision

            //float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            //RotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //RotationAngle %= circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(Texture,Position,Color.White);

                // Rotation is not working at the moment in order to get proper collision
                /*spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.White,
                    RotationAngle,
                    Origin,
                    1.0f,
                    SpriteEffects.None,
                    0f);*/
            }
        }
    }
}