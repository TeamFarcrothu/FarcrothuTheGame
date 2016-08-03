namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using Interfaces;
    using Utils.Assets;
    using Utils.Globals;

    public class Asteroid : EnemyEntity, IAsteroid
    {
        private const int DefaultSpeed = 4;
        private const int DefaultDamage = 10;
        private const int DefaultScorePoints = 10;
        private const string Type = nameof(Asteroid);

        private static Random random = new Random();

        // Rotation is not working at the moment in order to get proper collision
        // public Vector2 Origin;
        //public float RotationAngle;
        //public float RandomX;
        //public float RandomY;

        public Asteroid(Vector2 position)
            : base(position)
        {
            this.Texture = TexturesManager.AsteroidTexture;
            this.Speed = DefaultSpeed;
            this.Damage = DefaultDamage;
            this.ScorePoints = DefaultScorePoints;
            //    // Rotation is not working at the moment in order to get proper collision
            //    //this.Origin.X = Texture.Width / 2;
            //    //this.Origin.Y = Texture.Height / 2;
        }

        public override void Update(GameTime gameTime)
        {
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.MAIN_SCREEN_HEIGHT)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - 75);
                this.IsVisible = false;
            }

            // Rotation is not working at the moment in order to get proper collision

            //float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            //RotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //RotationAngle %= circle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);

                // Rotation is not working at the moment in order to get proper collision
                /*spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.White,
                    RotationAngle,
                    Origin,
                    1.0f,
                    SpriteEffects.None,
                    0f);*/
            }
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            //explosions.Add(new Explosion(this.Position));
            this.IsVisible = false;

            //TODO

            // Sound
        }
    }
}