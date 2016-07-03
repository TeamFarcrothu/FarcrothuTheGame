namespace SpaceShipFartrothu.Core
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class HUD
    {
        private int screenWidth, screenHeight;
        private SpriteFont playerScoreFont;
        private bool showHUD;

        public int playerscore;
        public int player2score;
        private Vector2 playerScorePos;
        private Vector2 player2ScorePos;

        public HUD()
        {
            this.showHUD = true;
            this.screenWidth = 1366;
            this.screenHeight = 768;
            this.playerScoreFont = null;

            this.playerscore = 0;
            this.player2score = 0;
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
            }
        }
    }
}