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
    public class Water {

        public Vector2 Position; //מיקום מים
        public Rectangle Rectangle; //מלבן מים
        public Texture2D Texture; //טקסטורת מים
        public WaterShape WaterShape; //סוג צורת מים

        /// <summary>
        /// פונקציה המקבלת מיקום וסוג צורת מים ומייצרת מים
        /// </summary>
        /// <param name="position"></param>
        /// <param name="waterShape"></param>
        public Water(Vector2 position, WaterShape waterShape) {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            WaterShape = waterShape;
        }

        /// <summary>
        /// טעינת מים
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/map/world/water/water_" + WaterShape.ToString());
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// ציור מים
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
