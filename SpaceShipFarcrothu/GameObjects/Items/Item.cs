namespace SpaceShipFartrothu.GameObjects.Items
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Item
    {
        private const int DefaultSpeed = 4;

        private int health;
        private int damage;
        private int speed;
        private bool isVisible;

        private Rectangle boundingBox;
        private Texture2D texture;
        private Vector2 position;

        protected Item(Texture2D texture, Vector2 position, int health, int damage)
        {
            this.Texture = texture;
            this.Position = position;
            this.Speed = DefaultSpeed;
            this.Health = health;
            this.IsVisible = true;
            this.Damage = damage;
        }

        public int Health
        {
            get { return this.health; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Item health cannot be negative.");
                }

                this.health = value;
            }
        }

        public int Damage
        {
            get { return this.damage; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Item damage cannot be negative.");
                }

                this.damage = value;
            }
        }

        public int Speed
        {
            get { return this.speed; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Item speed cannot be negative");
                }

                this.speed = value;
            }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
        }

        public Rectangle BoundingBox
        {
            get { return this.boundingBox; }
            set { this.boundingBox = value; }
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public virtual void Update()
        {
            this.BoundingBox = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                this.Texture.Width,
                this.Texture.Height);

            this.position.Y += this.speed;

            if (this.position.Y >= 768)
            {
                this.position.Y = -75;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);
            }
        }
    }
}