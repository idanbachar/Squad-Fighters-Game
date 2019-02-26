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
        public List<Item> Items;
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
            Items = new List<Item>();

        }

        public void GenerateItem(ItemCategory itemCategory, int type, float itemX, float itemY)
        {
            Item item = new GunAmmo(new Vector2(0,0), AmmoType.Bullet);

            switch (itemCategory)
            {
                case ItemCategory.Ammo:
                    item = new GunAmmo(new Vector2(itemX, itemY), (AmmoType)type);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Food:
                    item = new Food(new Vector2(itemX, itemY), (FoodType) type);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Shield:
                    item = new Shield(new Vector2(itemX, itemY), (ShieldType) type);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Helmet:
                    item = new Helmet(new Vector2(itemX, itemY), (HelmetType) type);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
            }

            Items.Add(item);
        }

        public Vector2 GeneratePosition()
        {
            return new Vector2(Random.Next(200, Rectangle.Width - 200), Random.Next(200, Rectangle.Height - 200));
        }
    }
}
