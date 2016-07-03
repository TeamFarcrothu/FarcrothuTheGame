namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    public class Enemy
    {
        public Rectangle boundingBox;
        private Texture2D texture, bulletTexture;
        public Vector2 position;
        private int health, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            this.bulletList = new List<Bullet>();
            this.texture = newTexture;
            this.bulletTexture = newBulletTexture;
            this.health = 5;
            this.position = newPosition;
            this.currentDifficultyLevel = 1;
            this.bulletDelay = 40;
            this.speed = 5;
            this.isVisible = true;
        }

        public void Update(GameTime gameTime)
        {
            this.boundingBox = new Rectangle((int) this.position.X, (int) this.position.Y, this.texture.Width, this.texture.Height);

            this.position.Y += this.speed;

            if (this.position.Y >= 768)
            {
                this.position.Y = -75;
            }

            this.EnemyShoot();
            this.UpdateBullets();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, Color.White);
            foreach (Bullet bullet in this.bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void UpdateBullets()
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

        private void EnemyShoot()
        {
            if (this.bulletDelay >= 0)
            {
                this.bulletDelay--;
            }

            if (this.bulletDelay <= 0)
            {
                if (this.bulletList.Count < 20)
                {
                    Bullet newBullet = new Bullet(this.bulletTexture);
                    newBullet.Position = new Vector2(this.position.X + this.texture.Width / 2 - newBullet.Texture.Width / 2, this.position.Y + this.texture.Height);
                    newBullet.IsVisible = true;
                    this.bulletList.Add(newBullet);
                }

                if (this.bulletDelay == 0)
                {
                    this.bulletDelay = 40;
                }
            }
        }
    }
}