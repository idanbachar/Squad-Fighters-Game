using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Royale_Project
{
    public class HUD
    {
        private SpriteFont PlayerNameFont;
        private SpriteFont PlayerBulletsFont;

        private string PlayerName;
        private string PlayerBullets;

        public HUD()
        {
        }

        public void LoadContent(ContentManager content)
        {
            PlayerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            PlayerBulletsFont = content.Load<SpriteFont>("fonts/bullets_count_font");
        }

        public void Update(Player player)
        {
            PlayerName = player.Name;
            PlayerBullets = player.bullets.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(PlayerNameFont, PlayerName, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(PlayerBulletsFont, PlayerBullets, new Vector2(0, 150), Color.White);
        }
    }
}
