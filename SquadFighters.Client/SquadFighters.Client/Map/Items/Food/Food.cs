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
    public class Food : Item {

        private Random Random; //רנדום
        public FoodType ItemType; //סוג אוכל
        public int Heal; //בריאות

        /// <summary>
        /// פונקציה המקבלת מיקום, סוג אוכל וכמות ומייצרת אוכל
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="itemType"></param>
        /// <param name="capacity"></param>
        public Food(Vector2 itemPosition, FoodType itemType, int capacity) : base(itemPosition) {
            ItemType = itemType;
            Random = new Random();
            Heal = capacity;
        }

        /// <summary>
        /// טעינת אוכל
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/items/food/" + ItemType);
        }

        /// <summary>
        /// עדכון אוכל
        /// </summary>
        public override void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה לקבלת בריאות
        /// </summary>
        /// <returns></returns>
        public int GetHealth() {
            return Heal;
        }

        /// <summary>
        /// ציור אוכל
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
            return Heal.ToString();
        }
    }
}
