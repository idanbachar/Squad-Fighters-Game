using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SquadFighters.Client {
    public class GunAmmo : Item {
        public int Capacity; //כמות כדורים
        public AmmoType ItemType; //סוג כדורים

        /// <summary>
        /// פונקציה המקבלת מיקום, סוג כדורים וכמות ומייצרת כדורים
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="ammoType"></param>
        /// <param name="capacity"></param>
        public GunAmmo(Vector2 itemPosition, AmmoType ammoType, int capacity) : base(itemPosition) {
            Capacity = capacity;
            ItemType = ammoType;
        }

        /// <summary>
        /// טעינת כדורים
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/items/ammunition/bullet");
        }

        /// <summary>
        /// עדכון כדורים
        /// </summary>
        public override void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// ציור כדורים
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        /// <summary>
        /// יצירת פורמט טקסט
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Capacity.ToString();
        }
    }
}
