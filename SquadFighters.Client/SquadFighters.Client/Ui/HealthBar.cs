using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class HealthBar {

        public Rectangle Rectangle; //מלבן בר בריאות
        public Rectangle BackgroundRectangle; //מלבן רקע
        public Vector2 Position; //מיקום בר בריאות
        public Texture2D Texture; //טקסטורת בר בריאות
        public Texture2D BackgroundTexture; //טקסטורת רקע בר בריאות
        public int Health; //כמות בריאות

        /// <summary>
        /// פונקציה המקבלת בריאות ומייצרת בר בריאות
        /// </summary>
        /// <param name="health"></param>
        public HealthBar(int health) {
            Health = health;
            Position = new Vector2(0, 0);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Health, 0);
            BackgroundRectangle = new Rectangle((int)Position.X, (int)Position.Y, Health, 0);
        }

        /// <summary>
        /// טעינת בר בריאות
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/HUD/health_bar");
            BackgroundTexture = content.Load<Texture2D>("images/HUD/health_bar_background");
            Position = new Vector2(0, 0);
            BackgroundRectangle = new Rectangle((int)Position.X, (int)Position.Y, BackgroundTexture.Width, BackgroundTexture.Height);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה המקבלת בריאות ומעדכנת את בר הבריאות
        /// </summary>
        /// <param name="health"></param>
        public void SetHealth(int health) {
            Health = health;
            Rectangle.Width = Health;
        }

        /// <summary>
        /// ציור בר בריאות
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(BackgroundTexture, BackgroundRectangle, Color.White);
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
