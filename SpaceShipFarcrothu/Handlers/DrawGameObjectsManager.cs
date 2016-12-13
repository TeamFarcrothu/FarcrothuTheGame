using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SpaceShipFartrothu.Data;
using SpaceShipFartrothu.GameObjects;

namespace SpaceShipFartrothu.Handlers
{
    public class DrawGameObjectsManager
    {
        internal void DrawAllGameObjects(GameDatabase db, SpriteBatch spriteBatch, StarField starfield)
        {
            starfield.Draw(spriteBatch);

            db.Enemies.GetAll().ForEach(e => e.Draw(spriteBatch));
            db.Bullets.GetAll().ForEach(b => b.Draw(spriteBatch));
            db.Asteroids.GetAll().ForEach(a => a.Draw(spriteBatch));
            db.Explosions.GetAll().ForEach(e => e.Draw(spriteBatch));

            foreach (var item in db.Items.GetAll())
            {
                item.Draw(spriteBatch);
            }
            db.Players.GetAll().ForEach(p => p.Draw(spriteBatch));
        }
    }
}
