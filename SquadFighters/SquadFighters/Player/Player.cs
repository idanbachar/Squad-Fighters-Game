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
        public Texture2D DeadSignTexture;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 Speed;
        public Rectangle Rectangle;
        public List<Bullet> Bullets;
        public ShieldType ShieldType;
        public bool IsShield;
        public bool IsSwimming;
        public bool IsDead;
        public int ReviveMaxTime;
        public int ReviveTimer;
        public bool IsFinishedRevive;
        public bool IsReviving;
        public string OtherPlayerRevivingName;
        public string ReviveCountUpString;
        public Team Team;
        public bool Visible;
        public int Kills;
        public int Deaths;
        public int Level;
        public string KilledBy;

        public Player(string playerName)
        {
            Name = playerName;
            SetDefaultHealth();
            Rotation = 0;
            MaxBulletsCapacity = 30;
            BulletsCapacity = 0;
            Bullets = new List<Bullet>();
            IsShoot = false;
            IsShield = false;
            IsSwimming = false;
            IsDead = false;
            ReviveTimer = 0;
            IsFinishedRevive = false;
            ReviveMaxTime = 300;
            IsReviving = false;
            OtherPlayerRevivingName = "None";
            ReviveCountUpString = "0/0";
            Team = Team.Alpha;
            Visible = false;
            Kills = 0;
            Deaths = 0;
            Level = 0;
            KilledBy = "None";
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Texture = content.Load<Texture2D>("images/player/player");
            DeadSignTexture = content.Load<Texture2D>("images/player/player_dead_sign");
            ShieldType = ShieldType.None;
            SetDefaultPosition();
        }

        public void Update(Map map)
        {
            UpdateRectangle();
            CheckKeyboardMovement();
            CheckIsDead();
            IsSwimming = IsWaterIntersects(map.WaterObjects);
            CheckOutSideMap(map);
        }

        public void LevelUp()
        {
            Level++;
        }

        public void AddKill()
        {
            Kills++;
        }

        public void AddDeath()
        {
            Deaths++;
        }

        public void RevivePlayer()
        {
            if (ReviveTimer < ReviveMaxTime)
            {
                ReviveTimer++;
                IsFinishedRevive = false;
                IsReviving = true;
            }
            else
            {
                ReviveTimer = 0;
                IsFinishedRevive = true;
                IsReviving = false;
            }
        }

        public void ResetRevive()
        {
            ReviveTimer = 0;
            IsFinishedRevive = false;
            IsReviving = false;
        }

        public void CheckOutSideMap(Map map)
        {
            if (Position.X < 0)
                Rotation = 0.04f;

            if (Position.Y < 0)
                Rotation = 1.5f;

            if (Position.X > map.Width)
                Rotation = -3.15f;

            if (Position.Y > map.Height)
                Rotation = -1.6f;
        }

        public void CheckIsDead()
        {
            IsDead = Health <= 0;
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

            if (!IsDead)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    Speed = Direction;
                    Position += Speed;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Rotation += 0.07f;
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Rotation -= 0.07f;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
                Health = 100;

        }

        public void Shoot()
        {
            IsShoot = true;

            Bullet bullet = new Bullet(Position, Direction, Name);
            bullet.LoadContent(Content);
            Bullets.Add(bullet);

            BulletsCapacity--;
        }

        private void SetDefaultPosition()
        {
            Position = new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - Texture.Width / 2, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - Texture.Height / 2);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(Texture, Position, null,
                                                         IsSwimming ? Color.LightSkyBlue :
                                                         Team == Team.Alpha ? new Color(71, 252, 234) :
                                                         Team == Team.Beta ? Color.Yellow :
                                                         Team == Team.Omega ? Color.Pink : new Color(71, 252, 234),
                                                         Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                if (IsDead)
                    spriteBatch.Draw(DeadSignTexture, Position, null, Color.White, Rotation, new Vector2(DeadSignTexture.Width / 2, DeadSignTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            }
        }

        public override string ToString()
        {
            return ServerMethod.PlayerData.ToString() + "=true,PlayerName=" + Name + ",PlayerX=" + Position.X + ",PlayerY=" + Position.Y + ",PlayerRotation=" + Rotation + ",PlayerHealth=" + Health + ",PlayerIsShoot=" + IsShoot + ",PlayerDirectionX=" + Direction.X + ",PlayerDirectionY=" + Direction.Y + ",PlayerIsSwimming=" + IsSwimming + ",IsShield=" + IsShield + ",ShieldType=" + (int)ShieldType + ",PlayerBulletsCapacity=" + BulletsCapacity + ",PlayerIsDead=" + IsDead + ",PlayerIsReviving=" + IsReviving + ",RevivingPlayerName="  + OtherPlayerRevivingName + ",PlayerReviveCountUpString=" + ReviveCountUpString + ",PlayerTeam=" + (int)Team + ",PlayerVisible=" + Visible + ",";
        }
    }
}
