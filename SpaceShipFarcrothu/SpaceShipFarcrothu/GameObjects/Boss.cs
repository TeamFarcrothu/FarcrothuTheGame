namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss
    {
        private const int DefaultHealth = 50;
        private const int DefaultSpeed = 1;
        private const int DefaultBulletDelay = 20;
        private const int ScreenWidth = 1000;

        private List<Bullet> bulletList;
        private Rectangle boundingBox;
        private Texture2D texture;
        private Texture2D bulletTexture;
        public Vector2 position;
        
        private int health;
        private int bulletDelay;
        private int speed;
        public bool isVisible;

        public Boss(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            this.position = newPosition;
            this.texture = newTexture;
            this.bulletTexture = newBulletTexture;
            this.health = DefaultHealth;
            this.bulletDelay = DefaultBulletDelay;
            this.speed = DefaultSpeed;
            this.isVisible = true;
            this.bulletList = new List<Bullet>();

        }

        public void Update(GameTime gameTime)
        {
            this.boundingBox = new Rectangle(
                (int)this.position.X,
                (int)this.position.Y,
                this.texture.Width,
                this.texture.Height);

            this.position.X += this.speed;
            if (this.position.X >= ScreenWidth)
            {
                this.position.X -= this.speed;
            }

            if (this.position.X <= 0)
            {
                this.position.X += this.speed;
            }

            if (this.health <= 0)
            {
                this.isVisible = false;
            }

            this.BossShoot();

            this.UpdateBossBullets();
        }

        public void BossShoot()
        {
            if (this.bulletDelay >= 0)
            {
                this.bulletDelay--;
            }

            if (this.bulletDelay <= 0)
            {
                if (bulletList.Count < 20)
                {
                    Bullet newBullet = new Bullet(this.bulletTexture);
                    newBullet.Position = new Vector2(
                        this.position.X + this.texture.Width / 2 - newBullet.Texture.Width / 2,
                        this.position.Y + this.texture.Height - 20);

                    newBullet.IsVisible = true;
                    this.bulletList.Add(newBullet);
                }

                if (this.bulletDelay == 0)
                {
                    this.bulletDelay = DefaultBulletDelay;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, Color.White);

            foreach (var bullet in this.bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void UpdateBossBullets()
        {
            foreach (var bullet in this.bulletList)
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
    }
}
