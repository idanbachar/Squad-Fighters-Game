using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Battle_Royale_Project
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Camera camera;
        Map map;
        HUD HUD;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 700;
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

            player = new Player("idan");
            player.LoadContent(Content);

            camera = new Camera(GraphicsDevice.Viewport);
            map = new Map(new Rectangle(0, 0, 3000, 3000), Content);

            HUD = new HUD();
            HUD.LoadContent(Content);

            Random rndItem = new Random();
            for (int i = 0; i < 100; i++)
                map.AddItem((ItemType)rndItem.Next(2, 13));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            camera.Focus(player.Position, 3000, 3000);

            player.Update();
            HUD.Update(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            

            foreach (Item item in map.Items)
                item.Draw(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            HUD.Draw(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
