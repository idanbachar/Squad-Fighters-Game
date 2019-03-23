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
        public Texture2D CoinTexture;
        public Texture2D DeadSignTexture;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Rectangle Rectangle;
        public List<Bullet> Bullets;
        public ShieldType ShieldType;
        public bool IsShield;
        public bool IsSwimming;
        public bool IsDead;
        public int ReviveMaxTime;
        public int ReviveTimer;
        public bool IsFinishedRevive;
        public bool IsAbleToBeRevived;
        public bool IsReviving;
        public string OtherPlayerRevivingName;
        public string ReviveCountUpString;
        public Team Team;
        public bool Visible;
        public int Kills;
        public int Deaths;
        public int Level;
        public string KilledBy;
        public bool IsDrown;
        public int CoinsCarrying;
        public bool IsCarryingCoins;
        public bool Cheats;

        public Player(string playerName)
        {
            Cheats = false;
            Name = playerName;
            SetDefaultHealth();
            Rotation = 0;
            MaxBulletsCapacity = Cheats ? 999 : 30;
            BulletsCapacity = Cheats ? 999 : 0;
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
            IsAbleToBeRevived = true;
            IsDrown = false;
            CoinsCarrying = 0;
            IsCarryingCoins = false;
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Texture = content.Load<Texture2D>("images/player/player");
            CoinTexture = content.Load<Texture2D>("images/items/coins/ib");
            DeadSignTexture = content.Load<Texture2D>("images/player/player_dead_sign");
            ShieldType = ShieldType.None;
            SetDefaultPosition();
        }

        public void SetNormalSpeed()
        {
            Speed = 3.5f;
        }

        public void SetWaterSpeed()
        {
            Speed = 2f;
        }

        public void Update(Map map)
        {

            UpdateRectangle();
            CheckKeyboardMovement();
            CheckIsDead();
            IsSwimming = IsWaterIntersects(map.WaterObjects);
            IsCarryingCoins = CoinsCarrying > 0;

            if (IsSwimming)
            {
                SetWaterSpeed();
            }
            else
            {
                SetNormalSpeed();
            }

            CheckOutSideMap(map);

            Direction = new Vector2((float)Math.Cos(Rotation) * Speed, (float)Math.Sin(Rotation) * Speed);
        }

        public void LevelUp()
        {
            Level++;
        }

        public void AddCoin()
        {
            CoinsCarrying++;
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
            if (!IsDead)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    Position += Direction;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Rotation += 0.07f;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Rotation -= 0.07f;

            }

            if (Cheats)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                    Health = 100;

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    Team = Team.Alpha;
                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    Team = Team.Beta;
                if (Keyboard.GetState().IsKeyDown(Keys.D3))
                    Team = Team.Omega;
            }
        }

        public void Shoot()
        {
            IsShoot = true;

            Bullet bullet = new Bullet(Position, Direction, Name);
            bullet.LoadContent(Content);
            Bullets.Add(bullet);

            BulletsCapacity--;
        }

        public void SpawnOnTeamSpawner(TeamSpawner alphaSpawner, TeamSpawner betaSpawner, TeamSpawner omegaSpawner)
        {
            switch (Team)
            {
                case Team.Alpha:
                    SetNewPosition(new Vector2(alphaSpawner.Position.X + 100, alphaSpawner.Position.Y + 100));
                    break;
                case Team.Beta:
                    SetNewPosition(new Vector2(betaSpawner.Position.X + 100, betaSpawner.Position.Y + 100));
                    break;
                case Team.Omega:
                    SetNewPosition(new Vector2(omegaSpawner.Position.X + 100, omegaSpawner.Position.Y + 100));
                    break;
            }
        }

        private void SetDefaultPosition()
        {
            Position = new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - Texture.Width / 2, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - Texture.Height / 2);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        public void SetNewPosition(Vector2 newPosition)
        {
            Position = new Vector2(newPosition.X, newPosition.Y);
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

                if (IsCarryingCoins)
                    spriteBatch.Draw(CoinTexture, Position, null, Color.White, Rotation, new Vector2(CoinTexture.Width / 2, CoinTexture.Height / 2), 1.0f, SpriteEffects.None, 1);


                if (IsDead)
                    spriteBatch.Draw(DeadSignTexture, Position, null, Color.White, Rotation, new Vector2(DeadSignTexture.Width / 2, DeadSignTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            }
        }

        public override string ToString()
        {
            return ServerMethod.PlayerData.ToString() + "=true,PlayerName=" + Name + ",PlayerX=" + Position.X + ",PlayerY=" + Position.Y + ",PlayerRotation=" + Rotation + ",PlayerHealth=" + Health + ",PlayerIsShoot=" + IsShoot + ",PlayerDirectionX=" + Direction.X + ",PlayerDirectionY=" + Direction.Y + ",PlayerIsSwimming=" + IsSwimming + ",IsShield=" + IsShield + ",ShieldType=" + (int)ShieldType + ",PlayerBulletsCapacity=" + BulletsCapacity + ",PlayerIsDead=" + IsDead + ",PlayerIsReviving=" + IsReviving + ",RevivingPlayerName=" + OtherPlayerRevivingName + ",PlayerReviveCountUpString=" + ReviveCountUpString + ",PlayerTeam=" + (int)Team + ",PlayerVisible=" + Visible + ",PlayerIsAbleToBeRevived=" + IsAbleToBeRevived + ",PlayerIsDrown=" + IsDrown + ",PlayerIsCarryingCoins=" + IsCarryingCoins + ",";
        }
    }
}
