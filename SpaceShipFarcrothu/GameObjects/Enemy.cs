namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Items;
    using Interfaces;
    using Utils.Assets;
    using Utils.Globals;

    public class Enemy : EnemyEntity
    {
        private const int DefaultScorePoints = 15;
        private const int DefaultBulletDamage = 10;
        private const int DefaultDamage = 10;
        private const int DefaultHealth = 20;
        private const int DefaultSpeed = 2;
        private const int DefaultShooterId = 0;

        private static Random random = new Random();

        private int health, bulletDelay, currentDifficultyLevel;

        private int bulletDamage;

        public Enemy(Vector2 position)
            : base(position)
        {
            this.Texture = TexturesManager.EnemyTexture;

            this.Health = DefaultHealth;
            this.BulletDelay = Globals.DefaultEnemyBulletDelay;
            this.Speed = DefaultSpeed;
            this.BulletDamage = DefaultBulletDamage;
            this.Damage = DefaultDamage;
            this.ScorePoints = DefaultScorePoints;
            this.currentDifficultyLevel = 1;
            this.ShooterId = DefaultShooterId;
        }

        public int Health { get; set; }

        public void AddScore(int score)
        {
            if (score <= 0)
            {
                throw new ArgumentException("Score must be positive.");
            }

            int newScore = this.ScorePoints + score;
            this.ScorePoints = newScore;
        }

        public void Die()
        {
            this.Health = 0;
            this.IsVisible = false;
            this.Position = new Vector2(0, -300);
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateEnemyMovement();
        }

        private void UpdateEnemyMovement()
        {
            //enemy BoundingBox
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width,
                this.Texture.Height);

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.MAIN_SCREEN_HEIGHT)
            {
                this.IsVisible = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            this.Health -= (target as Player).BulletDamage;
            if (Health <= 0)
            {
                this.IsVisible = false;
            }
        }

    }
}