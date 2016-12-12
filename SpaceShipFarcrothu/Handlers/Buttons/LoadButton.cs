using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.Utils.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Handlers.Buttons
{
    public class LoadButton
    {
        private readonly Vector2 buttonPosition;
        private readonly Vector2 textPosition;
        private readonly Texture2D texture;
        private readonly Rectangle buttonRectangle;
        private readonly string buttonText;
        private readonly int gameId;

        public LoadButton(Vector2 position, string buttonText, int gameId)
        {
            this.buttonPosition = position;
            this.texture = TexturesManager.ButtonTexture;
            this.textPosition = new Vector2(this.buttonPosition.X + 20, this.buttonPosition.Y + texture.Height / 4);
            this.buttonRectangle = new Rectangle((int)this.buttonPosition.X, (int)this.buttonPosition.Y, texture.Width, texture.Height);
            this.buttonText = buttonText + ": " + gameId;
            this.gameId = gameId;
        }


        public int GetGameId(MouseState mouse)
        {
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(buttonRectangle))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    return this.gameId;
                }
            }

            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(this.texture, this.buttonPosition);

            spriteBatch.DrawString(
                TexturesManager.PlayerScoreFont, buttonText, this.textPosition, new Color(10, 10, 10));

        }
    }
}
