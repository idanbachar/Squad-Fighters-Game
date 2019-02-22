using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Battle_Royale_Project
{
    public class Helmet : Item
    {
        public int Heal;
        public ItemType ItemType;

        public Helmet(Vector2 itemPosition, ItemType itemType) : base(itemPosition)
        {
            ItemType = itemType;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/helmets/" + GetHelmetName());
        }

        private string GetHelmetName()
        {
            switch (ItemType)
            {
                case ItemType.Shield_Level_1:
                    return "helmet_lv1";
                case ItemType.Shield_Level_2:
                    return "helmet_lv2";
                case ItemType.Shield_Rare:
                    return "helmet_rare";
                case ItemType.Shield_Legendery:
                    return "helmet_legendery";
                default:
                    return "helmet_lv1";
            }
        }

        public override void Update()
        {

        }

        public int GetArmor()
        {
            return new Random().Next(15, 43);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
