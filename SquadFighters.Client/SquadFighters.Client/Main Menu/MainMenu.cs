using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class MainMenu {

        public Button[] Buttons; //מערך כפתורים
        public Button[] Teams; //מערך כפתורי קבוצות
        public Texture2D BackgroundTexture; //תמונת רקע לתפריט הראשי
        public Texture2D BackgroundTeamTexture; //תמונת רקע לקבוצות
        public Texture2D BackgroundDownloadTexture; //תמונת רקע להורדה מהשרת
        public Vector2 BackgroundPosition; //מיקום תמונת רקע
        public Player MenuPlayer; //השחקן המוצג בתפריט
        private SpriteFont Font; //פונט

        /// <summary>
        /// פונקציה המקבלת אורך של כמות כפתורים ומייצרת תפריט
        /// </summary>
        /// <param name="buttonsLength"></param>
        public MainMenu(int buttonsLength) {
            Buttons = new Button[buttonsLength];
            Teams = new Button[3];
            BackgroundPosition = new Vector2(0, 0);
            MenuPlayer = new Player("Menu-Player");
        }

        /// <summary>
        /// עדכון התפריט
        /// </summary>
        public void Update() {
            MouseState mouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

            Vector2 direction = mousePosition - MenuPlayer.Position;
            direction.Normalize();

            MenuPlayer.Rotation = (float)Math.Atan2(
                          (double)direction.Y,
                          (double)direction.X);


        }

        /// <summary>
        /// טעינת התפריט
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            BackgroundTexture = content.Load<Texture2D>("images/main menu/background/main_menu");
            BackgroundTeamTexture = content.Load<Texture2D>("images/main menu/background/main_menu_team");
            BackgroundDownloadTexture = content.Load<Texture2D>("images/main menu/background/main_menu_download");

            Font = content.Load<SpriteFont>("fonts/big");

            MenuPlayer.LoadContent(content);
            MenuPlayer.SetNewPosition(new Vector2(635, 240));
            MenuPlayer.Visible = true;

            for (int i = 0; i < Buttons.Length; i++) {
                Buttons[i] = new Button(new Vector2(350, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 50 + i * 50), ((ButtonType)i));
                Buttons[i].LoadContent(content);
            }

            for (int i = 0; i < Teams.Length; i++) {
                Teams[i] = new Button(new Vector2(350, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 50 + (i * 65)), (ButtonType)(2 + i));
                Teams[i].LoadContent(content);
            }
        }

        /// <summary>
        /// ציור השחקן בתפריט
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMenuPlayer(SpriteBatch spriteBatch) {
            MenuPlayer.Draw(spriteBatch);
            spriteBatch.DrawString(Font, "Made by Idan Bachar.", new Vector2(0, SquadFighters.Graphics.PreferredBackBufferHeight - 50), Color.DarkGreen);
        }

        /// <summary>
        /// ציור הרקע של הטעינה מהשרת
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawDownloadBackground(SpriteBatch spriteBatch) {
            spriteBatch.Draw(BackgroundDownloadTexture, BackgroundPosition, Color.White);
            DrawMenuPlayer(spriteBatch);
        }


        /// <summary>
        /// ציור הרקע של הקבוצות
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawTeamBackground(SpriteBatch spriteBatch) {
            spriteBatch.Draw(BackgroundTeamTexture, BackgroundPosition, Color.White);
            DrawMenuPlayer(spriteBatch);
        }


        /// <summary>
        /// ציור הרקע של התפריט הראשי
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawBackground(SpriteBatch spriteBatch) {
            spriteBatch.Draw(BackgroundTexture, BackgroundPosition, Color.White);
            DrawMenuPlayer(spriteBatch);
        }
    }
}
