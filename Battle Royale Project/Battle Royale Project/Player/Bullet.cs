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
    public class Bullet
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction;

        public Bullet(Vector2 position, Vector2 direction)
        {
            Position = new Vector2(position.X, position.Y);
            Direction = new Vector2(direction.X, direction.Y);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/player/bullets/bullet");
        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            Position += Direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
