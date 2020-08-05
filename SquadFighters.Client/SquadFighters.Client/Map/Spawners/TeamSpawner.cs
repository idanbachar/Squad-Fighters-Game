using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class TeamSpawner {

        public Vector2 Position; //מיקום בסיס
        public Rectangle Rectangle; //מלבן בסיס
        public Team Team; //קבוצת בסיס
        public Texture2D Texture; //טקסטורת בסיס
        public int Coins; //כמות מטבעות

        /// <summary>
        /// פונקציה המקבלת מיקום וקבוצה ומייצרת בסיס
        /// </summary>
        /// <param name="position"></param>
        /// <param name="team"></param>
        public TeamSpawner(Vector2 position, Team team) {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            Team = team;
            Coins = 0;
        }

        /// <summary>
        /// טעינת בסיס
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Texture = content.Load<Texture2D>("images/map/world/spawners/" + Team.ToString() + "_team_spawner");
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// ציור בסיס
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
