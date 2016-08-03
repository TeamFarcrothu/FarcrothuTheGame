namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Items;
    using Interfaces;
    using Utils.Assets;

    public class Player : GameObject, IPlayer
    {
        private const int DefaultBulletDamage = 2;
        private const int DefaultSpeed = 5;
        private const int DefaultBulletDelay = 11;
        private const int DefaultHealth = 100;

        private readonly Vector2 resetPosition;
       // private readonly Texture2D bulleTexture = AssetsLoader.BulletTexture;

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

        public Player(Vector2 position, int id)
            : base(position)
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
                     
            this.Position = position;
            this.IsAlive = true;

            if (this.id == 1)
            {
                this.Texture = TexturesManager.Player1Texture;
                this.resetPosition = new Vector2(200, 600);
            }
            else
            {
                this.Texture = TexturesManager.Player2Texture;
                this.resetPosition = new Vector2(1000, 600);
            }
        }

        public new Vector2 Position { get; set; }

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

        public Texture2D BulletTexture { get; set; }

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

            //this.Health += item.Health;
            //this.BulletDamage += item.Damage;
        }

        // Player load content method
        public void LoadContent(ContentManager content)
        {
           // this.Texture = content.Load<Texture2D>(this.shipTextureFile);
           //this.BulletTexture = content.Load<Texture2D>("bullet");
           // this.healthTexture = content.Load<Texture2D>("healthbar");
           // this.SoundManager.LoadContent(content);
        }

        // Player draw method
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw player itself if he is alive
            if (this.isAlive)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);
            }
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
        }

        public override void ReactOnColission(IGameObject target)
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
