using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.GameObjects.Items;
using SpaceShipFartrothu.Multimedia;

namespace SpaceShipFartrothu.GameObjects
{
    public class Player : GameObject
    {
        private const int DefaultBulletDamage = 2;
        private const int DefaultSpeed = 5;
        private const int DefaultBulletDelay = 20;
        private const int DefaultHealth = 100;

        private Texture2D bulletTexture, healthTexture;

        private readonly Vector2 resetPosition;

        private Rectangle healthRectangle;
        private List<Item> items;

        private int id;  // holds the player identifier

        private readonly string shipTextureFile;  // the ship Texture file name

        private int health;
        private int maxHealth;

        private int level; //***

        private int bulletDamage;
        private float bulletDelay;

        private bool isColiding;
        public bool isAlive;
        //public bool isSecondBulletActive;
        //public bool isThirdBulletActive;

        private SoundManager sm = new SoundManager();

        public static List<Player> Players = new List<Player>();

        public Player(Texture2D texture, Vector2 position, int id)
            : base(texture, position)
        {
            this.Level = 1;
            this.Id = id;
            this.BulletDamage = DefaultBulletDamage;
            this.BulletDelay = DefaultBulletDelay;
            this.Speed = DefaultSpeed;
            this.MaxHealth = DefaultHealth;
            this.Health = DefaultHealth;
            this.Score = 0;
            this.items = new List<Item>();
            //TODO: check if needed
            // this.isColiding = false;

            //this.isSecondBulletActive = false;
            // this.isThirdBulletActive = false;

            this.IsAlive = true;
            if (this.id == 1)
            {
                this.resetPosition = new Vector2(200, 600);
            }
            else
            {
                this.resetPosition = new Vector2(1000, 600);
            }

            Players.Add(this);
        }

        public int Level //***
        {
            get
            {
                return this.level;
            }

            set
            {
                this.level = value;
            }
        }

        public int Id { get { return this.id; } set { this.id = value; } }

        public bool IsAlive { get { return this.isAlive; } set { this.isAlive = value; } }

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

        public int Score { get; set; }

        public int Health
        {
            get
            {
                return this.health;
            }
            set
            {
                if (this.health + value >= this.MaxHealth)
                {
                    this.health = MaxHealth;
                }
                else
                {
                    this.health += value;
                }
            }
        }

        public int BulletDelay { get; set; }
        public int MaxHealth { get; set; }

        public void AddItem(Item item)
        {
            //this.items.Add(item);

            this.Health += item.Health;
            this.BulletDamage += item.Damage;
        }

        // Player load content method
        public void LoadContent(ContentManager content)
        {
            //this.Texture = content.Load<Texture2D>(this.shipTextureFile);
            this.bulletTexture = content.Load<Texture2D>("bullet");
            this.healthTexture = content.Load<Texture2D>("healthbar");
            this.sm.LoadContent(content);
        }

        // Player draw method
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw player itself if he is alive
            if (this.isAlive)
                spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }

        // Player update method
        public override void Update(GameTime gameTime)
        {
            // if player health is below zero set isAlive flag to false
            if (this.Health <= 0)
            {
                this.IsAlive = false;
            }

            // if isAlive flag is false the player should be dead, so we reset it's Position depending on id
            if (!this.IsAlive)
            {
                this.Position = this.resetPosition;
                return;
            }

            // Create bounding box around the player
            this.BoundingBox = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Texture.Width - 5,
                    this.Texture.Height - 5
                );

            // Keyboard state monitoring
            var keyState = Keyboard.GetState();

            // Player shooting
            if ((keyState.IsKeyDown(Keys.LeftControl) && this.id == 2) || (keyState.IsKeyDown(Keys.LeftControl) && this.id == 1))
            {
                this.Shoot();
            }

            // Player movement
            if ((keyState.IsKeyDown(Keys.W) && this.id == 2) || (keyState.IsKeyDown(Keys.Up) && this.id == 1))
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - this.Speed);
            }
            if ((keyState.IsKeyDown(Keys.A) && this.id == 2) || (keyState.IsKeyDown(Keys.Left) && this.id == 1))
            {
                this.Position = new Vector2(this.Position.X - this.Speed, this.Position.Y);
            }
            if ((keyState.IsKeyDown(Keys.S) && this.id == 2) || (keyState.IsKeyDown(Keys.Down) && this.id == 1))
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);
            }
            if ((keyState.IsKeyDown(Keys.D) && this.id == 2) || (keyState.IsKeyDown(Keys.Right) && this.id == 1))
            {
                this.Position = new Vector2(this.Position.X + this.Speed, this.Position.Y);
            }

            // Moving left and right through screen borders
            if (this.Position.X <= -30 || this.Position.X >= Globals.Globals.MAIN_SCREEN_WIDTH)
            {
                if (this.Position.X > Globals.Globals.MAIN_SCREEN_WIDTH)
                {
                    this.Position = new Vector2(this.Position.X - Globals.Globals.MAIN_SCREEN_WIDTH, this.Position.Y);
                }
                else if (this.Position.X < -30)
                {
                    this.Position = new Vector2(Globals.Globals.MAIN_SCREEN_WIDTH, this.Position.Y);
                }
            }
            if (this.Position.Y <= 0)
            {
                this.Position = new Vector2(this.Position.X, 0);
            }
            if (this.Position.Y >= Globals.Globals.MAIN_SCREEN_HEIGHT - this.Texture.Height)
            {
                this.Position = new Vector2(this.Position.X, Globals.Globals.MAIN_SCREEN_HEIGHT - this.Texture.Height);
            }

        }

        // Player shooting method
        private void Shoot()
        {
            if (this.BulletDelay >= 0)
            {
                this.BulletDelay--;
            }

            if (this.BulletDelay <= 0)
            {
                this.sm.playerShootSound.Play();

                var newBulletPosition = new Vector2(this.Position.X + 32 - this.bulletTexture.Width / 2, this.Position.Y + 10);

                Bullet newBullet = new Bullet(this.bulletTexture, newBulletPosition, this.Id, this.BulletDamage);

                if (Bullet.Bullets.Where(b => b.ShooterId == this.Id).ToList().Count < 20)
                {
                    Bullet.Bullets.Add(newBullet);
                }
            }
            if (this.BulletDelay == 0)
            {
                this.BulletDelay = 10;
            }
        }

        public override void ReactOnColission(GameObject target)
        {
            string currentTargetType = target.GetType().Name;

            if (currentTargetType == "Asteroid")
            {
                //TODO: add points to player score and reduce health
                //this.soundManager.explodeSound.Play();
                //this.hud.playerscore += 5;

                this.Health = -target.Damage;

            }

            else if (currentTargetType == "Enemy")
            {

                //TODO: add points to player score and reduce health
                this.Health = -target.Damage;
            }

            else if (currentTargetType == "Boss")
            {

                //TODO: add points to player score and reduce health
                this.Health = -target.Damage;
            }

            else if (currentTargetType == "Bullet")
            {
                //CHeck bullet id :   If is 0 = enemy ;If is > 2 = some boss or other enemy
                //Then initiate proper reaction 
                //var bullet = target as Bullet;
                //var bulletId = bullet.ShooterId;
                //if(bulletId == 0)

                this.Health = -target.Damage;
            }
        }
    }
}
