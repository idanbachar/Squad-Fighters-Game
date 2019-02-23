using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Battle_Royale_Project
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        public static Player Player;
        private Camera Camera;
        private Map Map;
        private HUD HUD;
        private Client PlayerClient;
        public static Dictionary<string, Player> Players;
        public static ContentManager ContentManager;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 500;
            Graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Players = new Dictionary<string, Player>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            ContentManager = Content;

            Player = new Player("idan" + new Random().Next(1000));
            Player.LoadContent(Content);

            PlayerClient = new Client("192.168.1.17", 7895, Player.Name);
            PlayerClient.ReceiveThread = new Thread(PlayerClient.ReceiveData);
            PlayerClient.ReceiveThread.Start();

            Thread.Sleep(100);
            PlayerClient.SendThread = new Thread(() => PlayerClient.SendData());
            PlayerClient.SendThread.Start();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new Camera(GraphicsDevice.Viewport);

            Map = new Map(new Rectangle(0, 0, 10000, 10000), Content);

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
            {
                PlayerClient.CloseConnection();
                Exit();
            }
            
            Camera.Focus(Player.Position, Map.Rectangle.Width, Map.Rectangle.Height);

            Player.Update(Map);


            foreach(KeyValuePair<string, Player> otherPlayer in Players)
            {
                for(int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                {
                    if (!otherPlayer.Value.Bullets[i].IsFinished)
                        otherPlayer.Value.Bullets[i].Update();
                    else
                        otherPlayer.Value.Bullets.RemoveAt(i);
                }
            }            

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

 
            foreach (KeyValuePair<string, Player> otherPlayer in Players)
            {
                otherPlayer.Value.Draw(spriteBatch);

                for (int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                {
                    otherPlayer.Value.Bullets[i].Draw(spriteBatch);
                }
            }

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
