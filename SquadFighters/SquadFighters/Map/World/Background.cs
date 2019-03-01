using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class Background
    {
        public Vector2 Position;
        public Texture2D Texture;

        public Background(Vector2 position)
        {
            Position = new Vector2(position.X, position.Y);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/map/background");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
