using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class HealthBar
    {
        public Rectangle Rectangle;
        public Vector2 Position;
        public Texture2D Texture;
        public int Health;

        public HealthBar(int health)
        {
            Health = health;
            Position = new Vector2(0, 0);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Health, 0);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/HUD/health_bar");
            Position = new Vector2(0, 0);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void SetHealth(int health)
        {
            Health = health;
            Rectangle.Width = Health;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
