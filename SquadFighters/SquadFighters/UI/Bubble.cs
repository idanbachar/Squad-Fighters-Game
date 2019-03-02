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
    public class Bubble
    {
        public Texture2D Texture;
        public Vector2 Position;
        public bool Visible;

        public Bubble(Vector2 position)
        {
            Position = new Vector2(position.X, position.Y);
            Visible = true;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/HUD/bubble");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
