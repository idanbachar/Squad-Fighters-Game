using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Royale_Project
{
    public class Map
    {
        public Random random;
        public Rectangle Rectangle;
        public List<Item> Items;
        private ContentManager content;

        public Map(Rectangle mapRectangle, ContentManager contentManager)
        {
            content = contentManager;

            Rectangle = new Rectangle(mapRectangle.X,
                                      mapRectangle.Y,
                                      mapRectangle.Width,
                                      mapRectangle.Height);


            random = new Random();
            Items = new List<Item>();

        }

        public void AddItem(ItemType itemToAdd)
        {
            Item item;

            switch (itemToAdd)
            {
                case ItemType.GunAmmo:
                    item = new GunAmmo(GeneratePosition(), 20);
                    item.LoadContent(content);
                    Items.Add(item);
                    break;
            }
        }

        public Vector2 GeneratePosition()
        {
            return new Vector2(random.Next(0, 3000), random.Next(0, 3000));
        }
    }
}
