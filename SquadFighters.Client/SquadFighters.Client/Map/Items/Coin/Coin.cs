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
    public class Coin : Item {

        private Random Random; //רנדום
        public CoinType ItemType; //סוג מטבע
        public int Points; //נקודות

        /// <summary>
        /// פונקציה המקבלת מיקום, סוג מטבע, כמות ומייצרת מטבע
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="itemType"></param>
        /// <param name="capacity"></param>
        public Coin(Vector2 itemPosition, CoinType itemType, int capacity) : base(itemPosition) {
            ItemType = itemType;
            Random = new Random();
            Points = capacity;
        }

        /// <summary>
        /// טעינת מטבע
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/items/Coins/" + ItemType);
        }

        /// <summary>
        /// עדכון מטבע
        /// </summary>
        public override void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה לקבלת כמות נקודות
        /// </summary>
        /// <returns></returns>
        public int GetPoints() {
            return Points;
        }


        /// <summary>
        /// פונקציית ציור מטבע
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
            return Points.ToString();
        }
    }
}
