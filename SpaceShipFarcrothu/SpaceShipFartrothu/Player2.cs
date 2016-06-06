using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu;

namespace Fartrothu
{
    public class Player2
    {
        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition;
        public int speed;
        public int health;
        public float bulletDelay;
        public Rectangle boundingBox, healthRectangle;
        public bool isColiding;
        public List<Bullet> bulletList;

        public Player2()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(700, 600);
            bulletDelay = 20;
            speed = 10;
            isColiding = false;
            this.health = 200;
            this.healthBarPosition = new Vector2(1110, 50);
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship_p2");
            bulletTexture = Content.Load<Texture2D>("bullet");
            this.healthTexture = Content.Load<Texture2D>("healthbar");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(this.healthTexture, this.healthRectangle, Color.White);

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            this.boundingBox = new Rectangle(
                (int)this.position.X,
                (int)this.position.Y,
                this.texture.Width,
                this.texture.Height);

            this.healthRectangle = new Rectangle(
                (int)this.healthBarPosition.X,
                (int)this.healthBarPosition.Y,
                this.health, 25);

            if (keyState.IsKeyDown(Keys.RightAlt))
            {
                Shoot();
            }

            this.UpdateBullets();
            //Moving faster
            if (keyState.IsKeyDown(Keys.Space))
            {
                speed = 10;
            }
            if (keyState.IsKeyUp(Keys.Space))
            {
                speed = 5;
            }


            if (keyState.IsKeyDown(Keys.Up))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                position.X = position.X - speed;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.Right))
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
            if (position.Y >= 768 - texture.Height)
            {
                position.Y = 768 - texture.Height;
            }
        }

        public void Shoot()
        {
            if (bulletDelay >= 0)
            {
                bulletDelay--;
            }

            if (bulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;


                if (bulletList.Count() < 20)
                {
                    bulletList.Add(newBullet);
                }
            }
            if (bulletDelay == 0)
            {
                bulletDelay = 20;
            }
        }

        public void UpdateBullets()
        {
            foreach (Bullet b in bulletList)
            {
                b.boundingBox = new Rectangle(
                (int)b.position.X,
                (int)b.position.Y,
                b.texture.Width,
                b.texture.Height);

                b.position.Y = b.position.Y - b.speed;
                if (b.position.Y <= 0)
                {
                    b.isVisible = false;
                }
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
