using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipFartrothu
{
    public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public int speed;
        public bool isColiding, destroyed;

        public Asteroid()
        {
            position = new Vector2(400, -50);
            texture = null;
            speed = 4;
            isColiding = false;
            destroyed = false;
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("asteroid");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }
        public void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            position.Y = position.Y + speed;
            if (position.Y >= 768)
            {
                position.Y = -50;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!destroyed)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }

    }
}
