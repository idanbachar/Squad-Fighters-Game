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
        public int bullets;
        public string Name;
        public Texture2D Texture;
        public Vector2 Position;
        private Vector2 Direction;
        public Vector2 Speed;
        public float Rotation;

        public Player(string playerName)
        {
            Name = playerName;
            SetDefaultHealth();
            SetDefaultPosition();
            Rotation = 0;
            bullets = 100;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/player/player");
        }

        public void Update()
        {
            CheckKeyboardMovement();

        }

        private void SetDefaultHealth()
        {
            Health = 100;
        }

        public void CheckKeyboardMovement()
        {

           Direction = new Vector2((float)Math.Cos(Rotation) * 5f, (float)Math.Sin(Rotation) * 5f);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Speed = Direction;
                Position += Speed;
            }
            

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Rotation += 0.05f;
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                Rotation -= 0.05f;

        }

        private void SetDefaultPosition()
        {
            Position = new Vector2(100, 100);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
