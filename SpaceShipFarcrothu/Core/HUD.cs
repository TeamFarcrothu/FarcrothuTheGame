namespace SpaceShipFartrothu.Core
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class HUD
    {
        public int p1neededPointsToNextLevel;
        public int p2neededPointsToNextLevel;

        public bool p1hasEnoughToNextLevel = false;
        public bool p2hasEnoughToNextLevel = false;

        private int screenWidth, screenHeight;
        private SpriteFont playerScoreFont;
        private bool showHUD;

        public int playerLevel;
        public int player2Level;

        public int playerscore;
        public int player2score;

        private Vector2 playerLevelPos;
        private Vector2 player2LevelPos;

        private Vector2 playerScorePos;
        private Vector2 player2ScorePos;

        public HUD()
        {
            this.showHUD = true;
            this.screenWidth = 1366;
            this.screenHeight = 768;
            this.playerScoreFont = null;

            this.p1neededPointsToNextLevel = 150;
            this.p2neededPointsToNextLevel = 150;

            this.playerscore = 0;
            this.player2score = 0;

            this.playerLevel = 1;
            this.player2Level = 1;

            this.playerLevelPos = new Vector2(50, 75);
            this.player2LevelPos = new Vector2(1110, 75);

            this.playerScorePos = new Vector2(50, 20);
            this.player2ScorePos = new Vector2(1110, 20);
        }

        public void LoadContent(ContentManager Content)
        {
            this.playerScoreFont = Content.Load<SpriteFont>("georgia");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.showHUD)
            {
                spriteBatch.DrawString(this.playerScoreFont, "Player 1: " + this.playerscore, this.playerScorePos, Color.Red);
                spriteBatch.DrawString(this.playerScoreFont, "Player 2: " + this.player2score, this.player2ScorePos, Color.Red);

                spriteBatch.DrawString(this.playerScoreFont, "Level: " + this.playerLevel, this.playerLevelPos, Color.Red);
                spriteBatch.DrawString(this.playerScoreFont, "Level: " + this.player2Level, this.player2LevelPos, Color.Red);
            }
        }
    }
}