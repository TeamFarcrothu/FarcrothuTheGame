using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu.Handlers.Buttons;
using SpaceShipFartrothu.Handlers.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Factories
{
    public class FormFactory
    {
        private ICollection<Form> forms;
        
        public FormFactory()
        {
            this.forms = new List<Form>();
        }

        public void CreateForm(Vector2 formPosition, string title, bool showSymbols)
        {
            forms.Add(new Form(formPosition, title, showSymbols));
        }

        public void UpdateForms(MouseState mouse)
        {
            foreach (var form in forms)
            {
                form.Update(mouse);
            }
        }

        public void DrawForms(SpriteBatch spriteBatch)
        {
            foreach (var form in forms)
            {
                form.Draw(spriteBatch);
            }
        }

        public void EraseForms()
        {
            foreach (var form in forms)
            {
                form.Erase();
            }
        }

        public ICollection<string> GetAllForms()
        {
            ICollection<string> formStrings = new List<string>();

            foreach (var form in this.forms)
            {
                formStrings.Add(form.ToString());
            }

            return formStrings;
        }

        public void RemoveAllForms()
        {
            this.forms.Clear();
        }
    }
}
