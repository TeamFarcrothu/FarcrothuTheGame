namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Globals;
    using Effects;
    using Core;
    using Items;

    public class Enemy : EnemyEntity
    {
        private const int DefaultScorePoints = 15;
        private const int DefaultHealth = 5;
        //private Texture2D bulletTexture;
        private const int shooterId = 0;
        private static Random random = new Random();

        private int health, bulletDelay, currentDifficultyLevel;

        private int bulletDamage;

        public static Texture2D enemyTexture;

        public static List<GameObject> Enemies = new List<GameObject>();

        public Enemy(Texture2D texture, Vector2 position, Texture2D bulleTexture)
            : base(enemyTexture, position)
        {
            this.Texture = enemyTexture;
            this.BulletTexture = bulleTexture;
            this.Health = DefaultHealth;
            this.BulletDelay = 40;
            this.Speed = 2;
            this.IsVisible = true;
            this.BulletDamage = 5;
            this.Damage = 10;
            this.ScorePoints = DefaultScorePoints;
            this.currentDifficultyLevel = 1;

            Enemies.Add(this);
        }

        public int BulletDamage { get; set; }

        public int BulletDelay { get; set; }

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

        //   TODO :  must make it work better
        public Texture2D BulletTexture { get; set; }

        //// TODO :  Must fix it
        //public void LoadContent(ContentManager content)
        //{
        //    this.bulletTexture = content.Load<Texture2D>("bullet");
        //}

        public override void Update(GameTime gameTime)
        {
            UpdateEnemyMovement();

            ClearNotVisibleEnemies();

            if (Enemies.Count > 0)
                this.EnemyShoot();
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

        private static void ClearNotVisibleEnemies()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].IsVisible)
                {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void LoadEnemies()
        {
            var newRand = new Random();

            int randomX = random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = random.Next(-700, -50) + newRand.Next(0, 100);

            if (Enemies.Count < 5)
            {
                Enemies.Add(new Enemy(enemyTexture, new Vector2(randomX, randomY), GameEngine.bulletTexture));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }

        public void EnemyShoot()
        {
            if (this.BulletDelay >= 0)
            {
                this.BulletDelay--;
            }

            if (this.BulletDelay <= 0)
            {
                if (Bullet.Bullets.Where(b => b.ShooterId == shooterId).ToList().Count < 20)
                {
                    var newBulletPosition = new Vector2(this.Position.X + this.Texture.Width / 2 - this.BulletTexture.Width / 2, this.Position.Y + this.BulletTexture.Height);

                    Bullet newBullet = new Bullet(this.BulletTexture, newBulletPosition, shooterId, this.BulletDamage);
                    Bullet.Bullets.Add(newBullet);
                }

                if (this.BulletDelay == 0)
                {
                    this.BulletDelay = 40;
                }
            }
        }

        public override void ReactOnColission(GameObject target = null)
        {
            Explosion.Explosions.Add(new Explosion(this.Position));
            HealthItem.LoadItems(this.Position);

            this.IsVisible = false;

            //TODO: Sound

        }
    }
}