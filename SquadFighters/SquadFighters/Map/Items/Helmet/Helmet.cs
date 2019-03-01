﻿using System;
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
    public class Helmet : Item
    {
        public int Armor;
        public HelmetType ItemType;

        public Helmet(Vector2 itemPosition, HelmetType itemType, int capacity) : base(itemPosition)
        {
            ItemType = itemType;
            Armor = capacity;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/helmets/" + GetHelmetName());
        }

        private string GetHelmetName()
        {
            switch (ItemType)
            {
                case HelmetType.Helmet_Level_1:
                    return "helmet_lv1";
                case HelmetType.Helmet_Level_2:
                    return "helmet_lv2";
                case HelmetType.Helmet_Rare:
                    return "helmet_rare";
                case HelmetType.Helmet_Legendery:
                    return "helmet_legendery";
                default:
                    return "helmet_lv1";
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