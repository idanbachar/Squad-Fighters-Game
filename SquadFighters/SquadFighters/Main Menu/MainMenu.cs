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

        public MainMenu(int buttonsLength)
        {
            Buttons = new Button[buttonsLength];
        }

        public void LoadContent(ContentManager content)
        {
            for(int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = new Button(new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - 100, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - 30 + i * 120), ((ButtonType)i));
                Buttons[i].LoadContent(content);
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
