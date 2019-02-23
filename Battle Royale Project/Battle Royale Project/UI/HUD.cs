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
        public SpriteFont PlayerNameFont;
        public SpriteFont PlayerBulletsFont;
        public SpriteFont ItemsCapacityFont;

        private string PlayerName;
        private string PlayerBullets;
        private string PlayerMaxBullets;

        public HUD()
        {
        }

        public void LoadContent(ContentManager content)
        {
            PlayerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            PlayerBulletsFont = content.Load<SpriteFont>("fonts/bullets_count_font");
            ItemsCapacityFont = content.Load<SpriteFont>("fonts/items_capacity_font");
        }

        public void Update(Player player)
        {
            PlayerName = player.Name + " (" + player.Health + "hp)";
            PlayerBullets = player.BulletsCapacity.ToString();
            PlayerMaxBullets = player.MaxBulletsCapacity.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(PlayerNameFont, PlayerName, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(PlayerBulletsFont, PlayerBullets + "/" + PlayerMaxBullets, new Vector2(0, Game1.Graphics.PreferredBackBufferHeight - 50), Color.Black);
        }
    }
}
