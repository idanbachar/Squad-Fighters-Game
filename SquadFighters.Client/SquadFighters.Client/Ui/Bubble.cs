using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Bubble {

        public Texture2D Texture; //טקסטורת בועה
        public Vector2 Position; //מיקום בועה
        public bool Visible; //האם הבועה מוצגת

        /// <summary>
        /// פונקציה המקבלת מיקום ומייצרת בועה
        /// </summary>
        /// <param name="position"></param>
        public Bubble(Vector2 position) {
            Position = new Vector2(position.X, position.Y);
            Visible = true;
        }

        /// <summary>
        /// טעינת בועה
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/HUD/bubble");
        }

        /// <summary>
        /// ציור בועה
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            if (Visible)
                spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
