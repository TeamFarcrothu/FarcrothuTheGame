using SpaceShipFartrothu.GameObjects;

namespace SpaceShipFartrothu.Core
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;
    using SpaceShipFartrothu.Interfaces;
    using SpaceShipFartrothu.Utils.Globals;

    public class HUD
    {
        public int p1neededPointsToNextLevel;
        public int p2neededPointsToNextLevel;

        public bool p1hasEnoughToNextLevel = false;
        public bool p2hasEnoughToNextLevel = false;

        private int screenWidth, screenHeight;
        private SpriteFont playerScoreFont;
        private bool showHUD;

        public List<int> playersLevels; //***
        public List<int> playersScores; //***
        public List<int> playersHealth;
        public List<int> playersMaxHealth;
        public List<int> playersBulletDamage; //***
        public List<int> playersArmor;
        public List<int> playersSpeed;//***
        public List<int> playersBulletSpeed;//***

        private int playersCount;

        private Vector2 player1LevelPos;
        private Vector2 player2LevelPos;

        private Vector2 player1ScorePos;
        private Vector2 player2ScorePos;

        private Vector2 player1DamagePos;
        private Vector2 player2DamagePos;

        private Vector2 player1HealthPos;
        private Vector2 player2HealthPos;

        private Vector2 player1ArmorPos;
        private Vector2 player2ArmorPos;

        private Vector2 player1SpeedPos;
        private Vector2 player2SpeedPos;

        private Vector2 player1BulletSpeedPos;
        private Vector2 player2BulletSpeedPos;

        public HUD()
        {
            this.playersLevels = new List<int>();
            this.playersScores = new List<int>();
            this.playersHealth = new List<int>();
            this.playersMaxHealth = new List<int>();
            this.playersBulletDamage = new List<int>();
            this.playersArmor = new List<int>();
            this.playersSpeed = new List<int>();
            this.playersBulletSpeed = new List<int>();

            this.showHUD = true;
            this.screenWidth = Globals.MAIN_SCREEN_WIDTH;
            this.screenHeight = Globals.MAIN_SCREEN_HEIGHT;
            this.playerScoreFont = null;

            this.p1neededPointsToNextLevel = 150;
            this.p2neededPointsToNextLevel = 150;

            this.player1HealthPos = new Vector2(50, 50);
            this.player2HealthPos = new Vector2(1110, 50);

            this.player1LevelPos = new Vector2(50, 80);
            this.player2LevelPos = new Vector2(1110, 80);

            this.player1ScorePos = new Vector2(50, 20);
            this.player2ScorePos = new Vector2(1110, 20);

            this.player1DamagePos = new Vector2(50, 110);
            this.player2DamagePos = new Vector2(1110, 110);

            this.player1ArmorPos = new Vector2(50, 140);
            this.player2ArmorPos = new Vector2(1110, 140);

            this.player1SpeedPos = new Vector2(50, 170);
            this.player2SpeedPos = new Vector2(1110, 170);

            this.player1BulletSpeedPos = new Vector2(50, 200);
            this.player2BulletSpeedPos = new Vector2(1110, 200);

        }
        public void UpdatePlayersInfo(IList<IPlayer> players)//***
        {
            playersCount = players.Count;
            for (int i = 0; i < players.Count; i++)
            {
                IPlayer currentPlayer = players[i];
                playersLevels.Add(currentPlayer.Level);
                playersScores.Add(currentPlayer.Score);
                playersHealth.Add(currentPlayer.Health);
                playersMaxHealth.Add(currentPlayer.MaxHealth);
                playersBulletDamage.Add(currentPlayer.BulletDamage);
                playersArmor.Add(currentPlayer.Armor);
                playersSpeed.Add(currentPlayer.Speed);
                playersBulletSpeed.Add(currentPlayer.BulletSpeed);
            }
        }

        public void LoadContent(ContentManager content)
        {
            this.playerScoreFont = content.Load<SpriteFont>("georgia");
        }

        //public void Update(GameTime gameTime)
        //{
        //    foreach (Player player in Player.Players)
        //    {
        //        if (player.Id == 1)
        //            this.player1score = player.Score;
        //        else if (player.Id == 2)
        //            this.player2score = player.Score;
        //    }
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.showHUD)
            {
                if (playersCount >= 1)
                {
                    spriteBatch.DrawString(this.playerScoreFont, "Player 1: " + this.playersScores[0], this.player1ScorePos, Color.Red);
                    spriteBatch.DrawString(this.playerScoreFont, $"Health: {this.playersHealth[0]} / {this.playersMaxHealth[0]}", this.player1HealthPos, Color.Red);
                    spriteBatch.DrawString(this.playerScoreFont, "Level: " + this.playersLevels[0], this.player1LevelPos, Color.Blue);
                    spriteBatch.DrawString(this.playerScoreFont, "Damage: " + this.playersBulletDamage[0], this.player1DamagePos, Color.Green);
                    spriteBatch.DrawString(this.playerScoreFont, "Armor: " + this.playersArmor[0], this.player1ArmorPos, Color.White);
                    spriteBatch.DrawString(this.playerScoreFont, "Speed: " + this.playersSpeed[0], this.player1SpeedPos, Color.Orange);
                    spriteBatch.DrawString(this.playerScoreFont, "BulletSpeed: " + this.playersBulletSpeed[0], this.player1BulletSpeedPos, Color.Yellow);

                }
                if (playersCount == 2)
                {
                    spriteBatch.DrawString(this.playerScoreFont, "Player 2: " + this.playersScores[1], this.player2ScorePos, Color.Red);
                    spriteBatch.DrawString(this.playerScoreFont, $"Health: {this.playersHealth[1]} / {this.playersMaxHealth[1]}", this.player2HealthPos, Color.Red);
                    spriteBatch.DrawString(this.playerScoreFont, "Level: " + this.playersLevels[1], this.player2LevelPos, Color.Blue);
                    spriteBatch.DrawString(this.playerScoreFont, "Damage: " + this.playersBulletDamage[1], this.player2DamagePos, Color.Green);
                    spriteBatch.DrawString(this.playerScoreFont, "Armor: " + this.playersArmor[1], this.player2ArmorPos, Color.White);
                    spriteBatch.DrawString(this.playerScoreFont, "Speed: " + this.playersSpeed[1], this.player2SpeedPos, Color.Orange);
                    spriteBatch.DrawString(this.playerScoreFont, "BulletSpeed: " + this.playersBulletSpeed[1], this.player2BulletSpeedPos, Color.Yellow);
                }
                playersLevels.Clear();
                playersScores.Clear();
                playersHealth.Clear();
                playersMaxHealth.Clear();
                playersBulletDamage.Clear();
                playersArmor.Clear();
                playersSpeed.Clear();
                playersBulletSpeed.Clear();
            }
        }
    }
}