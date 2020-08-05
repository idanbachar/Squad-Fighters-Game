using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters.Client {
    public class Map {

        public static Rectangle Rectangle; //מימדי המפה
        public Background Background; //רקע המפה
        public Dictionary<string, Item> Items; //פריטים במפה
        public List<Water> WaterObjects; //מים במפה

        public TeamSpawner AlphaTeamSpawner; //בסיס קבוצת אלפא
        public TeamSpawner BetaTeamSpawner; //בסיס קבוצת בטא
        public TeamSpawner OmegaTeamSpawner; //בסיס קבוצת אומגה

        private Random Random; //רנדום
        public int Width; //רוחב
        public int Height; //גובה

        /// <summary>
        /// פונקציה המקבלת מימדים ומייצרת מפה
        /// </summary>
        /// <param name="mapRectangle"></param>
        public Map(Rectangle mapRectangle) {
            Background = new Background(new Vector2(0, 0));

            Rectangle = new Rectangle(mapRectangle.X,
                                      mapRectangle.Y,
                                      mapRectangle.Width,
                                      mapRectangle.Height);

            Random = new Random();

            Width = Rectangle.Width;
            Height = Rectangle.Height;

            Items = new Dictionary<string, Item>();
            WaterObjects = new List<Water>();

            AlphaTeamSpawner = new TeamSpawner(new Vector2(100, 100), Team.Alpha);
            BetaTeamSpawner = new TeamSpawner(new Vector2(4000, 950), Team.Beta);
            OmegaTeamSpawner = new TeamSpawner(new Vector2(1800, 3900), Team.Omega);

        }

        /// <summary>
        /// טעינת מפה
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            Background.LoadContent(content);
            GenerateWaterObjects(content);

            AlphaTeamSpawner.LoadContent(content);
            BetaTeamSpawner.LoadContent(content);
            OmegaTeamSpawner.LoadContent(content);
        }

        /// <summary>
        /// פונקציה המקבלת סוג פריט, כמות, מיקומים ומייצרת פריט
        /// </summary>
        /// <param name="itemCategory"></param>
        /// <param name="itemType"></param>
        /// <param name="itemX"></param>
        /// <param name="itemY"></param>
        /// <param name="itemCapacity"></param>
        /// <param name="itemKey"></param>
        public void AddItem(ItemCategory itemCategory, int itemType, float itemX, float itemY, int itemCapacity, string itemKey) {
            Item item = new GunAmmo(new Vector2(0, 0), AmmoType.Bullet, 20);

            switch (itemCategory) {
                case ItemCategory.Ammo:
                    item = new GunAmmo(new Vector2(itemX, itemY), (AmmoType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Food:
                    item = new Food(new Vector2(itemX, itemY), (FoodType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Shield:
                    item = new Shield(new Vector2(itemX, itemY), (ShieldType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Helmet:
                    item = new Helmet(new Vector2(itemX, itemY), (HelmetType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
                case ItemCategory.Coin:
                    item = new Coin(new Vector2(itemX, itemY), (CoinType)itemType, itemCapacity);
                    item.LoadContent(SquadFighters.ContentManager);
                    break;
            }

            Items.Add(itemKey, item);
        }

        /// <summary>
        /// פונקציה המייצרת מים במפה
        /// </summary>
        /// <param name="content"></param>
        public void GenerateWaterObjects(ContentManager content) {
            Water water1 = new Water(new Vector2(500, 600), WaterShape.Medium);
            water1.LoadContent(content);

            Water water2 = new Water(new Vector2(1200, 1200), WaterShape.Rectangle);
            water2.LoadContent(content);

            Water water3 = new Water(new Vector2(1000, 3000), WaterShape.Tall);
            water3.LoadContent(content);

            Water water4 = new Water(new Vector2(3500, 900), WaterShape.Tall);
            water4.LoadContent(content);

            Water water5 = new Water(new Vector2(2200, 4200), WaterShape.Rectangle);
            water5.LoadContent(content);


            WaterObjects.Add(water1);
            WaterObjects.Add(water2);
            WaterObjects.Add(water3);
            WaterObjects.Add(water4);
            WaterObjects.Add(water5);
        }

        /// <summary>
        /// ציור המפה
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            Background.Draw(spriteBatch);

            foreach (Water water in WaterObjects)
                water.Draw(spriteBatch);
        }

        /// <summary>
        /// פונקציה המייצרת מיקומים רנדומלים
        /// </summary>
        /// <returns></returns>
        public Vector2 GeneratePosition() {
            return new Vector2(Random.Next(200, Rectangle.Width - 200), Random.Next(200, Rectangle.Height - 200));
        }
    }
}
