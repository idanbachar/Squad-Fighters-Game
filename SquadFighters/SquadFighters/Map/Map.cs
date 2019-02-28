using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class Map
    {
        public static Rectangle Rectangle;
        public Dictionary<string, Item> Items;
        private ContentManager content;
        private Random Random;

        public Map(Rectangle mapRectangle, ContentManager contentManager)
        {
            content = contentManager;

            Rectangle = new Rectangle(mapRectangle.X,
                                      mapRectangle.Y,
                                      mapRectangle.Width,
                                      mapRectangle.Height);

            Random = new Random();
            Items = new Dictionary<string, Item>();

        }

        public void AddItem(ItemCategory itemCategory, int itemType, float itemX, float itemY, int itemCapacity, string itemKey)
        {
            Item item = new GunAmmo(new Vector2(0,0), AmmoType.Bullet, 20);

            switch (itemCategory)
            {
                case ItemCategory.Ammo:
                    item = new GunAmmo(new Vector2(itemX, itemY), (AmmoType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Food:
                    item = new Food(new Vector2(itemX, itemY), (FoodType) itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Shield:
                    item = new Shield(new Vector2(itemX, itemY), (ShieldType) itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Helmet:
                    item = new Helmet(new Vector2(itemX, itemY), (HelmetType) itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
            }

            Items.Add(itemKey,item);
        }

        public Vector2 GeneratePosition()
        {
            return new Vector2(Random.Next(200, Rectangle.Width - 200), Random.Next(200, Rectangle.Height - 200));
        }
    }
}
