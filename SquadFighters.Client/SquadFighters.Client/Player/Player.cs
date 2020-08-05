using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SquadFighters.Client {
    public class Player {
        private ContentManager Content; //משתנה טעינה

        public string Name; //שם השחקן
        public int Health; //בריאות השחקן
        public int MaxBulletsCapacity; //כמות כדורים מקסימלית
        public int BulletsCapacity; //כמות כדורים נוכחית
        public float Rotation; //זווית
        public bool IsShoot; //אינדיקציה של האם ירה כדור
        public Texture2D Texture; //טקסטורת שחקן
        public Texture2D CoinTexture; //טקסטורת מטבע כשעל שחקן
        public Texture2D DeadSignTexture; //טקסטורת שחקן מת
        public Vector2 Position; //מיקום השחקן
        public Vector2 Direction; //כיוון השחקן
        public float Speed; //מהירות השחקן
        public Rectangle Rectangle; //מלבן השחקן
        public List<Bullet> Bullets; //רשימת כדורים של השחקן
        public ShieldType ShieldType; //סוג מגן של השחקן
        public bool IsShield; //אינדיקציית האם לשחקן יש מגן
        public bool IsSwimming; //אינדיקציית האם השחקן שוחה
        public bool IsDead; //אינדיקציית האם השחקן מת
        public int ReviveMaxTime; //כמות החייאה מקסימלית
        public int ReviveTimer; //טיימר החייאת שחקן
        public bool IsFinishedRevive; //אינדיקציית האם הסתיימה החייאה
        public bool IsAbleToBeRevived; //אינדיקציית האם יכולים להחיות את השחקן
        public bool IsReviving; //אינדיקציית האם השחקן עושה החייאה לשחקן אחר
        public string OtherPlayerRevivingName; //שם השחקן האחר שעושה החייאה
        public string ReviveCountUpString; //טקסט של החייאה
        public Team Team; //קבוצת השחקן
        public bool Visible; //האם השחקן נראה
        public int Kills; //כמות הריגות של השחקן
        public int Deaths; //כמות מוות של השחקן
        public int Level; //רמת השחקן
        public string KilledBy; //טקסט השחקן נהרג על ידי
        public bool IsDrown; //אינדיקציית האם השחקן טבע למוות
        public int CoinsCarrying; //כמות מטבעות שהשחקן סוחב כרגע
        public bool IsCarryingCoins; //אינדיקציית האם השחקן סוחב כרגע מטבעות
        public bool Cheats; //אינדיקציית צ'יטים

        /// <summary>
        /// פונקציה המקבלת שם ומייצרת שחקן
        /// </summary>
        /// <param name="playerName"></param>
        public Player(string playerName) {
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

        /// <summary>
        /// טעינת שחקן
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Content = content;
            Texture = content.Load<Texture2D>("images/player/player");
            CoinTexture = content.Load<Texture2D>("images/items/coins/ib");
            DeadSignTexture = content.Load<Texture2D>("images/player/player_dead_sign");
            ShieldType = ShieldType.None;
            SetDefaultPosition();
        }

        /// <summary>
        /// עדכון מהירות נורמלי
        /// </summary>
        public void SetNormalSpeed() {
            Speed = 3.5f;
        }

        /// <summary>
        /// עדכון מהירות במים
        /// </summary>
        public void SetWaterSpeed() {
            Speed = 2f;
        }
        
        /// <summary>
        /// פונקציה המקבלת מפה ומעדכנת את השחקן בהתאם
        /// </summary>
        /// <param name="map"></param>
        public void Update(Map map) {

            UpdateRectangle();
            CheckKeyboardMovement();
            CheckIsDead();
            IsSwimming = IsWaterIntersects(map.WaterObjects);
            IsCarryingCoins = CoinsCarrying > 0;

            if (IsSwimming) {
                SetWaterSpeed();
            }
            else {
                SetNormalSpeed();
            }

            CheckOutSideMap(map);

            Direction = new Vector2((float)Math.Cos(Rotation) * Speed, (float)Math.Sin(Rotation) * Speed);
        }

        /// <summary>
        /// פונקציה להעלאת רמה
        /// </summary>
        public void LevelUp() {
            Level++;
        }

        /// <summary>
        /// פונקציה להוספת מטבע
        /// </summary>
        public void AddCoin() {
            CoinsCarrying++;
        }

        /// <summary>
        /// פונקציה להוספת הריגה
        /// </summary>
        public void AddKill() {
            Kills++;
        }

        /// <summary>
        /// פונקציה להוספת מוות
        /// </summary>
        public void AddDeath() {
            Deaths++;
        }

        /// <summary>
        /// פונקציה להחייאת שחקן
        /// </summary>
        public void RevivePlayer() {
            if (ReviveTimer < ReviveMaxTime) {
                ReviveTimer++;
                IsFinishedRevive = false;
                IsReviving = true;
            }
            else {
                ReviveTimer = 0;
                IsFinishedRevive = true;
                IsReviving = false;
            }
        }

        /// <summary>
        /// פונקציה לאיפוס החייאה
        /// </summary>
        public void ResetRevive() {
            ReviveTimer = 0;
            IsFinishedRevive = false;
            IsReviving = false;
        }

        /// <summary>
        /// פונקציה המקבלת מפה ובודקת שהשחקן לא חרג מגבולותיה
        /// </summary>
        /// <param name="map"></param>
        public void CheckOutSideMap(Map map) {
            if (Position.X < 0)
                Rotation = 0.04f;

            if (Position.Y < 0)
                Rotation = 1.5f;

            if (Position.X > map.Width)
                Rotation = -3.15f;

            if (Position.Y > map.Height)
                Rotation = -1.6f;
        }

        /// <summary>
        /// פונקציה הבודקת אם השחקן מת ומעדכנת בהתאם
        /// </summary>
        public void CheckIsDead() {
            IsDead = Health <= 0;
        }

        /// <summary>
        /// פונקציה המעדכנת את מיקום המלבן של השחקן
        /// </summary>
        public void UpdateRectangle() {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// פונקציה המקבלת בריאות ומעדכנת בהתאם
        /// </summary>
        /// <param name="heal"></param>
        public void Heal(int heal) {
            Health = (Health + heal) > 100 ? 100 : Health += heal;
        }

        /// <summary>
        /// פונקציה המקבלת כמות פגיעה ומעדכנת בהתאם את בריאות השחקן
        /// </summary>
        /// <param name="damage"></param>
        public void Hit(int damage) {
            Health = (Health - damage) < 0 ? 0 : Health -= damage;
        }

        /// <summary>
        /// פונקציה המעדכנת את הבריאות הדיפולטית של השחקן
        /// </summary>
        private void SetDefaultHealth() {
            Health = 100;
        }

        /// <summary>
        /// פונקציה המקבלת רשימה של מים ובודקת אם השחקן בתוכן, מחזירה אמת אם כן ושקר אחרת
        /// </summary>
        /// <param name="waterObjects"></param>
        /// <returns></returns>
        public bool IsWaterIntersects(List<Water> waterObjects) {
            foreach (Water water in waterObjects) {
                if (Rectangle.Intersects(water.Rectangle))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// בדיקת הקשות במקלדת
        /// </summary>
        public void CheckKeyboardMovement() {
            if (!IsDead) {
                if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                    Position += Direction;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Rotation += 0.07f;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Rotation -= 0.07f;

            }

            if (Cheats) {

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

        /// <summary>
        /// פונקציה לירייה
        /// </summary>
        public void Shoot() {
            IsShoot = true;

            Bullet bullet = new Bullet(Position, Direction, Name);
            bullet.LoadContent(Content);
            Bullets.Add(bullet);

            BulletsCapacity--;
        }

        /// <summary>
        /// פונקציה המקבלת את 3 הקבוצות ובהתאם לקבוצת השחקן משגרת אותו לבסיס המתאים
        /// </summary>
        /// <param name="alphaSpawner"></param>
        /// <param name="betaSpawner"></param>
        /// <param name="omegaSpawner"></param>
        public void SpawnOnTeamSpawner(TeamSpawner alphaSpawner, TeamSpawner betaSpawner, TeamSpawner omegaSpawner) {
            switch (Team) {
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

        /// <summary>
        /// פונקציה המאפסת את מיקום השחקן למקום הדיפולטי
        /// </summary>
        private void SetDefaultPosition() {
            Position = new Vector2(SquadFighters.Graphics.PreferredBackBufferWidth / 2 - Texture.Width / 2, SquadFighters.Graphics.PreferredBackBufferHeight / 2 - Texture.Height / 2);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        /// <summary>
        /// פונקציה המקבלת מיקום ומעדכנת את מיקום השחקן
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetNewPosition(Vector2 newPosition) {
            Position = new Vector2(newPosition.X, newPosition.Y);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        }

        /// <summary>
        /// ציור השחקן
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            if (Visible) {
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

        /// <summary>
        /// יצירת פורמט טקסט
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return ServerMethod.PlayerData.ToString() + "=true,PlayerName=" + Name + ",PlayerX=" + Position.X + ",PlayerY=" + Position.Y + ",PlayerRotation=" + Rotation + ",PlayerHealth=" + Health + ",PlayerIsShoot=" + IsShoot + ",PlayerDirectionX=" + Direction.X + ",PlayerDirectionY=" + Direction.Y + ",PlayerIsSwimming=" + IsSwimming + ",IsShield=" + IsShield + ",ShieldType=" + (int)ShieldType + ",PlayerBulletsCapacity=" + BulletsCapacity + ",PlayerIsDead=" + IsDead + ",PlayerIsReviving=" + IsReviving + ",RevivingPlayerName=" + OtherPlayerRevivingName + ",PlayerReviveCountUpString=" + ReviveCountUpString + ",PlayerTeam=" + (int)Team + ",PlayerVisible=" + Visible + ",PlayerIsAbleToBeRevived=" + IsAbleToBeRevived + ",PlayerIsDrown=" + IsDrown + ",PlayerIsCarryingCoins=" + IsCarryingCoins + ",";
        }
    }
}
