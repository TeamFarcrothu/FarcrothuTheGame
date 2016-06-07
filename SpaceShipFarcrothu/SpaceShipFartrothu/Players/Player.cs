namespace SpaceShipFartrothu.Players
{
    using System.Collections.Generic;
    using System.Linq;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player
    {
        public Texture2D Texture, BulletTexture, HealthTexture;
        public Vector2 Position, HealthBarPosition;
        public Rectangle BoundingBox, HealthRectangle;
        public List<Bullet> BulletList;

        public int Speed;
        public int Health;
        public float BulletDelay;
        public bool IsColiding;
        public bool isAlive;

        public Player()
        {
            this.BulletList = new List<Bullet>();
            this.Texture = null;
            this.Position = new Vector2(600, 600);
            this.BulletDelay = 20;
            this.Speed = 10;
            this.IsColiding = false;
            this.Health = 200;
            this.HealthBarPosition = new Vector2(50, 50);
            this.isAlive = true;
        }
        public void LoadContent(ContentManager content)
        {
            this.Texture = content.Load<Texture2D>("ship_p1");
            this.BulletTexture = content.Load<Texture2D>("bullet");
            this.HealthTexture = content.Load<Texture2D>("healthbar");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
            spriteBatch.Draw(this.HealthTexture, this.HealthRectangle, Color.White);

            foreach (var bullet in this.BulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            this.BoundingBox = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                this.Texture.Width,
                this.Texture.Height);

            this.HealthRectangle = new Rectangle(
                (int)this.HealthBarPosition.X,
                (int)this.HealthBarPosition.Y,
                this.Health,
                25);

            if (keyState.IsKeyDown(Keys.LeftAlt))
            {
                this.Shoot();
            }

            this.UpdateBullets();
            //Moving faster
            if (keyState.IsKeyDown(Keys.Space))
            {
                this.Speed = 10;
            }
            if (keyState.IsKeyUp(Keys.Space))
            {
                this.Speed = 5;
            }


            if (keyState.IsKeyDown(Keys.W))
            {
                this.Position.Y = this.Position.Y - this.Speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                this.Position.X = this.Position.X - this.Speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                this.Position.Y = this.Position.Y + this.Speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                this.Position.X = this.Position.X + this.Speed;
            }

            if (this.Position.X <= 0)
            {
                this.Position.X = 0;
            }
            if (this.Position.X >= 1366 - this.Texture.Width)
            {
                this.Position.X = 1366 - this.Texture.Width;
            }
            if (this.Position.Y <= 0)
            {
                this.Position.Y = 0;
            }
            if (this.Position.Y >= 768 - this.Texture.Height)
            {
                this.Position.Y = 768 - this.Texture.Height;
            }

        }
        public void Shoot()
        {
            if (this.BulletDelay >= 0)
            {
                this.BulletDelay--;
            }

            if (this.BulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(this.BulletTexture);
                newBullet.Position = new Vector2(
                    this.Position.X + 32 - newBullet.Texture.Width / 20,
                    this.Position.Y + 30);

                newBullet.IsVisible = true;


                if (this.BulletList.Count() < 20)
                {
                    this.BulletList.Add(newBullet);
                }
            }
            if (this.BulletDelay == 0)
            {
                this.BulletDelay = 20;
            }
        }

        public void UpdateBullets()
        {
            foreach (Bullet b in this.BulletList)
            {
                b.BoundingBox = new Rectangle(
                (int)b.Position.X,
                (int)b.Position.Y,
                b.Texture.Width,
                b.Texture.Height);

                b.Position.Y = b.Position.Y - b.Speed;
                if (b.Position.Y <= 0)
                {
                    b.IsVisible = false;
                }
            }

            for (int i = 0; i < this.BulletList.Count; i++)
            {
                if (!this.BulletList[i].IsVisible)
                {
                    this.BulletList.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
