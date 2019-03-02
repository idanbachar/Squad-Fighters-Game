using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SquadFighters
{
    public class Player
    {
        private ContentManager Content;

        public string Name;
        public int Health;
        public int MaxBulletsCapacity;
        public int BulletsCapacity;
        public float Rotation;
        public bool IsShoot;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 Speed;
        public Rectangle Rectangle;
        public List<Bullet> Bullets;
        public ShieldType ShieldType;
        public bool IsShield;
        public bool IsSwimming;

        public Player(string playerName)
        {
            Name = playerName;
            SetDefaultHealth();
            SetDefaultPosition();
            Rotation = 0;
            MaxBulletsCapacity = 30;
            BulletsCapacity = 0;
            Bullets = new List<Bullet>();
            IsShoot = false;
            IsShield = false;
            IsSwimming = false;
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Texture = content.Load<Texture2D>("images/player/player");
            ShieldType = ShieldType.None;
        }

        public void Update(Map map)
        {
            UpdateRectangle();
            CheckKeyboardMovement();
            IsSwimming = IsWaterIntersects(map.WaterObjects);
        }

        public void UpdateRectangle()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Heal(int heal)
        {
            Health = (Health + heal) > 100 ? 100 : Health += heal;
        }

        public void Hit(int damage)
        {
            Health = (Health - damage) < 0 ? 0 : Health -= damage;
        }

        private void SetDefaultHealth()
        {
            Health = 100;
        }

        public bool IsWaterIntersects(List<Water> waterObjects)
        {
            foreach (Water water in waterObjects)
            {
                if (Rectangle.Intersects(water.Rectangle))
                    return true;
            }

            return false;
        }

        public void CheckKeyboardMovement()
        {
            Direction = new Vector2((float)Math.Cos(Rotation) * 3f, (float)Math.Sin(Rotation) * 3f);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Speed = Direction;
                Position += Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Rotation += 0.05f;
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                Rotation -= 0.05f;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !IsShoot)
            {
                if (BulletsCapacity > 0)
                    Shoot();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                IsShoot = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Hit(5);
        }

        public void Shoot()
        {
            IsShoot = true;

            Bullet bullet = new Bullet(Position, Direction);
            bullet.LoadContent(Content);
            Bullets.Add(bullet);

            BulletsCapacity--;
        }

        private void SetDefaultPosition()
        {
            Position = new Vector2(100, 100);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, IsSwimming ? Color.LightSkyBlue :Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
        }

        public override string ToString()
        {
            return "PlayerName=" + Name + ",PlayerX=" + Position.X + ",PlayerY=" + Position.Y + ",PlayerRotation=" + Rotation + ",PlayerHealth=" + Health + ",PlayerIsShoot=" + IsShoot + ",PlayerDirectionX=" + Direction.X + ",PlayerDirectionY=" + Direction.Y + ",PlayerIsSwimming=" + IsSwimming + ",IsShield=" + IsShield + ",ShieldType=" + (int)ShieldType + ",PlayerBulletsCapacity=" + BulletsCapacity + ",";
        }
    }
}
