using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShipFartrothu.GameObjects.Items;
using SpaceShipFartrothu.Interfaces;
using System;
using System.Collections.Generic;
namespace SpaceShipFartrothu.Factories
{
    public static class ItemFactory
    {
        public static void CreateItems(IRepository<IItem> items, List<IGameObject> entityList, Random random)
        {
            int randomNumber = random.Next(0, 13);

            if (items.GetCount() < 30)
            {
                foreach (var entity in entityList)
                {
                    if (!entity.IsVisible)
                    {
                        if (randomNumber > 9 && randomNumber <= 12)
                        {
                            items.AddEntity(new BulletSpeedItem(entity.Position));
                        }
                        if (randomNumber > 6 && randomNumber <= 9)
                        {
                            items.AddEntity(new ArmorItem(entity.Position));
                        }
                        if (randomNumber > 3 && randomNumber <= 6)
                        {
                            items.AddEntity(new DamageItem(entity.Position));
                        }
                        if (randomNumber <= 3)
                        {
                            items.AddEntity(new HealthItem(entity.Position));
                        }
                    }
                }
            }

            for (int i = 0; i < items.GetCount(); i++)
            {
                if (!items.GetAll()[i].IsVisible)
                {
                    items.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
