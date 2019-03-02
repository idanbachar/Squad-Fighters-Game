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
        public ShieldBar [] ShieldBars;
        public Bubble[] Bubbles;

        private SpriteFont playerNameFont;
        private SpriteFont playerAmmoFont;

        private Vector2 playerNamePosition;
        private Vector2 playerAmmoPosition;

        public string PlayerName;
        public string AmmoString;
        public bool Visible;
        public bool CanBubble;
        private int BubbleIndex;
        private int BubbleDelayTimer;
        public bool IsBubbleHit;

        public PlayerCard(string playerName, int health, string ammoString)
        {
            PlayerName = playerName;
            CardPosition = new Vector2(0, 0);
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, 0, 0);
            HealthBar = new HealthBar(health);
            ShieldBars = new ShieldBar[3];
            Bubbles = new Bubble[5];
            AmmoString = ammoString;
            Visible = false;
            CanBubble = false;
            BubbleIndex = Bubbles.Length - 1;
            BubbleDelayTimer = 0;
            IsBubbleHit = false;
        }

        public void LoadContent(ContentManager content)
        {
            HealthBar.LoadContent(content);

            CardTexture = content.Load<Texture2D>("images/HUD/player_card");
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, CardTexture.Width, CardTexture.Height);

            playerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            playerAmmoFont = content.Load<SpriteFont>("fonts/bullets_count_font");

            for (int i = 0; i < ShieldBars.Length; i++)
            {
                ShieldBars[i] = new ShieldBar(ShieldType.None, new Vector2(0, 0));
                ShieldBars[i].LoadContent(content);
            }

            for (int i = 0; i < Bubbles.Length; i++)
            {
                Bubbles[i] = new Bubble(new Vector2(0, 0));
                Bubbles[i].LoadContent(content);
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            CardPosition = new Vector2(newPosition.X, newPosition.Y);
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, CardRectangle.Width, CardRectangle.Height);

            playerNamePosition = new Vector2(CardPosition.X + 5, CardPosition.Y + 3);
            playerAmmoPosition = new Vector2(CardRectangle.Right - 100, newPosition.Y + 5);

            HealthBar.Position = new Vector2(playerNamePosition.X, playerNamePosition.Y + 20);
            HealthBar.Rectangle = new Rectangle((int)HealthBar.Position.X, (int)HealthBar.Position.Y, HealthBar.Rectangle.Width, HealthBar.Rectangle.Height);

            for (int i = 0; i < ShieldBars.Length; i++)
                ShieldBars[i].Position = new Vector2(HealthBar.Position.X + i * 75, HealthBar.Rectangle.Bottom);

            for (int i = 0; i < Bubbles.Length; i++)
                Bubbles[i].Position = new Vector2(CardRectangle.Right + 10 + i * 40, CardRectangle.Top);
        }

        public void Update(Player currentPlayer, Vector2 newPosition)
        {
            HealthBar.SetHealth(currentPlayer.Health);
            AmmoString = currentPlayer.BulletsCapacity + "/" + currentPlayer.MaxBulletsCapacity;
            CanBubble = currentPlayer.IsSwimming;
            SetPosition(newPosition);

            if (CanBubble)
                UpdateBubble();
            else
                ResetBubbleUpdate();
        }

        public void UpdateBubble()
        {
            if (BubbleIndex > -1)
            {
                if(BubbleDelayTimer < 100)
                    BubbleDelayTimer++;
                else
                {
                    BubbleDelayTimer = 0;
                    Bubbles[BubbleIndex].Visible = false;
                    --BubbleIndex;
                }
            }
            else
            {
                ResetBubbleUpdate();
                IsBubbleHit = true;
            }
        }

        public void ResetBubbleUpdate()
        {
            BubbleIndex = Bubbles.Length - 1;
            BubbleDelayTimer = 0;
            CanBubble = false;
            IsBubbleHit = false;

            foreach (Bubble bubble in Bubbles)
                bubble.Visible = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CardTexture, CardPosition, Color.White);
            HealthBar.Draw(spriteBatch);

            foreach (ShieldBar shieldBar in ShieldBars)
                if (shieldBar.ShieldType != ShieldType.None)
                    shieldBar.Draw(spriteBatch);


            if (CanBubble && !IsBubbleHit)
                foreach (Bubble bubble in Bubbles)
                    bubble.Draw(spriteBatch);

            spriteBatch.DrawString(playerNameFont, PlayerName, playerNamePosition, Color.Black);
            spriteBatch.DrawString(playerAmmoFont, AmmoString, playerAmmoPosition, Color.Black);
        }
    }
}
