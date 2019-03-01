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
    public class PlayerCard
    {
        public Rectangle CardRectangle;
        public Texture2D CardTexture;
        public Vector2 CardPosition;

        public HealthBar HealthBar;
        public Shield Shield;

        private SpriteFont playerNameFont;
        private SpriteFont playerAmmoFont;

        private Vector2 playerNamePosition;
        private Vector2 playerAmmoPosition;

        public string PlayerName;
        public string AmmoString;

        public PlayerCard(string playerName, int health, string ammoString)
        {
            PlayerName = playerName;
            CardPosition = new Vector2(0, 0);
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, 0, 0);
            HealthBar = new HealthBar(health);
            Shield = new Shield(new Vector2(0, 0), ShieldType.None, 100);
            AmmoString = ammoString;
        }

        public void LoadContent(ContentManager content)
        {
            HealthBar.LoadContent(content);

            CardTexture = content.Load<Texture2D>("images/HUD/player_card");
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, CardTexture.Width, CardTexture.Height);

            playerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            playerAmmoFont = content.Load<SpriteFont>("fonts/bullets_count_font");

            Shield.LoadContent(content);
        }

        public void SetPosition(Vector2 newPosition)
        {
            CardPosition = new Vector2(newPosition.X, newPosition.Y);
            playerNamePosition = new Vector2(CardPosition.X + 5, CardPosition.Y + 3);
            playerAmmoPosition = new Vector2(CardRectangle.Right - 100, newPosition.Y + 5);

            HealthBar.Position = new Vector2(playerNamePosition.X, playerNamePosition.Y + 20);
            HealthBar.Rectangle = new Rectangle((int)HealthBar.Position.X, (int)HealthBar.Position.Y, HealthBar.Rectangle.Width, HealthBar.Rectangle.Height);

            if(Shield.ItemType != ShieldType.None)
            {
                for(int i = 0; i < Shield.ShieldBars.Length; i++)
                {
                    Shield.ShieldBars[i].Position = new Vector2(HealthBar.Position.X + i * 75, HealthBar.Rectangle.Bottom );
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CardTexture, CardPosition, Color.White);
            HealthBar.Draw(spriteBatch);
            if (Shield.ItemType != ShieldType.None)
            {
                foreach (ShieldBar shieldBar in Shield.ShieldBars)
                    shieldBar.Draw(spriteBatch);
            }
            spriteBatch.DrawString(playerNameFont, PlayerName, playerNamePosition, Color.Black);
            spriteBatch.DrawString(playerAmmoFont, AmmoString, playerAmmoPosition, Color.Black);
        }
    }
}
