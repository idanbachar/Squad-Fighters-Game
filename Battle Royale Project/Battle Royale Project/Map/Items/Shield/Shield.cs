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
    public class Shield : Item
    {
        public int Armor;
        public ShieldType ItemType;

        public Shield(Vector2 itemPosition, ShieldType itemType) : base(itemPosition)
        {
            ItemType = itemType;
            Armor = 50;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/shields/" + GetShieldName());
        }

        private string GetShieldName()
        {
            switch (ItemType)
            {
                case ShieldType.Shield_Level_1:
                    return "shield_lv1";
                case ShieldType.Shield_Level_2:
                    return "shield_lv2";
                case ShieldType.Shield_Rare:
                    return "shield_rare";
                case ShieldType.Shield_Legendery:
                    return "shield_legendery";
                default:
                    return "shield_lv1";
            }
        }

        public override void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public int GetArmor()
        {
            return new Random().Next(15, 43);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public override string ToString()
        {
            return Armor.ToString();
        }
    }
}
