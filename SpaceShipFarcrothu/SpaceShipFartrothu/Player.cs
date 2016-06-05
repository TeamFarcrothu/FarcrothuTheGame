using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu;

namespace Fartrothu
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 position;
        public int speed;


        public Rectangle boundingBox;
        public bool isColiding;

        public Player()
        {
            texture = null;
            position = new Vector2(600, 600);
            speed = 5;
            isColiding = false;
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship_p1");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            //Moving faster
            if (keyState.IsKeyDown(Keys.Space))
            {
                speed = 10;
            }
            if (keyState.IsKeyUp(Keys.Space))
            {
                speed = 5;
            }


            if (keyState.IsKeyDown(Keys.W))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X = position.X - speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X = position.X + speed;
            }

            if (position.X <= 0)
            {
                position.X = 0;
            }
            if (position.X >= 1366 - texture.Width)
            {
                position.X = 1366 - texture.Width;
            }
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            if (position.Y >= 768 - texture.Height )
            {
                position.Y = 768 - texture.Height;
            }
        }

    }
}
