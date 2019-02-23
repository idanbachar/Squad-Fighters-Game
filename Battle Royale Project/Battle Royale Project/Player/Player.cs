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
        private ContentManager Content;

        public string Name;
        public int Health;
        public int MaxBulletsCapacity;
        public int BulletsCapacity;
        public float Rotation;
        public bool IsShoot;
        public Texture2D Texture;
        public Vector2 Position;
        private Vector2 Direction;
        public Vector2 Speed;
        public Rectangle Rectangle;
        public List<Bullet> Bullets;

        public Player(string playerName)
        {
            Name = playerName;
            SetDefaultHealth();
            SetDefaultPosition();
            Rotation = 0;
            MaxBulletsCapacity = 500;
            BulletsCapacity = 500;
            Bullets = new List<Bullet>();
            IsShoot = false;
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Texture = content.Load<Texture2D>("images/player/player");
        }

        public void Update(Map map)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            CheckKeyboardMovement();
            CheckItemsIntersects(map.Items);
        }

        public void CheckItemsIntersects(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(Rectangle.Intersects(items[i].Rectangle))
                {
                    if (items[i] is Food)
                    {
                        if (Health < 100)
                        {
                            Health += ((Food)(items[i])).GetHealth();
                            items.RemoveAt(i);
                        }
                    }
                    else if (items[i] is GunAmmo)
                    {
                        int capacity = ((GunAmmo)(items[i])).Capacity;

                        if (BulletsCapacity + capacity <= MaxBulletsCapacity)
                        {
                            BulletsCapacity += ((GunAmmo)(items[i])).Capacity;
                            items.RemoveAt(i);
                        }
                        else
                        {
                            if (BulletsCapacity + capacity > MaxBulletsCapacity)
                            {
                                ((GunAmmo)(items[i])).Capacity -= MaxBulletsCapacity - BulletsCapacity;
                                BulletsCapacity = MaxBulletsCapacity;
                            }
                        }
                    }
                    else if (items[i] is Shield)
                    {
                        items.RemoveAt(i);
                    }
                    else if (items[i] is Helmet)
                    {
                        items.RemoveAt(i);
                    }
                }
            }
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

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !IsShoot)
            {
                if (BulletsCapacity > 0)
                    Shoot();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                IsShoot = false;
            }
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
            Position = new Vector2(new Random().Next(0, Map.Rectangle.Width - 100), new Random().Next(0, Map.Rectangle.Height - 100));
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
