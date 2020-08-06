using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class HUD {

        public SpriteFont PlayerNameFont; //פונט שם שחקן
        public SpriteFont PlayerBulletsFont; //פונט כמות כדורי שחקן
        public SpriteFont ItemsCapacityFont; //פונט כמות פריטים
        public SpriteFont LoadingFont; //פונט טעינה
        public SpriteFont GameTitleFont; //פונט כותרת משחק
        public SpriteFont DeadFont; //פונט מוות
        public SpriteFont PlayerCoordinatesFont; //פונט מיקום השחקן
        public SpriteFont ChooseTeamFont; //פונט בחירת קבוצה
        public SpriteFont KDFONT; //פונט הריגות ומוות
        public SpriteFont KD_PopupFont; //פונט פופאפ הריגות ומוות

        public PlayerCard PlayerCard; //כרטיסיית שחקן
        public List<PlayerCard> PlayersCards; //רשימת כרטיסיות שחקן

        public List<Popup> Popups; //רשימת פופאפים
        public List<Popup> KD_Popups; //רשימת פופאפים הריגות ומוות
        public int PlayerDeathCountDownTimer; //טיימר שחקן מת
        public bool PlayerIsAbleToBeRevived; //אינדיקציית האם השחקן יכול לקבל החייאה
        public bool PlayerCanCountDown;
        public bool PlayerIsDrown; //אינדיקציית האם השחקן טבע

        /// <summary>
        /// פונקציה המייצרת תצוגה עילית
        /// </summary>
        public HUD() {
            PlayersCards = new List<PlayerCard>();
            Popups = new List<Popup>();
            KD_Popups = new List<Popup>();
            PlayerDeathCountDownTimer = 30;
            PlayerIsAbleToBeRevived = true;
            PlayerCanCountDown = false;
            PlayerIsDrown = false;
        }

        /// <summary>
        /// טעינת תצוגה עילית
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            PlayerNameFont = content.Load<SpriteFont>("fonts/player_name_font");
            PlayerBulletsFont = content.Load<SpriteFont>("fonts/bullets_count_font");
            ItemsCapacityFont = content.Load<SpriteFont>("fonts/items_capacity_font");
            LoadingFont = content.Load<SpriteFont>("fonts/loading");
            GameTitleFont = content.Load<SpriteFont>("fonts/gameTitle");
            DeadFont = content.Load<SpriteFont>("fonts/dead_font");
            PlayerCoordinatesFont = content.Load<SpriteFont>("fonts/player_coordinates");
            ChooseTeamFont = content.Load<SpriteFont>("fonts/choose_team");
            KDFONT = content.Load<SpriteFont>("fonts/kd");
            KD_PopupFont = content.Load<SpriteFont>("fonts/kd_popup_font");
        }

        /// <summary>
        /// עדכון תצוגה עילית
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player) {
            UpdatePopups();

            PlayerCard.Update(player, new Vector2(0, 0));
        }

        /// <summary>
        /// עדכון פופאפים
        /// </summary>
        public void UpdatePopups() {
            //פופאפים רגילים
            for (int i = 0; i < Popups.Count; i++) {
                if (Popups[i].IsShowing)
                    Popups[i].Update();
                else
                    Popups.RemoveAt(i);
            }

            //פופאפים של הריגות
            for (int i = 0; i < KD_Popups.Count; i++) {
                if (KD_Popups[i].IsShowing)
                    KD_Popups[i].Update();
                else
                    KD_Popups.RemoveAt(i);
            }
        }

        /// <summary>
        /// פונקציית טיימר שחקן מת
        /// </summary>
        public void PlayerDeathCountDown() {
            while (PlayerCanCountDown && PlayerDeathCountDownTimer > 0) {
                PlayerDeathCountDownTimer--;
                Thread.Sleep(1000);
            }

            PlayerDeathCountDownTimer = 30;
            PlayerCanCountDown = false;
            PlayerIsAbleToBeRevived = false;
        }

        /// <summary>
        /// פונקציית איפוס מוות שחקן
        /// </summary>
        public void ResetPlayerDeathCountDown() {
            PlayerDeathCountDownTimer = 30;
            PlayerCanCountDown = false;
            PlayerIsAbleToBeRevived = true;
        }

        /// <summary>
        /// פונקציה המקבלת טקסט, מיקום, אינדיקציית תזוזה, ועוד עיצובי פופאפ ומייצרת פופאפ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="isMove"></param>
        /// <param name="popupLabelType"></param>
        /// <param name="popupSizeType"></param>
        public void AddPopup(string text, Vector2 position, bool isMove, PopupLabelType popupLabelType, PopupSizeType popupSizeType) {
            Popups.Add(new Popup(text, position, isMove, popupLabelType, popupSizeType));
        }


        /// <summary>
        /// פונקציה המקבלת טקסט, מיקום, אינדיקציית תזוזה, ועוד עיצובי פופאפ ומייצרת פופאפ הריגה
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="isMove"></param>
        /// <param name="popupLabelType"></param>
        /// <param name="popupSizeType"></param>
        public void AddKilledPopup(string text, Vector2 position, bool isMove, PopupLabelType popupLabelType, PopupSizeType popupSizeType) {
            KD_Popups.Add(new Popup(text, position, isMove, popupLabelType, popupSizeType));
        }

        /// <summary>
        /// פונקציה המקבלת שחקנים ומציירת את הפרטים עליהם
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="player"></param>
        /// <param name="currentPlayer"></param>
        public void DrawPlayersInfo(SpriteBatch spriteBatch, Player player, Player currentPlayer) {
            if (player.Visible) {
                if (player.Team == currentPlayer.Team) {
                    spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 70),
                        player.Team != currentPlayer.Team ? Color.Red : Color.Green);
                }
                else {
                    if (!player.IsSwimming) {
                        spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 70),
                            player.Team != currentPlayer.Team ? Color.Red : Color.Green);
                    }
                }
            }
        }

        /// <summary>
        /// פונקציה המקבלת שחקן מציירת פרטים עליו
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="player"></param>
        public void DrawPlayerInfo(SpriteBatch spriteBatch, Player player) {
            spriteBatch.DrawString(PlayerNameFont, "You", new Vector2(player.Position.X - 30, player.Position.Y - 70), Color.Blue);
        }

        /// <summary>
        /// פונקציה המקבלת שחקן ומציירת את כמות המטבעות שסוחב
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="player"></param>
        public void DrawCoinsCount(SpriteBatch spriteBatch, Player player) {
            if (player.IsCarryingCoins)
                spriteBatch.DrawString(KDFONT, "You are carrying " + player.CoinsCarrying + " Coins!\nTake them to your base!", new Vector2(50, PlayerCard.CardRectangle.Bottom + 20), Color.Black);
        }

        /// <summary>
        /// פונקציה המציירת את שם המשחק
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawGameTitle(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(GameTitleFont, "Squad Fighters", new Vector2(90, 80), Color.Black);
        }

        /// <summary>
        /// פונקציה המציירת בחירת קבוצה
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawChooseTeam(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(ChooseTeamFont, "Choose Team", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, 140), Color.White);
        }

        /// <summary>
        /// פונקציה המציירת טעינה
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="MaxItems"></param>
        /// <param name="CurrentItemsLoaded"></param>
        public void DrawLoading(SpriteBatch spriteBatch, double MaxItems, double CurrentItemsLoaded) {
            double percent = 0;

            if (MaxItems > 0 && CurrentItemsLoaded > 0) {
                percent = (double)((CurrentItemsLoaded / MaxItems) * 100);
            }

            spriteBatch.DrawString(LoadingFont, "(" + (int)percent + "%)", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 50, SquadFighters.Graphics.PreferredBackBufferHeight / 2 + 100), Color.Black);

            //spriteBatch.DrawString(LoadingFont, "Downloading\nGame Data(" + (int)percent + "% ..)", new Vector2(70, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 75), Color.Black);
        }

        /// <summary>
        /// פונקציה המציירת פופאפים
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawPopups(SpriteBatch spriteBatch) {
            foreach (Popup popup in Popups)
                popup.Draw(spriteBatch);
        }

        /// <summary>
        /// פונקציה המציירת עדכון לגבי כמה זמן נשאר לשחק לקבל החייאה לפני שמת תמידית
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawPlayerAbleToBeRevivedCountDown(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(DeadFont, PlayerDeathCountDownTimer.ToString() + " seconds to death.", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 50, 200), Color.Red);
        }

        /// <summary>
        /// פונקציה המציירת את הכרטיסיות של השחקנים
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawPlayersCards(SpriteBatch spriteBatch) {
            PlayerCard.Draw(spriteBatch);

            foreach (PlayerCard playerCard in PlayersCards)
                if (playerCard.Visible)
                    playerCard.Draw(spriteBatch);
        }

        /// <summary>
        /// פונקציה המציירת הריגות ומוות
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="currentPlayer"></param>
        public void DrawKd(SpriteBatch spriteBatch, Player currentPlayer) {
            spriteBatch.DrawString(KDFONT, currentPlayer.Kills + " Kills", new Vector2(30, SquadFighters.Graphics.PreferredBackBufferHeight - 70), Color.Black);
            spriteBatch.DrawString(KDFONT, currentPlayer.Deaths + " Deaths", new Vector2(30, SquadFighters.Graphics.PreferredBackBufferHeight - 40), Color.Black);
        }

        /// <summary>
        /// פונקציה המציירת הודעת מוות
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawDeadMessage(SpriteBatch spriteBatch) {

            if (PlayerCard.HealthBar.Health <= 0)
                spriteBatch.DrawString(DeadFont, "You Are Dead :(" + (!PlayerIsDrown ? (PlayerDeathCountDownTimer > 0 && PlayerIsAbleToBeRevived ? "\n" + PlayerDeathCountDownTimer + " sec till full DEATH." : "\nPERMANENTLY!") : "\nPERMANENTLY!"), new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 200), Color.Red);

        }

        /// <summary>
        /// פונקציה המקבלת שחקן ומציירת את המיקומים שלו
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="player"></param>
        public void DrawPlayerCoordinates(SpriteBatch spriteBatch, Player player) {
            spriteBatch.DrawString(PlayerCoordinatesFont, "(x=" + (int)player.Position.X + ",Y=" + (int)player.Position.Y + ")", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth - 130, SquadFighters.Graphics.PreferredBackBufferHeight - 25), Color.Black);
        }

        /// <summary>
        /// פונקציה המקבלת טקסט ומיקום ומציירת את הפופאפים של הריגות ומוות
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawKDPopups(SpriteBatch spriteBatch, string text, int x, int y) {
            spriteBatch.DrawString(KD_PopupFont, text, new Vector2(x, y), Color.Red);
        }

        /// <summary>
        /// פונקציה המקבלת שחקן, ואת שאר השחקנים המחוברים ומציירת אותם
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="player"></param>
        /// <param name="players"></param>
        public void Draw(SpriteBatch spriteBatch, Player player, Dictionary<string, Player> players) {
            DrawPopups(spriteBatch);

            DrawPlayerCoordinates(spriteBatch, player);

            DrawPlayersCards(spriteBatch);

            DrawKd(spriteBatch, player);

            DrawCoinsCount(spriteBatch, player);

            DrawDeadMessage(spriteBatch);
        }
    }
}
