namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 40;
            speed = 5;
            isVisible = true;
        }

        public void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            position.Y += speed;

            if(position.Y >= 768)
            {
                position.Y = -75;
            }

            EnemyShoot();
            UpdateBullets();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,position,Color.White);
            foreach(Bullet bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void UpdateBullets()
        {
            foreach (Bullet bullet in this.bulletList)
            {
                bullet.BoundingBox = new Rectangle(
                (int)bullet.Position.X,
                (int)bullet.Position.Y,
                bullet.Texture.Width,
                bullet.Texture.Height);

                bullet.Position.Y = bullet.Position.Y + bullet.Speed;
                if (bullet.Position.Y >= 768)
                {
                    bullet.IsVisible = false;
                }
            }

            for (int i = 0; i < this.bulletList.Count; i++)
            {
                if (!this.bulletList[i].IsVisible)
                {
                    this.bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
        public void EnemyShoot()
        {
            if(bulletDelay >= 0)
            {
                bulletDelay--;
            }

            if(bulletDelay <= 0)
            {
                if (bulletList.Count < 20)
                {
                    Bullet newBullet = new Bullet(bulletTexture);
                    newBullet.Position = new Vector2(this.position.X + this.texture.Width / 2 - newBullet.Texture.Width / 2, this.position.Y + this.texture.Height);
                    newBullet.IsVisible = true;
                    bulletList.Add(newBullet);
                }

                if(bulletDelay == 0)
                {
                    bulletDelay = 40;
                }
            }
        }
    }
}
