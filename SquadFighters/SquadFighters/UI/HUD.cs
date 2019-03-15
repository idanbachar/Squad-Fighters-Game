using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public SpriteFont PlayerCoordinatesFont;
        public SpriteFont ChooseTeamFont;
        public SpriteFont KDFONT;
        public SpriteFont KD_PopupFont;

        public PlayerCard PlayerCard;
        public List<PlayerCard> PlayersCards;

        public List<Popup> Popups;
        public List<Popup> KD_Popups;
        public int PlayerDeathCountDownTimer;
        public bool PlayerIsAbleToBeRevived;
        public bool PlayerCanCountDown;
        public bool PlayerIsDrown;

        public HUD()
        {
            PlayersCards = new List<PlayerCard>();
            Popups = new List<Popup>();
            KD_Popups = new List<Popup>();
            PlayerDeathCountDownTimer = 30;
            PlayerIsAbleToBeRevived = true;
            PlayerCanCountDown = false;
            PlayerIsDrown = false;
        }
 
        public void LoadContent(ContentManager content)
        {
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

        public void Update(Player player)
        {
            UpdatePopups();

            PlayerCard.Update(player, new Vector2(0, 0));
        }

        public void UpdatePopups()
        {
            //פופאפים רגילים
            for(int i = 0; i < Popups.Count; i++)
            {
                if (Popups[i].IsShowing)
                    Popups[i].Update();
                else
                    Popups.RemoveAt(i);
            }

            //פופאפים של הריגות
            for (int i = 0; i < KD_Popups.Count; i++)
            {
                if (KD_Popups[i].IsShowing)
                    KD_Popups[i].Update();
                else
                    KD_Popups.RemoveAt(i);
            }
        }

        public void PlayerDeathCountDown()
        {
            while (PlayerCanCountDown && PlayerDeathCountDownTimer > 0)
            {
                PlayerDeathCountDownTimer--;
                Thread.Sleep(1000);
            }

            PlayerDeathCountDownTimer = 30;
            PlayerCanCountDown = false;
            PlayerIsAbleToBeRevived = false;
        }

        public void ResetPlayerDeathCountDown()
        {
            PlayerDeathCountDownTimer = 30;
            PlayerCanCountDown = false;
            PlayerIsAbleToBeRevived = true;
        }

        public void AddPopup(string text, Vector2 position, bool isMove, PopupLabelType popupLabelType, PopupSizeType popupSizeType)
        {
            Popups.Add(new Popup(text, position, isMove, popupLabelType, popupSizeType));
        }

        public void AddKilledPopup(string text, Vector2 position, bool isMove, PopupLabelType popupLabelType, PopupSizeType popupSizeType)
        {
            KD_Popups.Add(new Popup(text, position, isMove, popupLabelType, popupSizeType));
        }

        public void DrawPlayersInfo(SpriteBatch spriteBatch, Player player, Player currentPlayer)
        {
            if (player.Visible)
            {
                if (player.Team == currentPlayer.Team)
                {
                    spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 70),
                        player.Team != currentPlayer.Team ? Color.Red : Color.Green);
                }
                else
                {
                    if(!player.IsSwimming)
                    {
                        spriteBatch.DrawString(PlayerNameFont, player.Name, new Vector2(player.Position.X - 30, player.Position.Y - 70),
                            player.Team != currentPlayer.Team ? Color.Red : Color.Green);
                    }
                }
            }
        }

        public void DrawPlayerInfo(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerNameFont, "You", new Vector2(player.Position.X - 30, player.Position.Y - 70), Color.Blue);
        }

        public void DrawGameTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameTitleFont, "Squad Fighters", new Vector2(90, 80), Color.Black);
        }

        public void DrawChooseTeam(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ChooseTeamFont, "Choose Team", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, 140), Color.White);
        }

        public void DrawLoading(SpriteBatch spriteBatch, double MaxItems, double CurrentItemsLoaded)
        {
            double percent = 0;

            if (MaxItems > 0 && CurrentItemsLoaded > 0)
            {
                percent = (double)((CurrentItemsLoaded / MaxItems) * 100);
            }

            spriteBatch.DrawString(LoadingFont, "(" + (int)percent + "%)", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 50, SquadFighters.Graphics.PreferredBackBufferHeight / 2 + 50), Color.Black);

            //spriteBatch.DrawString(LoadingFont, "Downloading\nGame Data(" + (int)percent + "% ..)", new Vector2(70, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 75), Color.Black);
        }

        public void DrawPopups(SpriteBatch spriteBatch)
        {
            foreach (Popup popup in Popups)
                popup.Draw(spriteBatch);
        }

        public void DrawPlayerAbleToBeRevivedCountDown(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(DeadFont , PlayerDeathCountDownTimer.ToString() + " seconds to death.", new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 50, 200), Color.Red);
        }

        public void DrawPlayersCards(SpriteBatch spriteBatch)
        {
            PlayerCard.Draw(spriteBatch);

            foreach (PlayerCard playerCard in PlayersCards)
                if (playerCard.Visible)
                    playerCard.Draw(spriteBatch);
        }

        public void DrawKd(SpriteBatch spriteBatch, Player currentPlayer)
        {
            spriteBatch.DrawString(KDFONT, currentPlayer.Kills + " Kills" , new Vector2(30, SquadFighters.Graphics.PreferredBackBufferHeight - 60), Color.Black);
            spriteBatch.DrawString(KDFONT, currentPlayer.Deaths + " Deaths", new Vector2(30, SquadFighters.Graphics.PreferredBackBufferHeight - 30), Color.Black);
        }

        public void DrawDeadMessage(SpriteBatch spriteBatch)
        {

            if (PlayerCard.HealthBar.Health <= 0)
                spriteBatch.DrawString(DeadFont, "You Are Dead :(" + (!PlayerIsDrown ? (PlayerDeathCountDownTimer > 0 && PlayerIsAbleToBeRevived ? "\n" + PlayerDeathCountDownTimer + " sec till full DEATH." : "\nPERMANENTLY!")  : "\nPERMANENTLY!"), new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 200), Color.Red);

        }

        public void DrawPlayerCoordinates(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.DrawString(PlayerCoordinatesFont, "(x=" + (int)player.Position.X + ",Y=" + (int)player.Position.Y + ")", new Vector2(PlayerCard.CardRectangle.Right + 10, PlayerCard.CardRectangle.Bottom - 10), Color.Black);
        }

        public void DrawKDPopups(SpriteBatch spriteBatch, string text, int x, int y)
        {
            spriteBatch.DrawString(KD_PopupFont, text, new Vector2(x, y), Color.Red);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, Dictionary<string, Player> players)
        {
            DrawPopups(spriteBatch);

            DrawPlayerCoordinates(spriteBatch, player);

            DrawPlayersCards(spriteBatch);

            DrawKd(spriteBatch, player);

            DrawDeadMessage(spriteBatch);
        }
    }
}
