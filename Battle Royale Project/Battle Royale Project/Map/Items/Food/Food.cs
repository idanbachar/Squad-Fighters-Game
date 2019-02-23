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
    public class Food : Item
    {
        private Random Random;
        public FoodType ItemType;
        public int Heal;

        public Food(Vector2 itemPosition, FoodType itemType) : base(itemPosition)
        {
            ItemType = itemType;
            Random = new Random();
            Heal = GetHealth();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/food/" + ItemType);
        }

        public override void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public int GetHealth()
        {
            return Random.Next(15, 43);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public override string ToString()
        {
            return Heal.ToString();
        }
    }
}
