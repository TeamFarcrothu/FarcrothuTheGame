namespace SpaceShipFartrothu.Players
{
    using System.Collections.Generic;
    using System.Linq;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PlayerNew
    {
        public Texture2D texture /* this holds the texture graphics content of the ship */, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition, resetPosition;
        public Rectangle boundingBox, healthRectangle;
        public List<Bullet> bulletList;
        public int id;  // holds the player identifier
        public string shipTextureFile;  // the ship texture file name
        public int speed;
        public int health;
        public float bulletDelay;
        public bool isColiding;
        public bool isAlive;

        public PlayerNew(string newShipTextureFile, Vector2 newPosition, int newId)
        {
            this.id = newId;
            this.bulletList = new List<Bullet>();
            this.shipTextureFile = newShipTextureFile;
            this.position = newPosition;
            this.bulletDelay = 20;
            this.speed = 5;
            this.isColiding = false;
            this.health = 200;
            if(id == 1)
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

        // Player load content method
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(shipTextureFile);
            bulletTexture = content.Load<Texture2D>("bullet");
            healthTexture = content.Load<Texture2D>("healthbar");
        }

        // Player draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw player itself
            spriteBatch.Draw(texture, position, Color.White);

            // Draw player health
            healthRectangle = new Rectangle(
                (int)healthBarPosition.X,
                (int)healthBarPosition.Y,
                health,
                20);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

            // Draw bullets
            foreach (var bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        // Player update method
        public void Update(GameTime gameTime)
        {
            // if player health is below zero set isAlive flag to false
            if (health <= 0)
            {
                isAlive = false;
            }

            // if isAlive flag is false the player should be dead, so we reset it's position depending on id
            if (!isAlive)
            {
                position = resetPosition;
                return;
            }
            
            // Create bounding box around the player
            boundingBox = new Rectangle(
                (int)position.X,
                (int)position.Y,
                texture.Width,
                texture.Height);

            // Keyboard state monitoring
            var keyState = Keyboard.GetState();

            // Player shooting
            if ((keyState.IsKeyDown(Keys.LeftControl) && id == 1) || (keyState.IsKeyDown(Keys.RightControl) && id == 2))
            {
                Shoot();
            }
            // PlayerNew movement
            if ((keyState.IsKeyDown(Keys.W) && id == 1) || (keyState.IsKeyDown(Keys.Up) && id == 2))
            {
                position.Y = position.Y - speed;
            }
            if ((keyState.IsKeyDown(Keys.A) && id == 1) || (keyState.IsKeyDown(Keys.Left) && id == 2))
            {
                position.X = position.X - speed;
            }
            if ((keyState.IsKeyDown(Keys.S) && id == 1) || (keyState.IsKeyDown(Keys.Down) && id == 2))
            {
                position.Y = position.Y + speed;
            }
            if ((keyState.IsKeyDown(Keys.D) && id == 1) || (keyState.IsKeyDown(Keys.Right) && id == 2))
            {
                position.X = position.X + speed;
            }

            // Moving left and right through screen borders
            if (position.X <= -30 || position.X >= 1366)
            {
                if (position.X > 1366)
                {
                    position.X = -30;
                }
                else if (position.X < -30)
                {
                    position.X = 1366;
                }
            }
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            if (position.Y >= 768 - texture.Height)
            {
                position.Y = 768 - texture.Height;
            }

            // Update bullets fired
            UpdateBullets();
        }

        // Player shooting method
        public void Shoot()
        {
            if (bulletDelay >= 0)
            {
                bulletDelay--;
            }

            if (bulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.Position = new Vector2(
                    position.X + 32 - newBullet.Texture.Width / 2,
                    position.Y + 10);

                newBullet.IsVisible = true;


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

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].IsVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
