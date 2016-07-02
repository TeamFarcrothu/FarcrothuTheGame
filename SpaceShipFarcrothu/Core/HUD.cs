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
            showHUD = true;
            screenWidth = 1366;
            screenHeight = 768;
            playerScoreFont = null;

            playerscore = 0;
            player2score = 0;
            playerScorePos = new Vector2(50, 20);
            player2ScorePos = new Vector2(1110, 20);
        }

        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("georgia");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showHUD)
            {
                spriteBatch.DrawString(playerScoreFont, "Player 1: " + playerscore, playerScorePos, Color.Red);
                spriteBatch.DrawString(playerScoreFont, "Player 2: " + player2score, player2ScorePos, Color.Red);
            }
        }
    }
}