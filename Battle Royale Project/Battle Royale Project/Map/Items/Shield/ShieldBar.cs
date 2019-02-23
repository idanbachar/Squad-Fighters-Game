using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Royale_Project
{
    public class ShieldBar
    {
        public ShieldType ShieldType;
        public Texture2D Texture;
        public Vector2 Position;

        public ShieldBar(ShieldType shieldType, Vector2 position)
        {
            ShieldType = shieldType;
            Position = new Vector2(position.X, position.Y);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/player/armor_bars/" + GetArmorImageName());
        }

        public string GetArmorImageName()
        {
            switch (ShieldType)
            {
                case ShieldType.Shield_Level_1:
                    return "armor_lv1";
                case ShieldType.Shield_Level_2:
                    return "armor_lv2";
                case ShieldType.Shield_Rare:
                    return "armor_rare";
                case ShieldType.Shield_Legendery:
                    return "armor_legendery";
            }

            return string.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
