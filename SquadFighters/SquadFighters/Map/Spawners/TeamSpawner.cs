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
        public Team Team;
        public Texture2D Texture;

        public TeamSpawner(Vector2 position, Team team)
        {
            Position = new Vector2(position.X, position.Y);
            Team = team;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/map/world/spawners/" + Team.ToString() + "_team_spawner");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
