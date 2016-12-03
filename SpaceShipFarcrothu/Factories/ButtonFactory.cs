using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.Handlers.Buttons;
using SpaceShipFartrothu.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Factories
{
    public class ButtonFactory
    {
        private ICollection<Button> buttons;

        public ButtonFactory()
        {
            this.buttons = new List<Button>();
        }

        public void CreateButton(Vector2 buttonPosition, State returnState, string text)
        {
            buttons.Add(new Button(buttonPosition, returnState, text));
        }

        public State ReturnButtonState(MouseState mouse, State currentState)
        {
            foreach (var button in buttons)
            {
                if (button.GetGameState(mouse, currentState) != currentState)
                {
                    return button.GetGameState(mouse, currentState);
                }
            }
            return currentState;
        }

        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (var buton in buttons)
            {
                buton.Draw(spriteBatch);
            }
        }

        public void RemoveAllButtons()
        {
            this.buttons.Clear();
        }
    }
}
