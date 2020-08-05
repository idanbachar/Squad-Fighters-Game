using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Button {

        private Texture2D Texture; //טקסטורת כפתור
        public Vector2 Position; //מיקום כפתור
        public Rectangle Rectangle; //מלבן כפתור
        public ButtonType ButtonType; //סוג כפתור


        /// <summary>
        /// מקבל מיקום וסוג כפתור ומייצר כפתור
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buttonType"></param>
        public Button(Vector2 position, ButtonType buttonType) {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            ButtonType = buttonType;
        }

        /// <summary>
        /// טעינת כפתור
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/main menu/buttons/" + ButtonType.ToString());
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// ציור כפתור
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="isMouseOver"></param>
        public void Draw(SpriteBatch spriteBatch, bool isMouseOver) {
            spriteBatch.Draw(Texture, Position, !isMouseOver ? Color.White : Color.DarkGray);
        }
    }
}
