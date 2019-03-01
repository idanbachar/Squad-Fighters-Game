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
    public class Water
    {
        public Vector2 Position;
        public Rectangle Rectangle;
        public Texture2D Texture;
        public WaterShape WaterShape;

        public Water(Vector2 position, WaterShape waterShape)
        {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            WaterShape = waterShape;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/map/world/water/water_" + WaterShape.ToString());
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }  
    }
}
