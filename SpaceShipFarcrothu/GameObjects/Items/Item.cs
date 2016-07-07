namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Globals;

    public abstract class Item : FallingObject
    {
        private const int DefaultSpeed = 4;

        protected Item(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Speed = DefaultSpeed;
        }

        public int Health { get; set; }

        public virtual void Update()
        {
            this.BoundingBox = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Texture.Width,
                    this.Texture.Height
                );

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.MAIN_SCREEN_HEIGHT)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - 75);
                //??
                this.IsVisible = false;
            }
        }

        public override void ReactOnColission(GameObject target = null)
        {
            this.IsVisible = false;
        }
    }
}