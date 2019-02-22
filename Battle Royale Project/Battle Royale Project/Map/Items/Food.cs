﻿using System;
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
    public class Food : Item
    {
        public int Heal;
        public ItemType ItemType;

        public Food(Vector2 itemPosition, ItemType itemType) : base(itemPosition)
        {
            ItemType = itemType;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/food/" + ItemType);
        }

        public override void Update()
        {

        }

        public int GetHealth()
        {
            return new Random().Next(15, 43);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
