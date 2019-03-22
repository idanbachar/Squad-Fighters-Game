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
    public class Coin : Item
    {
        private Random Random;
        public CoinType ItemType;
        public int Points;

        public Coin(Vector2 itemPosition, CoinType itemType, int capacity) : base(itemPosition)
        {
            ItemType = itemType;
            Random = new Random();
            Points = capacity;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/items/Coins/" + ItemType);
        }

        public override void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public int GetPoints()
        {
            return Points;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public override string ToString()
        {
            return Points.ToString();
        }
    }
}
