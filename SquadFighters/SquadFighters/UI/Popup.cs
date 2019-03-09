using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class Popup
    {
        private SpriteFont Font;
        public Vector2 Position;
        public string Text;
        public bool IsShowing;
        public bool IsMove;
        private int timeLimiter;
        private int timer;

        public Popup(string text, Vector2 position, bool isMove)
        {
            Text = text;
            Position = new Vector2(position.X, position.Y);
            IsShowing = true;
            timeLimiter = 300;
            timer = 0;
            IsMove = isMove;
            LoadContent();
        }

        public void LoadContent()
        {
            Font = SquadFighters.ContentManager.Load<SpriteFont>("fonts/items_capacity_font");
        }

        public void Update()
        {
            if (timer < timeLimiter)
            {
                timer++;
                if (IsMove)
                {
                    Move();
                }
            }
            else
            {
                timer = 0;
                IsShowing = false;
            }
        }

        private void Move()
        {
            Position.Y -= 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color.Black);
        }
    }
}
