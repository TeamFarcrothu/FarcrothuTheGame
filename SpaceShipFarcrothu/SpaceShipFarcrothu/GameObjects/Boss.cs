namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

    public class Boss
    {
        private const int DefaultHealth = 50;
        private const int DefaultSpeed = 2;
        private const int DefaultBulletDelay = 10;
        private const int DefaultSideBulletDelay = 70;
        private const int ScreenWidth = 1181;

        public List<Bullet> bulletList;
        public Rectangle boundingBox, healthRectangle;
        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition;

        public int health;
        private int bulletDelay;
        private int sideBulletDelay;
        private int speed;
        private bool isAtTheRightBorder;
        public bool isVisible;


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("space_Boss_Level_1");
            bulletTexture = content.Load<Texture2D>("bullet");
            healthTexture = content.Load<Texture2D>("healthbar");
            this.position = new Vector2(501, -600);
        }

        public Boss()
        {
            this.health = DefaultHealth;
            this.bulletDelay = DefaultBulletDelay;
            this.speed = DefaultSpeed;
            this.isVisible = true;
            this.bulletList = new List<Bullet>();
            this.isAtTheRightBorder = true;
            this.healthBarPosition = this.position;
        }

        public void Update(GameTime gameTime)
        {
            this.boundingBox = new Rectangle(
                (int)this.position.X,
                (int)this.position.Y,
                this.texture.Width,
                this.texture.Height);

            // Showing the Boss slowly from top of the screen
            if (this.position.Y < 31)
            {
                this.position.Y += this.speed;
                healthBarPosition = new Vector2(position.X + 140, position.Y - 30);
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
                healthBarPosition = new Vector2(position.X + 140, position.Y - 30);
            }
            else if (this.position.X >= -171 && !isAtTheRightBorder)
            {
                if (this.position.X == -171)
                {
                    isAtTheRightBorder = true;
                }
                this.position.X -= this.speed;
                healthBarPosition = new Vector2(position.X + 140, position.Y - 30);
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

            healthRectangle = new Rectangle(
               (int)healthBarPosition.X,
               (int)healthBarPosition.Y,
               health,
               20);

            spriteBatch.Draw(this.healthTexture, this.healthRectangle, Color.White);

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
