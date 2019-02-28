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
    public class HUD
    {
        public SpriteFont PlayerNameFont;
        public SpriteFont PlayerBulletsFont;
        public SpriteFont ItemsCapacityFont;

        public HUD()
        {
        }

        public void LoadContent(ContentManager content)
        {
            PlayerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            PlayerBulletsFont = content.Load<SpriteFont>("fonts/bullets_count_font");
            ItemsCapacityFont = content.Load<SpriteFont>("fonts/items_capacity_font");
        }

        public void DrawPlayerName(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, player.Name + "(x=" + (int)player.Position.X + ",y=" + (int)player.Position.Y + ")", new Vector2(0, 0), Color.Black);
        }

        public void DrawPlayerHealth(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, "HP: " + player.Health, new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth - 100, 0), Color.Red);
        }

        public void DrawPlayerBullets(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerBulletsFont, "Ammo: " + player.BulletsCapacity.ToString() + "/" + player.MaxBulletsCapacity.ToString(), new Vector2(0, SquadFighters.Graphics.PreferredBackBufferHeight - 50), Color.White);
        }

        public void DrawPlayersInfo(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 50), Color.Black);
        }

        public void DrawPlayerInfo(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, "You", new Vector2(player.Position.X - 30, player.Position.Y - 50), Color.Blue);
        }

        public void DrawLoading(SpriteBatch spriteBatch, double MaxItems, double CurrentItemsLoaded)
        {
            double percent = 0;

            if (MaxItems > 0 && CurrentItemsLoaded > 0)
            {
                percent = (double)((CurrentItemsLoaded / MaxItems) * 100);
            }

            spriteBatch.DrawString(PlayerBulletsFont, "Loading..(" + (int)percent + "%)", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 75), Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, Dictionary<string, Player> players)
        {
            DrawPlayerName(spriteBatch, player);
            DrawPlayerHealth(spriteBatch, player);
            DrawPlayerBullets(spriteBatch, player);
        }
    }
}
