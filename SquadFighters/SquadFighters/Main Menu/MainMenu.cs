using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class MainMenu
    {
        public Button[] Buttons;
        public Button[] Teams;
        public Texture2D BackgroundTexture;
        public Texture2D BackgroundTeamTexture;
        public Texture2D BackgroundDownloadTexture;
        public Vector2 BackgroundPosition;

        public MainMenu(int buttonsLength)
        {
            Buttons = new Button[buttonsLength];
            Teams = new Button[3];
            BackgroundPosition = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager content)
        {
            BackgroundTexture = content.Load<Texture2D>("images/main menu/background/main_menu_background");
            BackgroundTeamTexture = content.Load<Texture2D>("images/main menu/background/main_menu_background_team");
            BackgroundDownloadTexture = content.Load<Texture2D>("images/main menu/background/main_menu_background_download");

            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = new Button(new Vector2(50, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 50 + i * 50), ((ButtonType)i));
                Buttons[i].LoadContent(content);
            }

            for(int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = new Button(new Vector2(50, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 50 + (i * 65)),  (ButtonType)(2 + i));
                Teams[i].LoadContent(content);
            }
        }

        public void DrawDownloadBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundDownloadTexture, BackgroundPosition, Color.White);
        }

        public void DrawTeamBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTeamTexture, BackgroundPosition, Color.White);
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTexture, BackgroundPosition, Color.White);
        }
    }
}
