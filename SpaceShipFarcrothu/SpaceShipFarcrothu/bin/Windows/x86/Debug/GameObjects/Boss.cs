namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss
    {
        private const int DefaultHealth = 50;
        private const int DefaultSpeed = 2;
        private const int DefaultBulletDelay = 10;
        private const int DefaultSideBulletDelay = 70;
        private const int ScreenWidth = 1181;

        private List<Bullet> bulletList;
        private Rectangle boundingBox;
        private Texture2D texture;
        private Texture2D bulletTexture;
        public Vector2 position;

        private int health;
        private int bulletDelay;
        private int sideBulletDelay;
        private int speed;
        public bool isAtTheRightBorder;
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
            this.isAtTheRightBorder = true;
        }

        public void Update(GameTime gameTime)
        {
            this.boundingBox = new Rectangle(
                (int)this.position.X,
                (int)this.position.Y,
                this.texture.Width,
                this.texture.Height);

            // Showing the Boss slowly from top of the screen
            if (this.position.Y <= 31)
            {
                this.position.Y += this.speed;
                return;
            }

            // Moving  the Boss automaticaly left and right
            if (this.position.X <= ScreenWidth && isAtTheRightBorder)
            {
                if (this.position.X == ScreenWidth)
                {
                    isAtTheRightBorder = false;
                }
                this.position.X += this.speed;
            }
            else if (this.position.X >= -171 && !isAtTheRightBorder)
            {
                if (this.position.X == -171)
                {
                    isAtTheRightBorder = true;
                }
                this.position.X -= this.speed;
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
                        this.position.Y + this.texture.Height - 100);

                    newBullet.IsVisible = true;
                    this.bulletList.Add(newBullet);

                    Bullet leftBullet = new Bullet(this.bulletTexture);
                    leftBullet.Position = new Vector2(
                        this.position.X + this.texture.Width / 2 - leftBullet.Texture.Width / 2 - 130,
                        this.position.Y + this.texture.Height - 210);

                    leftBullet.IsVisible = true;
                    this.bulletList.Add(leftBullet);

                    Bullet rightBullet = new Bullet(this.bulletTexture);
                    rightBullet.Position = new Vector2(
                        this.position.X + this.texture.Width / 2 - rightBullet.Texture.Width / 2 + 130,
                        this.position.Y + this.texture.Height - 210);

                    rightBullet.IsVisible = true;
                    this.bulletList.Add(rightBullet);
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
