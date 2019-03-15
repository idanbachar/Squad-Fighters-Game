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
    public class Button
    {
        private Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;
        public ButtonType ButtonType;

        public Button(Vector2 position, ButtonType buttonType)
        {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            ButtonType = buttonType;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/main menu/buttons/" + ButtonType.ToString());
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch, bool isMouseOver)
        {
            spriteBatch.Draw(Texture, Position, !isMouseOver ? Color.White : Color.DarkGray);
        }
    }
}
