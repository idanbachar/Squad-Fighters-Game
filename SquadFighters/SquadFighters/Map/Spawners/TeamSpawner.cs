using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class TeamSpawner
    {
        public Vector2 Position;
        public Rectangle Rectangle;
        public Team Team;
        public Texture2D Texture;
        public int Coins;

        public TeamSpawner(Vector2 position, Team team)
        {
            Position = new Vector2(position.X, position.Y);
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            Team = team;
            Coins = 0;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/map/world/spawners/" + Team.ToString() + "_team_spawner");
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
