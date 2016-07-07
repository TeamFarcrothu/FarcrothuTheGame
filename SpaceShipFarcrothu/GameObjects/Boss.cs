namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using System.Linq;
    using Core;
    using Globals;

    public class Boss : EnemyEntity
    {
        private const int DefaultHealth = 200;
        private const int DefaultSpeed = 2;
        private const int DefaultBulletDelay = 10;
        private const int DefaultSideBulletDelay = 70;
        private const int ScreenWidth = 1181;
        //private int ScreenWidth = Globals.MAIN_SCREEN_WIDTH - GameEngine.bossTexture.Width;

        public Rectangle healthRectangle;
        public Texture2D healthTexture;
        public Vector2 healthBarPosition;

        //public int Health;
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
                    instance = new Boss(GameEngine.bossTexture, new Vector2(501, 10));
                }
                return instance;
            }
        }

        private Boss(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Health = DefaultHealth;
            this.Speed = DefaultSpeed;
            this.isAtTheRightBorder = true;
            this.healthBarPosition = this.Position;
            this.BulletDelay = DefaultBulletDelay;
            this.BulletDamage = 15;

            this.BulletTexture = GameEngine.bulletTexture;
            this.healthTexture = GameEngine.healthTexture;
        }

        public int Health { get; set; }

        public int BulletDelay { get; set; }

        public int BulletDamage { get; set; }

        public Texture2D BulletTexture { get; set; }

        public void LoadContent(ContentManager content)
        {
            //this.texture = content.Load<Texture2D>("space_Boss_Level_1");
            //this.bulletTexture = GameEngine.bossTexture;///content.Load<Texture2D>("bullet");
            /*this.healthTexture = GameEngine.healthTexture;*/ //content.Load<Texture2D>("healthbar");
            //this.Position = new Vector2(501, -600);
        }

        public override void Update(GameTime gameTime)
        {
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

            if (this.Health <= 0)
            {
                this.IsVisible = false;
            }

            this.BossShoot();

        }

        private void BossShoot()
        {
            if (this.BulletDelay >= 0)
            {
                this.BulletDelay--;
            }

            if (this.BulletDelay <= 0)
            {
                if (Bullet.Bullets.Where(b => b.ShooterId == 3).ToList().Count < 20)
                {
                    var newBulletPosition = new Vector2(
                        this.Position.X + this.Texture.Width / 2 - this.BulletTexture.Width / 2,
                        this.Position.Y + this.Texture.Height - 100);
                    Bullet newBullet = new Bullet(this.BulletTexture, newBulletPosition, 3, this.BulletDamage);
                    Bullet.Bullets.Add(newBullet);

                    var newLeftBulletPosition = new Vector2(
                       this.Position.X + this.Texture.Width / 2 - this.BulletTexture.Width / 2 - 130,
                        this.Position.Y + this.Texture.Height - 210);
                    Bullet newLeftBullet = new Bullet(this.BulletTexture, newLeftBulletPosition, 3, this.BulletDamage);
                    Bullet.Bullets.Add(newLeftBullet);

                    var newRightBulletPosition = new Vector2(
                       this.Position.X + this.Texture.Width / 2 - this.BulletTexture.Width / 2 + 130,
                        this.Position.Y + this.Texture.Height - 210);
                    Bullet newRightBullet = new Bullet(this.BulletTexture, newRightBulletPosition, 3, this.BulletDamage);
                    Bullet.Bullets.Add(newRightBullet);
                }

                if (this.BulletDelay == 0)
                {
                    this.BulletDelay = DefaultBulletDelay;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);

            this.healthRectangle = new Rectangle(
                (int)this.healthBarPosition.X,
                (int)this.healthBarPosition.Y, this.Health,
                20);

            spriteBatch.Draw(this.healthTexture, this.healthRectangle, Color.White);
        }

        public override void ReactOnColission(GameObject target = null)
        {
            foreach (Player player in Player.Players)
            {
                this.Health -= player.BulletDamage;
            }
        }

    }
}