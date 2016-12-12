using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.Handlers.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Factories
{
    public class LoadButtonFactory
    {
        private ICollection<LoadButton> loadButtons;
        private const int VerticalStep = 63;
        private Vector2 buttonPosition;
        private Vector2 originalPosition;
        private const string text = "Game";
        public LoadButtonFactory(Vector2 firstButtonPosition)
        {
            this.loadButtons = new List<LoadButton>();
            this.buttonPosition = firstButtonPosition;
            this.originalPosition = this.buttonPosition;
        }

        public void CreateButtons(ICollection<int> gameIds)
        {
            int counter = 1;

            foreach (var gameId in gameIds)
            {
                buttonPosition = new Vector2(
                    this.originalPosition.X, this.originalPosition.Y + (VerticalStep * counter));
                loadButtons.Add(new LoadButton(buttonPosition, text, gameId));
                counter++;
            }
        }

        public int ReturnGameId(MouseState mouse)
        {
            foreach (var button in loadButtons)
            {
                if (button.GetGameId(mouse) != 0)
                {
                    return button.GetGameId(mouse);
                }
            }
            return 0;
        }

        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (var buton in loadButtons)
            {
                buton.Draw(spriteBatch);
            }
        }

        public void RemoveAllButtons()
        {
            this.loadButtons.Clear();
        }
    }
}

