namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Interfaces;
    using Utils.Assets;

    public class Boss : EnemyEntity
    {
        private const int DefaultHealth = 200;
        private const int DefaultSpeed = 2;
        private const int DefaultBulletDelay = 10;
        private const int DefaultSideBulletDelay = 70;
        private const int DefaultBulletDamange = 15;

        private const int ScreenWidth = 1181;
        //private int ScreenWidth = Globals.MAIN_SCREEN_WIDTH - GameEngine.bossTexture.Width;

        public Rectangle healthRectangle;
        public Texture2D healthTexture;
        public Vector2 healthBarPosition;

        private int bulletDelay;
        private int sideBulletDelay;
        private bool isAtTheRightBorder;

        private static Boss instance;
        //Singletone stuff
        public static Boss Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Boss(new Vector2(501, 10));
                }
                return instance;
            }
        }

        private Boss(Vector2 position)
            : base(position)
        {
            this.Texture = TexturesManager.BossTexture;

            this.Health = DefaultHealth;
            this.Speed = DefaultSpeed;
            this.isAtTheRightBorder = true;
            this.healthBarPosition = this.Position;
            this.BulletDelay = DefaultBulletDelay;
            this.BulletDamage = DefaultBulletDamange;
        }

        public int Health { get; set; }


        public override void Update(GameTime gameTime)
        {
            if (this.Health <= 0)
            {
                this.IsVisible = false;
            }

            this.BoundingBox = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                this.Texture.Width,
                this.Texture.Height);

            // Showing the Boss slowly from top of the screen
            if (this.Position.Y < 31)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);
                this.healthBarPosition = new Vector2(this.Position.X + 140, this.Position.Y - 30);
                return;
            }

            // Moving  the Boss automaticaly left and right
            if (this.Position.X <= ScreenWidth && this.isAtTheRightBorder)
            {
                if (this.Position.X == ScreenWidth)
                {
                    this.isAtTheRightBorder = false;
                }
                this.Position = new Vector2(this.Position.X + this.Speed, this.Position.Y);
                this.healthBarPosition = new Vector2(this.Position.X + 140, this.Position.Y - 30);
            }
            else if (this.Position.X >= -171 && !this.isAtTheRightBorder)
            {
                if (this.Position.X == -171)
                {
                    this.isAtTheRightBorder = true;
                }

                this.Position = new Vector2(this.Position.X - this.Speed, this.Position.Y);
                this.healthBarPosition = new Vector2(this.Position.X + 140, this.Position.Y - 30);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);

            this.healthRectangle = new Rectangle(
                (int)this.healthBarPosition.X,
                (int)this.healthBarPosition.Y, this.Health,
                20);

            spriteBatch.Draw(TexturesManager.HealthTexture, this.healthRectangle, Color.White);
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            this.Health -= (target as IPlayer).BulletDamage;
        }

    }
}