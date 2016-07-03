namespace SpaceShipFartrothu.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Sound;
    using GameObjects;
    using GameObjects.Items;

    public class PlayerNew
    {
        private const int DefaultBulletDamage = 2;

        public Texture2D texture /* this holds the texture graphics content of the ship */, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition, resetPosition;
        public Rectangle boundingBox, healthRectangle;
        public List<Bullet> bulletList;
        private List<Item> items;

        private int id;  // holds the player identifier
        private string shipTextureFile;  // the ship texture file name
        private int speed;
        public int health;
        private int bulletDamage;
        private float bulletDelay;
        private bool isColiding;
        public bool isAlive;
        public bool isSecondBulletActive;

        private SoundManager sm = new SoundManager();

        //Constructor method
        public PlayerNew(string newShipTextureFile, Vector2 newPosition, int newId)
        {
            this.id = newId;
            this.bulletList = new List<Bullet>();
            this.BulletDamage = DefaultBulletDamage;
            this.items = new List<Item>();
            this.shipTextureFile = newShipTextureFile;
            this.position = newPosition;
            this.bulletDelay = 20;
            this.speed = 5;
            this.isColiding = false;
            this.isSecondBulletActive = false;
            this.health = 200;
            if (this.id == 1)
            {
                this.healthBarPosition = new Vector2(50, 50);
                this.resetPosition = new Vector2(200, 600);
            }
            else
            {
                this.healthBarPosition = new Vector2(1110, 50);
                this.resetPosition = new Vector2(1000, 600);
            }
            this.isAlive = true;
        }

        public int BulletDamage
        {
            get { return this.bulletDamage; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Damage cannot be negative");
                }

                this.bulletDamage = value;
            }
        }

        public void AddItem(Item item)
        {
            this.items.Add(item);

            this.health += item.Health;
            this.BulletDamage += item.Damage;
        }

        // Player load content method
        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>(this.shipTextureFile);
            this.bulletTexture = content.Load<Texture2D>("bullet");
            this.healthTexture = content.Load<Texture2D>("healthbar");
            this.sm.LoadContent(content);
        }

        // Player draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw player itself if he is alive
            if (this.isAlive)
                spriteBatch.Draw(this.texture, this.position, Color.White);

            // Draw player health
            this.healthRectangle = new Rectangle(
                (int) this.healthBarPosition.X,
                (int) this.healthBarPosition.Y, this.health,
                20);
            spriteBatch.Draw(this.healthTexture, this.healthRectangle, Color.White);

            // Draw bullets
            foreach (var bullet in this.bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        // Player update method
        public void Update(GameTime gameTime)
        {
            // if player health is below zero set isAlive flag to false
            if (this.health <= 0)
            {
                this.isAlive = false;
            }

            // if isAlive flag is false the player should be dead, so we reset it's position depending on id
            if (!this.isAlive)
            {
                this.position = this.resetPosition;
                return;
            }

            // Create bounding box around the player
            this.boundingBox = new Rectangle(
                (int) this.position.X,
                (int) this.position.Y, this.texture.Width, this.texture.Height);

            // Keyboard state monitoring
            var keyState = Keyboard.GetState();

            // Player shooting
            if ((keyState.IsKeyDown(Keys.RightControl) && this.id == 2) || (keyState.IsKeyDown(Keys.LeftControl) && this.id == 1))
            {
                this.Shoot();
            }
            // PlayerNew movement
            if ((keyState.IsKeyDown(Keys.W) && this.id == 2) || (keyState.IsKeyDown(Keys.Up) && this.id == 1))
            {
                this.position.Y = this.position.Y - this.speed;
            }
            if ((keyState.IsKeyDown(Keys.A) && this.id == 2) || (keyState.IsKeyDown(Keys.Left) && this.id == 1))
            {
                this.position.X = this.position.X - this.speed;
            }
            if ((keyState.IsKeyDown(Keys.S) && this.id == 2) || (keyState.IsKeyDown(Keys.Down) && this.id == 1))
            {
                this.position.Y = this.position.Y + this.speed;
            }
            if ((keyState.IsKeyDown(Keys.D) && this.id == 2) || (keyState.IsKeyDown(Keys.Right) && this.id == 1))
            {
                this.position.X = this.position.X + this.speed;
            }

            // Moving left and right through screen borders
            if (this.position.X <= -30 || this.position.X >= 1366)
            {
                if (this.position.X > 1366)
                {
                    this.position.X = -30;
                }
                else if (this.position.X < -30)
                {
                    this.position.X = 1366;
                }
            }
            if (this.position.Y <= 0)
            {
                this.position.Y = 0;
            }
            if (this.position.Y >= 768 - this.texture.Height)
            {
                this.position.Y = 768 - this.texture.Height;
            }

            // Update bullets fired
            this.UpdateBullets();
        }

        // Player shooting method
        private void Shoot()
        {
            if (this.bulletDelay >= 0)
            {
                this.bulletDelay--;
            }

            if (this.bulletDelay <= 0)
            {
                this.sm.playerShootSound.Play();
                Bullet newBullet = new Bullet(this.bulletTexture);
                newBullet.Position = new Vector2(this.position.X + 32 - newBullet.Texture.Width / 2, this.position.Y + 10);

                newBullet.IsVisible = true;

                Bullet secondBullet = new Bullet(this.bulletTexture);

                if (this.isSecondBulletActive)
                {

                    secondBullet.Position = new Vector2(this.position.X + 42 - secondBullet.Texture.Width / 2, this.position.Y + 10);

                    secondBullet.IsVisible = true;
                }

                if (this.bulletList.Count() < 20)
                {
                    this.bulletList.Add(newBullet);
                    if (this.isSecondBulletActive)
                        this.bulletList.Add(secondBullet);
                }
            }
            if (this.bulletDelay == 0)
            {
                this.bulletDelay = 10;
            }
        }

        private void UpdateBullets()
        {
            foreach (Bullet b in this.bulletList)
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
