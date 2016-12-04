namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Items;
    using Interfaces;
    using Handlers;
    using Utils.Assets;
    using Core;

    public class Player : GameObject, IPlayer
    {
        private const int DefaultBulletDamage = 2;
        private const int DefaultSpeed = 5;
        private const int DefaultBulletDelay = 11;
        private const int DefaultHealth = 100;
        private const int DefaultBulletSpeed = 6;
        private const int DefaultArmor = 0;
        private readonly Vector2 resetPosition;
        // private readonly Texture2D bulleTexture = AssetsLoader.BulletTexture;

        private Rectangle healthRectangle;
        private List<Item> items;

        private int id;  // holds the player identifier

        private readonly string shipTextureFile;  // the ship Texture file name

        private int health;
        private int maxHealth;

        private int armor;

        private int level;

        private int bulletDamage;
        private float bulletDelay;
        private int bulletSpeed;

        private bool isColiding;
        public bool isAlive;

        private HUD hud;

        public Player(Vector2 position, InputHandler inputHandler, int id)
            : base(position)
        {
            this.Level = 1;
            this.Id = id;
            this.BulletDamage = DefaultBulletDamage;
            this.BulletDelay = DefaultBulletDelay;
            this.BulletSpeed = DefaultBulletSpeed;
            this.Speed = DefaultSpeed;
            this.MaxHealth = DefaultHealth;
            this.Health = DefaultHealth;
            this.Armor = DefaultArmor;
            this.Score = 0;
            this.items = new List<Item>();

            this.hud = new HUD(this);

            this.InputHandler = inputHandler;

            this.Position = position;
            this.IsAlive = true;

            if (this.id == 1)
            {
                this.Texture = TexturesManager.Player1Texture;
                //this.resetPosition = new Vector2(200, 600);
            }
            else
            {
                this.Texture = TexturesManager.Player2Texture;
               // this.resetPosition = new Vector2(1000, 600);
            }
        }

        public new Vector2 Position { get; set; }

        public InputHandler InputHandler  { get; set; }

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
            get
            {
                return this.bulletDamage;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Damage cannot be negative");
                }

                this.bulletDamage = value;
            }
        }

        public int BulletSpeed
        {
            get
            {
                return this.bulletSpeed;
            }
            set
            {
                this.bulletSpeed = value;
            }
        }

        public int Armor
        {
            get
            {
                return this.armor;
            }
            set
            {
                this.armor = value;
            }
        }

        public int Score { get; set; }

        public new int Health
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

        // Player update method
        public override void Update(GameTime gameTime)
        {
            // if player health is below zero set isAlive flag to false
            if (this.Health <= 0)
            {
                this.IsAlive = false;
            }
            this.hud.Update(this);
          
            //// if isAlive flag is false the player should be dead, so we reset it's Position depending on id
            //if (!this.IsAlive)
            //{
            //    this.Position = this.resetPosition;
            //    return;
            //}

            // Create bounding box around the player
            this.BoundingBox = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Texture.Width - 5,
                    this.Texture.Height - 5
                );
        }

        // Player draw method
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw player itself if he is alive
            if (this.isAlive)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);
                this.hud.Draw(spriteBatch);
            }
        }

        public override void ReactOnColission(IGameObject target)
        {
            string currentTargetType = target.GetType().Name;
            string currentTargetBaseType = target.GetType().BaseType.Name;

            if (currentTargetBaseType == "Item")
            {
                Item item = target as Item;

                this.Health = item.ItemHealth;
                this.BulletDamage += item.ItemDamage;
                this.BulletSpeed += item.ItemBulletSpeed;
                this.Armor += item.ItemArmor;
                this.Speed += item.ItemShipSpeed;
            }
            else if (currentTargetType == "Asteroid")
            {
                this.PlayerStatsReaction(target);

            }

            else if (currentTargetType == "Enemy")
            {
                this.PlayerStatsReaction(target);
            }

            else if (currentTargetType == "Boss")
            {
                this.PlayerStatsReaction(target);

            }

            else if (currentTargetType == "Bullet")
            {
                this.PlayerStatsReaction(target);
            }
        }


        private void PlayerStatsReaction(IGameObject target)
        {
            if (this.Armor <= target.Damage)
            {
                this.Health = -target.Damage + this.Armor;
                if (this.Armor > 0)
                {
                    this.Armor--;
                }
            }
        }

    }
}
