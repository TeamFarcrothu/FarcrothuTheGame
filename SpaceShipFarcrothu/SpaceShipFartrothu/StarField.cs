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
    public class StarField
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        public StarField()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -768);
            speed = 1;
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("space");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            if (bgPos1.Y >= 768)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -768;
            }
        }
    }
}
