namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Interfaces;
    using Microsoft.Xna.Framework.Content;
    using SpaceShipFartrothu.Utils.Globals;

    public class Bullet : GameObject, IBullet
    {
        private const int DefaultSpeed = 6;
        

        // Enemy = 0; Player1 = 1; Player2 = 2; Boss = 3
        private int shooterId;
        
        public Bullet(Texture2D texture, Vector2 position, int shooterId, int bulletDamage)
            : base(texture, position)
        {
            this.Speed = DefaultSpeed;
            this.IsVisible = true;
            this.ShooterId = shooterId;
            this.Damage = bulletDamage;
        }

        public int ShooterId { get; set; }
        //public Texture2D BulleTexture { get; set; }

        //public static void LoadContent(ContentManager content)
        //{
        //    bulleTexture = content.Load<Texture2D>("bullet");
        //}


        public override void Update(GameTime gameTime)
        {
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);

            //       Enemy id               "Boss" id 
            if (this.ShooterId == 0 || this.ShooterId == 3)
            {
                //Check if enemy Bullets go out of botoom screen boundary and set it to not visible
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

                if (this.Position.Y + this.Speed >= Globals.MAIN_SCREEN_HEIGHT)
                {
                    this.IsVisible = false;
                }
            }
            if (this.ShooterId == 1 || this.ShooterId == 2)
            {
                //Check if player Bullets go out of top screen boundary and set it to not visible
                this.Position = new Vector2(this.Position.X, this.Position.Y - this.Speed);

                if (this.Position.Y <= 0)
                {
                    this.IsVisible = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            this.IsVisible = false;
        }
    }
}
