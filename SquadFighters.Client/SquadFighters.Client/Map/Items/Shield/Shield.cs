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
    public class Shield : Item {

        public int Armor; //מגן
        public ShieldType ItemType; //סוג מגן

        /// <summary>
        /// פונקציה המקבלת מיקום, סוג מגן וכמות ומייצרת מגן
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="itemType"></param>
        /// <param name="capacity"></param>
        public Shield(Vector2 itemPosition, ShieldType itemType, int capacity) : base(itemPosition) {
            ItemType = itemType;
            Armor = capacity;
        }

        /// <summary>
        /// טעינת מגן
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/items/shields/" + GetShieldName());
        }

        /// <summary>
        /// פונקציה המחזירה סוג מגן כטקסט
        /// </summary>
        /// <returns></returns>
        private string GetShieldName() {
            switch (ItemType) {
                case ShieldType.Shield_Level_1:
                case ShieldType.None:
                    return "shield_lv1";
                case ShieldType.Shield_Level_2:
                    return "shield_lv2";
                case ShieldType.Shield_Rare:
                    return "shield_rare";
                case ShieldType.Shield_Legendery:
                    return "shield_legendery";
                default:
                    return "shield_lv1";
            }
        }

        /// <summary>
        /// עדכון מגן
        /// </summary>
        public override void Update() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה המחזירה מגן
        /// </summary>
        /// <returns></returns>
        public int GetArmor() {
            return Armor;
        }

        /// <summary>
        /// ציור מגן
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
