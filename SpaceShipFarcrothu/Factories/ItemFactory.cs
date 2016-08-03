using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShipFartrothu.GameObjects.Items;
using SpaceShipFartrothu.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Factories
{
    public static class ItemFactory
    {
        public static void CreateItems(IList<IGameObject> items, Random random, IList<Texture2D> itemTextures, Vector2 potion)
        {
            int randomNumber = random.Next(0, 25);

            if (items.Count < 1)
            {
                if (randomNumber > 9 && randomNumber <= 12)
                {
                    items.Add(new BulletSpeedItem(itemTextures[3], potion));
                }
                if (randomNumber > 6 && randomNumber <= 9)
                {
                    items.Add(new ArmorItem(itemTextures[2], potion));
                }
                if (randomNumber > 3 && randomNumber <= 6)
                {
                    items.Add(new DamageItem(itemTextures[1], potion));
                }
                if (randomNumber <= 3)
                {
                    items.Add(new HealthItem(itemTextures[0], potion));
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].IsVisible)
                {
                    items.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
