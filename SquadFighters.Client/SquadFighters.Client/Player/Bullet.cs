using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Bullet {

        public Texture2D Texture; //טקסטורת כדור
        public Vector2 Position; //מיקום כדור
        public Rectangle Rectangle; //מלבן כדור
        public Vector2 Direction; //כיוון כדור
        public float Speed; //מהירות כדור
        public bool IsFinished; //אינדיקציית סיום יריית כדור
        private int Timer; //טיימר יריית כדור
        private int MaxTimer; //גבול מקסימלי יריית כדור
        public int Damage; //כאב פגיעה מכדור
        public string Owner; //שם שחקן בעל הכדור

        /// <summary>
        /// פונקציה המקבלת מיקום, כיוון ושם שחקן בעל הכדור ויוצרת כדור
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="owner"></param>
        public Bullet(Vector2 position, Vector2 direction, string owner) {
            Position = new Vector2(position.X, position.Y);
            Direction = new Vector2(direction.X, direction.Y);
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            Speed = 4.3f;
            IsFinished = false;
            Timer = 0;
            MaxTimer = 30;
            Damage = 20;
            Owner = owner;
        }

        /// <summary>
        /// טעינת כדור
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/player/bullets/bullet");
            Rectangle.Width = Texture.Width;
            Rectangle.Height = Texture.Height;
        }

        /// <summary>
        /// עדכון כדור
        /// </summary>
        public void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            //הזזת כדור:
            Move();
        }

        /// <summary>
        /// פונקציה המזיזה את הכדור לכיוון שאליו השחקן מכוון
        /// </summary>
        public void Move() {
            Position += Direction * Speed;

            if (Timer <= MaxTimer)
                Timer++;
            else
                IsFinished = true;
        }

        /// <summary>
        /// ציור הכדור
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
