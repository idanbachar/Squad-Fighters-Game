using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public class Bullet
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;
        public Vector2 Direction;
        public float Speed;
        public bool IsFinished;
        private int Timer;
        private int MaxTimer;
        public int Damage;

        public Bullet(Vector2 position, Vector2 direction)
        {
            Position = new Vector2(position.X, position.Y);
            Direction = new Vector2(direction.X, direction.Y);
            Rectangle = new Rectangle((int)position.X,(int)position.Y, 0, 0);
            Speed = 3.3f;
            IsFinished = false;
            Timer = 0;
            MaxTimer = 40;
            Damage = 5;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/player/bullets/bullet");
            Rectangle.Width = Texture.Width;
            Rectangle.Height = Texture.Height;
        }

        public void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            Move();
        }

        public void Move()
        {
            Position += Direction * Speed;

            if (Timer <= MaxTimer)
                Timer++;
            else
                IsFinished = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
