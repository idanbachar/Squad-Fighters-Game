using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

namespace SquadFighters
{
    public class SquadFighters : Game
    {
        public static GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        public static Player Player;
        private Camera Camera;
        private MainMenu MainMenu;
        private GameState GameState;
        private Map Map;
        private HUD HUD;
        private TcpClient Client;
        private Dictionary<string, Player> Players;
        public static ContentManager ContentManager;
        private bool isPressed;

        //Online
        private Thread ReceiveThread;
        private Thread SendThread;
        private string ServerIp;
        private int ServerPort;
        public string[] ReceivedDataArray;

        public SquadFighters()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 500;
            Graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Players = new Dictionary<string, Player>();
            ServerIp = "192.168.1.17";
            ServerPort = 7895;
            Window.Title = "SquadFighters: Battle Royale";
            GameState = GameState.MainMenu;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ContentManager = Content;

            MainMenu = new MainMenu(2);
            MainMenu.LoadContent(Content);

            Map = new Map(new Rectangle(0, 0, 10000, 10000), Content);
        }

        public void ConnectToServer(string serverIp, int serverPort)
        {
            try
            {
                Client = new TcpClient(ServerIp, ServerPort);
                SendOneDataToServer("Load Map");
                //SendOneDataToServer(Player.Name + ",Connected.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddPlayer(string CurrentConnectedPlayerName)
        {
            Player player = new Player(CurrentConnectedPlayerName);
            player.LoadContent(Content);
            Players.Add(CurrentConnectedPlayerName, player);
        }

        public void JoinGame()
        {
            Player = new Player("idan" + new Random().Next(1000));
            Player.LoadContent(Content);

            ConnectToServer(ServerIp, ServerPort);

            ReceiveThread = new Thread(ReceiveDataFromServer);
            ReceiveThread.Start();
        }

        public void SendOneDataToServer(string data)
        {
            try
            {
                NetworkStream stream = Client.GetStream();
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception)
            {

            }
        }

        public void SendDataToServer()
        {
            while (true)
            {
                string data = Player.ToString();
                try
                {
                    NetworkStream stream = Client.GetStream();
                    byte[] bytes = Encoding.ASCII.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception)
                {

                }

                Thread.Sleep(50);
            }
        }
        
        public void DisconnectFromServer()
        {
            try
            {
                ReceiveThread.Abort();
                SendThread.Abort();
                Client.Close();
            }
            catch (Exception)
            {

            }

        }

        public void ReceiveDataFromServer()
        {
            while (true)
            {
                try
                {
                    NetworkStream netStream = Client.GetStream();
                    byte[] bytes = new byte[10024];
                    netStream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.ASCII.GetString(bytes);
                    string ReceivedDataString = data.Substring(0, data.IndexOf("\0"));
                    ReceivedDataArray = ReceivedDataString.Split(',');

                    if (ReceivedDataString.Contains("Connected"))
                    {
                        string CurrentConnectedPlayerName = ReceivedDataString.Split(',')[0];

                        if (CurrentConnectedPlayerName != Player.Name)
                        {
                            AddPlayer(CurrentConnectedPlayerName);
                        }
                    }
                    else if (ReceivedDataString.Contains("PlayerName"))
                    {
                        string playerName = ReceivedDataArray[0].Split('=')[1];
                        float playerX = float.Parse(ReceivedDataArray[1].Split('=')[1]);
                        float playerY = float.Parse(ReceivedDataArray[2].Split('=')[1]);
                        float playerRotation = float.Parse(ReceivedDataArray[3].Split('=')[1]);
                        int playerHealth = int.Parse(ReceivedDataArray[4].Split('=')[1]);
                        bool playerIsShoot = bool.Parse(ReceivedDataArray[5].Split('=')[1]);
                        float playerDirectionX = float.Parse(ReceivedDataArray[6].Split('=')[1]);
                        float playerDirectionY = float.Parse(ReceivedDataArray[7].Split('=')[1]);

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
                        ItemCategory ItemCategory = (ItemCategory)int.Parse(ReceivedDataArray[1].Split('=')[1]);
                        int type = int.Parse(ReceivedDataArray[2].Split('=')[1].ToString());
                        float itemX = float.Parse(ReceivedDataArray[3].Split('=')[1].ToString());
                        float itemY = float.Parse(ReceivedDataArray[4].Split('=')[1].ToString());
                        Map.GenerateItem(ItemCategory, type, itemX, itemY);
                    }
                    else if (ReceivedDataString.Contains("Load Items Completed"))
                    {
                        GameState = GameState.Game;

                        SendOneDataToServer(Player.Name + ",Connected");

                        Thread.Sleep(500);

                        SendThread = new Thread(() => SendDataToServer());
                        SendThread.Start();
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
                DisconnectFromServer();
                Exit();
            }

            if (GameState == GameState.Game)
            {
                Camera.Focus(Player.Position, Map.Rectangle.Width, Map.Rectangle.Height);
                Player.Update(Map);


                foreach (KeyValuePair<string, Player> otherPlayer in Players)
                {
                    otherPlayer.Value.UpdateRectangle();

                    for (int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                    {
                        if (otherPlayer.Value.Bullets[i].Rectangle.Intersects(Player.Rectangle))
                        {
                            otherPlayer.Value.Bullets[i].IsFinished = true;
                            Player.Hit(otherPlayer.Value.Bullets[i].Damage);
                        }

                        if (!otherPlayer.Value.Bullets[i].IsFinished)
                            otherPlayer.Value.Bullets[i].Update();
                        else
                            otherPlayer.Value.Bullets.RemoveAt(i);
                    }
                }

                for (int i = 0; i < Players.Count; i++)
                {
                    for (int j = 0; j < Player.Bullets.Count; j++)
                    {
                        if (Player.Bullets[j].Rectangle.Intersects(Players.ElementAt(i).Value.Rectangle))
                            Player.Bullets[j].IsFinished = true;

                        if (!Player.Bullets[j].IsFinished)
                            Player.Bullets[j].Update();
                        else
                            Player.Bullets.RemoveAt(j);
                    }
                }

                for (int i = 0; i < Map.Items.Count; i++)
                {
                    Map.Items[i].Update();
                }
            }
            else if(GameState == GameState.MainMenu)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !isPressed)
                {
                    isPressed = true;

                    if (MainMenu.Buttons[0].Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                    {
                        GameState = GameState.Loading;
                        JoinGame();
                    } 
                }

                if(Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    isPressed = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            if (GameState == GameState.Game)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

                for (int i = 0; i < Map.Items.Count; i++)
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
                        otherPlayer.Value.Bullets[i].Draw(spriteBatch);

                    HUD.DrawPlayersInfo(spriteBatch, otherPlayer.Value);
                }

                Player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin();

                HUD.Draw(spriteBatch, Player, Players);

                if (Player.IsShield)
                {
                    foreach (ShieldBar shieldbar in Player.Shield.ShieldBars)
                        shieldbar.Draw(spriteBatch);
                }

                spriteBatch.End();
            }
            else if(GameState == GameState.MainMenu)
            {
                spriteBatch.Begin();

                foreach (Button button in MainMenu.Buttons)
                {
                    if (button.Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                        button.Draw(spriteBatch, true);
                    else
                        button.Draw(spriteBatch, false);
                }

                spriteBatch.End();
            }
            else if(GameState == GameState.Loading)
            {
                spriteBatch.Begin();

                HUD.DrawLoading(spriteBatch);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
