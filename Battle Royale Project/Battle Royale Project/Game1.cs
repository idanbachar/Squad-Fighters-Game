using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Battle_Royale_Project
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        private Player Player;
        private Camera Camera;
        private Map Map;
        private HUD HUD;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 500;
            Graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new Camera(GraphicsDevice.Viewport);

            Map = new Map(new Rectangle(0, 0, 10000, 10000), Content);

            Player = new Player("idan");
            Player.LoadContent(Content);

            HUD = new HUD();
            HUD.LoadContent(Content);

            Random rndItem = new Random();
            for (int i = 0; i < Map.Rectangle.Width / 20; i++)
                Map.AddItem((ItemCategory)rndItem.Next(4));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Camera.Focus(Player.Position, Map.Rectangle.Width, Map.Rectangle.Height);

            Player.Update(Map);

            for (int i = 0; i < Player.Bullets.Count; i++)
            {
                if (!Player.Bullets[i].IsFinished)
                    Player.Bullets[i].Update();
                else
                    Player.Bullets.RemoveAt(i);
            }

            foreach (Item item in Map.Items)
                item.Update();

            HUD.Update(Player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            foreach (Item item in Map.Items)
            {
                item.Draw(spriteBatch);
                spriteBatch.DrawString(HUD.ItemsCapacityFont, item.ToString(), new Vector2(item.Position.X + 15, item.Position.Y - 30) ,Color.Black);
            }

            foreach (Bullet bullet in Player.Bullets)
                bullet.Draw(spriteBatch);

            Player.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            HUD.Draw(spriteBatch);

            if (Player.IsShield)
            {
                foreach (ShieldBar shieldbar in Player.Shield.ShieldBars)
                    shieldbar.Draw(spriteBatch);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
