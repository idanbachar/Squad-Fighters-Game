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
        public SpriteFont LoadingFont;
        public SpriteFont GameTitleFont;
        public SpriteFont DeadFont;
        public List<PlayerCard> PlayersCards;
        public PlayerCard PlayerCard;

        public HUD()
        {
            PlayersCards = new List<PlayerCard>();
        }
 
        public void LoadContent(ContentManager content)
        {
            PlayerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            PlayerBulletsFont = content.Load<SpriteFont>("fonts/bullets_count_font");
            ItemsCapacityFont = content.Load<SpriteFont>("fonts/items_capacity_font");
            LoadingFont = content.Load<SpriteFont>("fonts/loading");
            GameTitleFont = content.Load<SpriteFont>("fonts/gameTitle");
            DeadFont = content.Load<SpriteFont>("fonts/dead_font");
        }

        public void DrawPlayersInfo(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 50), Color.Black);
        }

        public void DrawPlayerInfo(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, "You", new Vector2(player.Position.X - 30, player.Position.Y - 50), Color.Blue);
        }

        public void DrawGameTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameTitleFont, "SquadFighters", new Vector2(100, 100), Color.White);
        }

        public void DrawLoading(SpriteBatch spriteBatch, double MaxItems, double CurrentItemsLoaded)
        {
            double percent = 0;

            if (MaxItems > 0 && CurrentItemsLoaded > 0)
            {
                percent = (double)((CurrentItemsLoaded / MaxItems) * 100);
            }
            spriteBatch.DrawString(LoadingFont, "Loading..(" + (int)percent + "%)", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 75), Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, Dictionary<string, Player> players)
        {
            PlayerCard.Draw(spriteBatch);

            foreach (PlayerCard playerCard in PlayersCards)
                if (playerCard.Visible)
                    playerCard.Draw(spriteBatch);

            if (PlayerCard.HealthBar.Health <= 0)
                spriteBatch.DrawString(DeadFont, "You Are Dead :(", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 200), Color.Red);

        }
    }
}
