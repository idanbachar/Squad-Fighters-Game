using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Popup {

        private SpriteFont Font; //פונט
        public Vector2 Position; //מיקום פופאפ
        public string Text; //טקסט פופאפ
        public bool IsShowing; //אינדיקציית האם הפופאפ מוצג
        public bool IsMove; //אינדיקציית האם הפופאפ זז
        private int timeLimiter; //גבול זמן הצגת פופאפ
        private int timer; //טיימר הצגת פופאפ
        public PopupLabelType PopupLabelType; //סוג פופאפ
        public PopupSizeType PopupSizeType; //סוג גודל פופאפ

        /// <summary>
        /// פונקציה המקבלת טקסט, מיקום, אינדיקציית האם זז, סוג פופאפ, סוג גודל פופאפ ומייצרת פופאפ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="isMove"></param>
        /// <param name="popupLabelType"></param>
        /// <param name="popupSizeType"></param>
        public Popup(string text, Vector2 position, bool isMove, PopupLabelType popupLabelType, PopupSizeType popupSizeType) {
            Text = text;
            Position = new Vector2(position.X, position.Y);
            IsShowing = true;
            timeLimiter = !isMove ? 300 : 70;
            timer = 0;
            IsMove = isMove;
            PopupLabelType = popupLabelType;
            PopupSizeType = popupSizeType;
            LoadContent();
        }

        /// <summary>
        /// טעינת פופאפ
        /// </summary>
        public void LoadContent() {
            Font = SquadFighters.ContentManager.Load<SpriteFont>("fonts/" + PopupSizeType.ToString());
        }

        /// <summary>
        /// עדכון פופאפ
        /// </summary>
        public void Update() {
            if (timer < timeLimiter) {
                timer++;
                if (IsMove) {
                    Move();
                }
            }
            else {
                timer = 0;
                IsShowing = false;
            }
        }

        /// <summary>
        /// הזזת פופאפ
        /// </summary>
        private void Move() {
            Position.Y -= 2;
        }

        /// <summary>
        /// ציור פופאפ
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(Font, Text, Position, PopupLabelType == PopupLabelType.Regular ? Color.Black :
                                                         PopupLabelType == PopupLabelType.Nice ? Color.Green :
                                                         PopupLabelType == PopupLabelType.Warning ? Color.Red :
                                                         Color.Black);
        }
    }
}
