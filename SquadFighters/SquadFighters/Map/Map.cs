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
        public Background Background;
        public Dictionary<string, Item> Items;
        public List<Water> WaterObjects;
        private Random Random;
        
        public Map(Rectangle mapRectangle)
        {
            Background = new Background(new Vector2(0, 0));

            Rectangle = new Rectangle(mapRectangle.X,
                                      mapRectangle.Y,
                                      mapRectangle.Width,
                                      mapRectangle.Height);

            Random = new Random();

            Items = new Dictionary<string, Item>();
            WaterObjects = new List<Water>();

        }

        public void LoadContent(ContentManager content)
        {
            Background.LoadContent(content);
            GenerateWaterObjects(content);
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

        public void GenerateWaterObjects(ContentManager content)
        {
            Water water1 = new Water(new Vector2(500, 600), WaterShape.Medium);
            water1.LoadContent(content);

            Water water2 = new Water(new Vector2(1200, 1200), WaterShape.Rectangle);
            water2.LoadContent(content);

            Water water3 = new Water(new Vector2(1000, 3000), WaterShape.Tall);
            water3.LoadContent(content);

            Water water4 = new Water(new Vector2(3500, 900), WaterShape.Tall);
            water4.LoadContent(content);

            Water water5 = new Water(new Vector2(2200, 4200), WaterShape.Rectangle);
            water5.LoadContent(content);


            WaterObjects.Add(water1);
            WaterObjects.Add(water2);
            WaterObjects.Add(water3);
            WaterObjects.Add(water4);
            WaterObjects.Add(water5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);

            foreach (Water water in WaterObjects)
                water.Draw(spriteBatch);
        }

        public Vector2 GeneratePosition()
        {
            return new Vector2(Random.Next(200, Rectangle.Width - 200), Random.Next(200, Rectangle.Height - 200));
        }
    }
}
