using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SquadFighters
{
    public class Shield : Item
    {
        public int Armor;
        public ShieldType ItemType;
        public ShieldBar[] ShieldBars;

        public Shield(Vector2 itemPosition, ShieldType itemType, int capacity) : base(itemPosition)
        {
            ItemType = itemType;
            ShieldBars = new ShieldBar[3];
            Armor = capacity;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/shields/" + GetShieldName());

            for(int i = 0; i < ShieldBars.Length; i++)
            {
                ShieldBars[i] = new ShieldBar(ItemType, new Vector2(i * 75, 50));
                ShieldBars[i].LoadContent(content);
            }
        }

        private string GetShieldName()
        {
            switch (ItemType)
            {
                case ShieldType.Shield_Level_1:
                case ShieldType.None:
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
            return Armor;
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
