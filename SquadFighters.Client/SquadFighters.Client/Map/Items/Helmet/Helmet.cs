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
    public class Helmet : Item {

        public int Armor; //מגן
        public HelmetType ItemType; //סוג קסדה

        /// <summary>
        /// פונקציה המקבלת מיקום, סוג קסדה וכמות ומייצרת קסדה
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="itemType"></param>
        /// <param name="capacity"></param>
        public Helmet(Vector2 itemPosition, HelmetType itemType, int capacity) : base(itemPosition) {
            ItemType = itemType;
            Armor = capacity;
        }

        /// <summary>
        /// טעינת קסדה
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/items/helmets/" + GetHelmetName());
        }

        /// <summary>
        /// פונקציה המחזירה סוג קסדה כטקסט
        /// </summary>
        /// <returns></returns>
        private string GetHelmetName() {
            switch (ItemType) {
                case HelmetType.Helmet_Level_1:
                    return "helmet_lv1";
                case HelmetType.Helmet_Level_2:
                    return "helmet_lv2";
                case HelmetType.Helmet_Rare:
                    return "helmet_rare";
                case HelmetType.Helmet_Legendery:
                    return "helmet_legendery";
                default:
                    return "helmet_lv1";
            }
        }

        /// <summary>
        /// עדכון קסדה
        /// </summary>
        public override void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה לקבלת המגן
        /// </summary>
        /// <returns></returns>
        public int GetArmor() {
            return Armor;
        }

        /// <summary>
        /// ציור הקסדה
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
            return Armor.ToString();
        }
    }
}
