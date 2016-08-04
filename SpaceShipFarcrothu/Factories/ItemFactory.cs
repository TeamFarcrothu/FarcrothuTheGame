namespace SpaceShipFartrothu.Factories
{
    using System;
    using System.Collections.Generic;
    using GameObjects.Items;
    using Interfaces;

    public static class ItemFactory
    {
        public static void CreateItems(IRepository<IItem> items, List<IGameObject> entityList, Random random)
        {
            int randomNumber = random.Next(0, 36);

            if (items.GetCount() < 3)
            {
                foreach (var entity in entityList)
                {
                    if (!entity.IsVisible)
                    {
                        if (randomNumber > 8 && randomNumber <= 10)
                        {
                            items.AddEntity(new ShipSpeedItem(entity.Position));
                        }
                        if (randomNumber > 6 && randomNumber <= 8)
                        {
                            items.AddEntity(new BulletSpeedItem(entity.Position));
                        }
                        if (randomNumber > 4 && randomNumber <= 6)
                        {
                            items.AddEntity(new ArmorItem(entity.Position));
                        }
                        if (randomNumber > 2 && randomNumber <= 4)
                        {
                            items.AddEntity(new DamageItem(entity.Position));
                        }
                        if (randomNumber <= 2)
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
