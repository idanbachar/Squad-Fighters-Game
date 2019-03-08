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

        public MainMenu(int buttonsLength)
        {
            Buttons = new Button[buttonsLength];
            Teams = new Button[3];
        }

        public void LoadContent(ContentManager content)
        {
            for(int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = new Button(new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 30 + i * 120), ((ButtonType)i));
                Buttons[i].LoadContent(content);
            }

            for(int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = new Button(new Vector2(50, SquadFighters.Graphics.PreferredBackBufferHeight / 2 + (i * 65)),  (ButtonType)(2 + i));
                Teams[i].LoadContent(content);
            }
        }
    }
}
