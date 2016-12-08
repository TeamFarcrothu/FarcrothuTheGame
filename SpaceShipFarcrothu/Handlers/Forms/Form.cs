using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.Utils.Assets;
using SpaceShipFartrothu.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Handlers.Forms
{
    public class Form
    {
        private const int MaxLength = 17;
        private readonly Vector2 formPosition;
        private readonly Vector2 textPosition;
        private readonly Vector2 titlePosition;
        private readonly Texture2D texture;
        private readonly Rectangle textRectangle;
        private readonly string title;
        private readonly bool showSymbols;
        private string formText;
        private Keys previousPressedKey;
        private Keys currentPressedKey;

        public Form(Vector2 position, string title, bool showSymbols)
        {
            this.formPosition = position;
            this.texture = TexturesManager.FormTexture;
            this.titlePosition = new Vector2(this.formPosition.X + 20, this.formPosition.Y + texture.Height / 6);
            this.textPosition = new Vector2(this.formPosition.X + 20, this.formPosition.Y + texture.Height / 2);
            this.textRectangle = new Rectangle((int)this.formPosition.X, (int)this.formPosition.Y, texture.Width, texture.Height);
            this.formText = string.Empty;
            this.title = title;
            this.showSymbols = showSymbols;
        }


        public void Update(MouseState mouse)
        {
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(textRectangle))
            {
                KeyboardState keyState = Keyboard.GetState();
                Keys[] pressedKeys = keyState.GetPressedKeys();

                foreach (var key in pressedKeys)
                {
                    currentPressedKey = key;
                }

                if (currentPressedKey != previousPressedKey && formText.Length < MaxLength)
                {
                    this.OnKeyDown(this.currentPressedKey);
                    this.previousPressedKey = this.currentPressedKey;
                }

                if (keyState.IsKeyDown(Keys.Back) && formText != string.Empty)
                {
                    formText = formText.Remove(formText.Length - 1, 1);
                }
            }
        }

        private void OnKeyDown(Keys key)
        {
            if (key == Keys.Back && this.formText.Length > 0)
            {
                this.formText = this.formText.Remove(this.formText.Length - 1);
            }
            else if (key == (Keys.Space))
            {
                formText += " ";
                return;
            }
            else
            {
                formText += key.ToString();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(this.texture, this.formPosition);

            spriteBatch.DrawString(
    TexturesManager.PlayerScoreFont, this.title,
        this.titlePosition, new Color(10, 10, 10));

            if (showSymbols)
            {
                spriteBatch.DrawString(
                    TexturesManager.PlayerScoreFont, this.formText,
                        this.textPosition, new Color(10, 10, 10));
            }
            else
            {
                spriteBatch.DrawString(
                    TexturesManager.PlayerScoreFont, new string('*', this.formText.Length),
                        this.textPosition, new Color(10, 10, 10));
            }


        }

        public void Erase()
        {
            this.formText = string.Empty;
        }

        public override string ToString()
        {
            return this.title.Replace(" ", "") + " " + this.formText;
        }
    }
}
