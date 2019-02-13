using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Battle_Royale_Project
{
    public class Player
    {
        public int Health;
        public string Name;
        public Texture2D Texture;
        public Vector2 Position;
        public float Speed;
        

        public Player(string playerName)
        {
            Name = playerName;
            SetDefaultHealth();
            SetDefaultPosition();
            Speed = 2f;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/player/player");
        }

        private void SetDefaultHealth()
        {
            Health = 100;
        }

        public void CheckMovement()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Position.X += Speed;
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                Position.X -= Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Position.Y -= Speed;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                Position.Y += Speed;
        }

        private void SetDefaultPosition()
        {
            Position = new Vector2(100, 100);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
