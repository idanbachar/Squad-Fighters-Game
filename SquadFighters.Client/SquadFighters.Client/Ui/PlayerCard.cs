using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class PlayerCard {

        public Rectangle CardRectangle; //מלבן כרטיסייה
        public Texture2D CardTexture; //טקסטורת כרטיסייה
        public Vector2 CardPosition; //מיקום כרטיסייה

        public HealthBar HealthBar; //בר בריאות כרטיסייה
        public ShieldBar[] ShieldBars; //מערך בר מגנים
        public Bubble[] Bubbles; //מערך בועות

        private SpriteFont playerNameFont; //פונט שם שחקן
        private SpriteFont playerAmmoFont; //פונט כדורי שחקן

        private Vector2 playerNamePosition; //מיקום שם שחקן
        private Vector2 playerAmmoPosition; //מיקום כדורי שחקן

        public string PlayerName; //שם שחקן
        public string AmmoString; //כדורי שחקן
        public bool Visible; //האם הכרטיסייה מוצגת


        public bool CanBubble; //האם להציג בועות
        private int BubbleIndex; //אינדקס בועה
        private int BubbleDelayTimer; //טיימר דיליי בין בועות
        public bool IsBubbleHit; //האם כל הבועות נגמרו וצריך לפגוע בשחקן

        /// <summary>
        /// פונקציה המקבלת שם שחקן, כמות בריאות וכמות כדורים ומייצרת כרטיסיית שחקן
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="health"></param>
        /// <param name="ammoString"></param>
        public PlayerCard(string playerName, int health, string ammoString) {
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

        /// <summary>
        /// טעינת כרטיסיית שחקן
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            HealthBar.LoadContent(content);

            CardTexture = content.Load<Texture2D>("images/HUD/player_card");
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, CardTexture.Width, CardTexture.Height);

            playerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            playerAmmoFont = content.Load<SpriteFont>("fonts/bullets_count_font");

            for (int i = 0; i < ShieldBars.Length; i++) {
                ShieldBars[i] = new ShieldBar(ShieldType.None, new Vector2(0, 0));
                ShieldBars[i].LoadContent(content);
            }

            for (int i = 0; i < Bubbles.Length; i++) {
                Bubbles[i] = new Bubble(new Vector2(0, 0));
                Bubbles[i].LoadContent(content);
            }
        }

        /// <summary>
        /// פונקצייה המקבלת מיקום ומייצרת מיקום חדש
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition) {
            CardPosition = new Vector2(newPosition.X, newPosition.Y);
            CardRectangle = new Rectangle((int)CardPosition.X, (int)CardPosition.Y, CardRectangle.Width, CardRectangle.Height);

            playerNamePosition = new Vector2(CardPosition.X + 5, CardPosition.Y + 3);
            playerAmmoPosition = new Vector2(CardRectangle.Right - 60, newPosition.Y + 5);

            HealthBar.Position = new Vector2(playerNamePosition.X + 3, playerNamePosition.Y + 20);
            HealthBar.BackgroundRectangle = new Rectangle((int)HealthBar.Position.X, (int)HealthBar.Position.Y, HealthBar.BackgroundRectangle.Width, HealthBar.BackgroundRectangle.Height);
            HealthBar.Rectangle = new Rectangle((int)HealthBar.BackgroundRectangle.X + 5, (int)HealthBar.BackgroundRectangle.Y + 5, HealthBar.Rectangle.Width - 10, HealthBar.Rectangle.Height);

            for (int i = 0; i < ShieldBars.Length; i++)
                ShieldBars[i].Position = new Vector2(3 + HealthBar.Position.X + i * 75, HealthBar.BackgroundRectangle.Bottom + 5);

            for (int i = 0; i < Bubbles.Length; i++)
                Bubbles[i].Position = new Vector2(CardRectangle.Right + 10 + i * 40, CardRectangle.Top);
        }

        /// <summary>
        /// פונקציה המקבלת שחקן ומיקום ומעדכנת כרטיסיית שחקן בהתאם
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <param name="newPosition"></param>
        public void Update(Player currentPlayer, Vector2 newPosition) {
            HealthBar.SetHealth(currentPlayer.Health);
            AmmoString = currentPlayer.BulletsCapacity + "/" + currentPlayer.MaxBulletsCapacity;
            CanBubble = currentPlayer.IsSwimming;
            SetPosition(newPosition);

            if (CanBubble)
                UpdateBubble();
            else
                ResetBubbleUpdate();

            if (ShieldBars[2].Armor <= 0) {
                currentPlayer.IsShield = false;
                currentPlayer.ShieldType = ShieldType.None;
            }
        }

        /// <summary>
        /// עדכון בועה
        /// </summary>
        public void UpdateBubble() {
            if (BubbleIndex > -1) {
                if (BubbleDelayTimer < 50)
                    BubbleDelayTimer++;
                else {
                    BubbleDelayTimer = 0;
                    Bubbles[BubbleIndex].Visible = false;
                    --BubbleIndex;
                }
            }
            else {
                ResetBubbleUpdate();
                IsBubbleHit = true;
            }
        }

        /// <summary>
        /// עדכון איפוס בועה
        /// </summary>
        public void ResetBubbleUpdate() {
            BubbleIndex = Bubbles.Length - 1;
            BubbleDelayTimer = 0;
            CanBubble = false;
            IsBubbleHit = false;

            foreach (Bubble bubble in Bubbles)
                bubble.Visible = true;

        }

        /// <summary>
        /// ציור כרטיסיית שחקן
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(CardTexture, CardPosition, Color.White);
            HealthBar.Draw(spriteBatch);

            foreach (ShieldBar shieldBar in ShieldBars)
                shieldBar.Draw(spriteBatch);


            if (CanBubble && !IsBubbleHit)
                foreach (Bubble bubble in Bubbles)
                    bubble.Draw(spriteBatch);

            spriteBatch.DrawString(playerNameFont, PlayerName, playerNamePosition, Color.Black);
            spriteBatch.DrawString(playerAmmoFont, AmmoString, playerAmmoPosition, Color.Black);
        }
    }
}
