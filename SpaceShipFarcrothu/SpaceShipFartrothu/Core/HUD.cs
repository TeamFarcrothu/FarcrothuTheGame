namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class HUD
    {
        public int screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public bool showHUD;

        public int playerscore;
        public int player2score;
        public Vector2 playerScorePos;
        public Vector2 player2ScorePos;

        public HUD()
        {
            showHUD = true;
            screenWidth = 1366;
            screenHeight = 768;
            playerScoreFont = null;

            playerscore = 0;
            player2score = 0;
            playerScorePos = new Vector2(screenWidth / 4, 50);
            player2ScorePos = new Vector2(screenWidth / 2, 50);
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
