using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace SquadFighters
{
    public class SquadFighters : Game
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

        public SquadFighters()
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

            Map = new Map(new Rectangle(0, 0, 10000, 10000), Content);

            PlayerClient = new Client("192.168.1.17", 7895, Player.Name);
            PlayerClient.ReceiveThread = new Thread(ReceiveData);
            PlayerClient.ReceiveThread.Start();

            Thread.Sleep(100);
            PlayerClient.SendThread = new Thread(() => PlayerClient.SendData());
            PlayerClient.SendThread.Start();
        }

        public void AddPlayer(string CurrentConnectedPlayerName)
        {
            Player player = new Player(CurrentConnectedPlayerName);
            player.LoadContent(SquadFighters.ContentManager);
            SquadFighters.Players.Add(CurrentConnectedPlayerName, player);
        }

        public void ReceiveData()
        {
            while (true)
            {
                try
                {
                    NetworkStream netStream = PlayerClient.client.GetStream();
                    byte[] bytes = new byte[10024];
                    netStream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.ASCII.GetString(bytes);
                    string ReceivedDataString = data.Substring(0, data.IndexOf("\0"));
                    PlayerClient.ReceivedDataArray = ReceivedDataString.Split(',');

                    if (ReceivedDataString.Contains("Connected"))
                    {
                        string CurrentConnectedPlayerName = ReceivedDataString.Split(',')[0];

                        if (CurrentConnectedPlayerName != SquadFighters.Player.Name)
                        {
                            AddPlayer(CurrentConnectedPlayerName);
                        }
                    }
                    else if (ReceivedDataString.Contains("PlayerName"))
                    {
                        //new Random().Next(0, Map.Rectangle.Width - 100), new Random().Next(0, Map.Rectangle.Height - 100)

                        string playerName = PlayerClient.ReceivedDataArray[0].Split('=')[1];
                        float playerX = float.Parse(PlayerClient.ReceivedDataArray[1].Split('=')[1]);
                        float playerY = float.Parse(PlayerClient.ReceivedDataArray[2].Split('=')[1]);
                        float playerRotation = float.Parse(PlayerClient.ReceivedDataArray[3].Split('=')[1]);
                        int playerHealth = int.Parse(PlayerClient.ReceivedDataArray[4].Split('=')[1]);
                        bool playerIsShoot = bool.Parse(PlayerClient.ReceivedDataArray[5].Split('=')[1]);
                        float playerDirectionX = float.Parse(PlayerClient.ReceivedDataArray[6].Split('=')[1]);
                        float playerDirectionY = float.Parse(PlayerClient.ReceivedDataArray[7].Split('=')[1]);

                        if (Players.ContainsKey(playerName))
                        {
                            Players[playerName].Name = playerName;
                            Players[playerName].Position.X = playerX;
                            Players[playerName].Position.Y = playerY;
                            Players[playerName].Rotation = playerRotation;
                            Players[playerName].Health = playerHealth;
                            Players[playerName].IsShoot = playerIsShoot;
                            Players[playerName].Direction.X = playerDirectionX;
                            Players[playerName].Direction.Y = playerDirectionY;

                            if (playerIsShoot)
                            {
                                Players[playerName].Shoot();
                            }
                        }
                        else
                        {
                            AddPlayer(playerName);
                        }

                    }
                    else if (ReceivedDataString.Contains("AddItem=true"))
                    {
                        ItemCategory ItemCategory = (ItemCategory)int.Parse(PlayerClient.ReceivedDataArray[1].Split('=')[1]);
                        int type = int.Parse(PlayerClient.ReceivedDataArray[2].Split('=')[1].ToString());
                        float itemX = float.Parse(PlayerClient.ReceivedDataArray[3].Split('=')[1].ToString());
                        float itemY = float.Parse(PlayerClient.ReceivedDataArray[4].Split('=')[1].ToString());
                        Map.GenerateItem(ItemCategory, type, itemX, itemY);

                    }

                    Console.WriteLine(ReceivedDataString);

                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new Camera(GraphicsDevice.Viewport);

            HUD = new HUD();
            HUD.LoadContent(Content);

            Random rndItem = new Random();
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

            //foreach (Item item in Map.Items)
            //{

            //    item.Draw(spriteBatch);
            //    spriteBatch.DrawString(HUD.ItemsCapacityFont, item.ToString(), new Vector2(item.Position.X + 15, item.Position.Y - 30), Color.Black);

            //}


            for(int i = 0; i < Map.Items.Count; i++)
            {

                Map.Items[i].Draw(spriteBatch);
                spriteBatch.DrawString(HUD.ItemsCapacityFont, Map.Items[i].ToString(), new Vector2(Map.Items[i].Position.X + 15, Map.Items[i].Position.Y - 30), Color.Black);

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
