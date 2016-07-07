namespace SpaceShipFartrothu.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Globals;

    public class Bullet : GameObject
    {
        private const int DefaultSpeed = 5;

        public static List<Bullet> Bullets = new List<Bullet>();

        // Enemy = 0; Player1 = 1; Player2 = 2; Boss = 3
        private int shooterId;
        
        public Bullet(Texture2D texture, Vector2 position, int shooterId, int bulletDamage)
            : base(texture, position)
        {
            this.Speed = DefaultSpeed;
            this.IsVisible = true;
            this.ShooterId = shooterId;
            this.Damage = bulletDamage;

            Bullets.Add(this);
        }

        public int ShooterId { get; set; }

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

            //Remove all not visible Bullets
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (!Bullets[i].IsVisible)
                {
                    Bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }

        public override void ReactOnColission(GameObject target = null)
        {
            this.IsVisible = false;
        }
    }
}
