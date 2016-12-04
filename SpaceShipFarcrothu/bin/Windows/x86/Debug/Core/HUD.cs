namespace SpaceShipFartrothu.Core
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Interfaces;
    using Utils.Assets;

    public class HUD : IHUD
    {
        private int healthBarSize;
        private int playerScore;
        private int playerHealth;
        private int playerLevel;
        private int playerDamage;
        private int playerSpeed;
        private int playerBulletSpeed;
        private int playerArmor;
        private int playerId;
        private Vector2 playerHudPosition;

        public HUD(IPlayer player)
        {
            this.HealthBarSize = player.MaxHealth;
            this.PlayerScore = player.Score;
            this.PlayerHealth = player.Health;
            this.PlayerLevel = player.Level;
            this.PlayerDamage = player.BulletDamage;
            this.PlayerId = player.Id;
            this.PlayerHudPosition = HudPosition();
            this.PlayerBulletSpeed = player.BulletSpeed;
            this.PlayerArmor = player.Armor;
            this.PlayerSpeed = player.Speed;
        }

        public int HealthBarSize
        {
            get { return this.healthBarSize; }
            set { this.healthBarSize = value; }
        }

        public int PlayerScore
        {
            get { return this.playerScore; }
            set { this.playerScore = value; }
        }

        public int PlayerHealth
        {
            get { return this.playerHealth; }
            set { this.playerHealth = value; }
        }

        public int PlayerLevel
        {
            get { return this.playerLevel; }
            set { this.playerLevel = value; }
        }

        public int PlayerId
        {
            get { return this.playerId; }
            private set { this.playerId = value; }
        }

        private Vector2 PlayerHudPosition
        {
            get { return this.playerHudPosition; }
            set { this.playerHudPosition = value; }
        }

        public int PlayerDamage
        {
            get { return this.playerDamage; }
            set { this.playerDamage = value; }
        }

        public int PlayerBulletSpeed
        {
            get { return this.playerBulletSpeed; }
            set { this.playerBulletSpeed = value; }
        }

        public int PlayerArmor
        {
            get { return this.playerArmor; }
            set { this.playerArmor = value; }
        }

        public int PlayerSpeed
        {
            get { return this.playerSpeed; }
            set { this.playerSpeed = value; }
        }

        public void Update(IPlayer player)
        {
            this.HealthBarSize = player.MaxHealth;
            this.PlayerScore = player.Score;
            this.PlayerHealth = player.Health;
            this.PlayerLevel = player.Level;
            this.PlayerDamage = player.BulletDamage;
            this.PlayerBulletSpeed = player.BulletSpeed;
            this.PlayerArmor = player.Armor;
            this.PlayerSpeed = player.Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Player {this.PlayerId}",
                this.PlayerHudPosition,
                Color.LightGreen);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Level: {this.PlayerLevel}",
                new Vector2(this.PlayerHudPosition.X, this.PlayerHudPosition.Y + 20),
                Color.LightGreen);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Score: {this.PlayerScore}",
                new Vector2(this.PlayerHudPosition.X, this.PlayerHudPosition.Y + 40),
                Color.LightGreen);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Health: {this.PlayerHealth} / {this.HealthBarSize}",
                new Vector2(this.PlayerHudPosition.X, this.PlayerHudPosition.Y + 60),
                Color.LightGreen);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Bullet Speed: {this.PlayerBulletSpeed}",
                new Vector2(this.PlayerHudPosition.X, this.playerHudPosition.Y + 80),
                Color.LightSkyBlue);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Damage: {this.PlayerDamage}",
                new Vector2(this.PlayerHudPosition.X, this.playerHudPosition.Y + 100),
                Color.LightSkyBlue);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Ship Speed: {this.PlayerSpeed}",
                new Vector2(this.PlayerHudPosition.X, this.playerHudPosition.Y + 120),
                Color.LightSkyBlue);
            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont,
                $"Ship Armor: {this.PlayerArmor}",
                new Vector2(this.PlayerHudPosition.X, this.playerHudPosition.Y + 140),
                Color.LightSkyBlue);
        }
        private Vector2 HudPosition()
        {
            if (this.playerId == 1)
            {
                return new Vector2(50, 50);
            }
            else
            {
                return new Vector2(1110, 50);
            }
        }
    }
}