using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class ShieldBar {

        public ShieldType ShieldType; //סוג הגנה
        public Texture2D Texture; //טקסטורת בר הגנה
        public Vector2 Position; //מיקום בר הגנה
        public int Armor; //מגן

        /// <summary>
        /// פונקציה המקבלת סוג מגן ומיקום ומייצרת בר הגנה
        /// </summary>
        /// <param name="shieldType"></param>
        /// <param name="position"></param>
        public ShieldBar(ShieldType shieldType, Vector2 position) {
            ShieldType = shieldType;
            Position = new Vector2(position.X, position.Y);
        }

        /// <summary>
        /// טעינת בר הגנה
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/player/armor_bars/" + GetArmorImageName());

            switch (ShieldType) {
                case ShieldType.None:
                    Armor = 0;
                    break;
                case ShieldType.Shield_Level_1:
                    Armor = 50;
                    break;
                case ShieldType.Shield_Level_2:
                    Armor = 75;
                    break;
                case ShieldType.Shield_Rare:
                    Armor = 100;
                    break;
                case ShieldType.Shield_Legendery:
                    Armor = 150;
                    break;
            }
        }

        /// <summary>
        /// בדיקת קבלת מכה ושינוי המגן בהתאם
        /// </summary>
        /// <param name="damage"></param>
        public void Hit(int damage) {
            if ((Armor - damage) > 0) {
                Armor -= damage;

                if (Armor <= 0) {
                    Armor = 0;
                    ShieldType = ShieldType.None;
                    LoadContent(SquadFighters.ContentManager);
                }
            }
            else {
                Armor = 0;
                ShieldType = ShieldType.None;
                LoadContent(SquadFighters.ContentManager);
            }
        }

        /// <summary>
        /// פונקציה המחזירה את שם המגן כטקסט
        /// </summary>
        /// <returns></returns>
        public string GetArmorImageName() {
            switch (ShieldType) {
                case ShieldType.None:
                    return "armor_none";
                case ShieldType.Shield_Level_1:
                    return "armor_lv1";
                case ShieldType.Shield_Level_2:
                    return "armor_lv2";
                case ShieldType.Shield_Rare:
                    return "armor_rare";
                case ShieldType.Shield_Legendery:
                    return "armor_legendery";
            }

            return string.Empty;
        }

        /// <summary>
        /// ציור בר הגנה
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            if (Armor > 0 && ShieldType != ShieldType.None || Armor == 0 && ShieldType == ShieldType.None)
                spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
