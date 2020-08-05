using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Background {

        public Vector2 Position; //מיקום רקע
        public Texture2D Texture; //טקסטורת רקע

        /// <summary>
        /// פונקציה המקבלת מיקום ויוצרת רקע
        /// </summary>
        /// <param name="position"></param>
        public Background(Vector2 position) {
            Position = new Vector2(position.X, position.Y);
        }

        /// <summary>
        /// טעינת רקע
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/map/background");
        }

        /// <summary>
        /// ציור רקע
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
