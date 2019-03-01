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
        private SpriteBatch spriteBatch;

        public Player Player;
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
        private Thread SendPlayerDataThread;
        private string ServerIp;
        private int ServerPort;
        public string[] ReceivedDataArray;
        private int MaxItems;

        public SquadFighters()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 450;
            Graphics.PreferredBackBufferHeight = 650;
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

            PlayerCard playerCard = new PlayerCard(player.Name, player.Health, player.BulletsCapacity + "/" + player.MaxBulletsCapacity);
            playerCard.LoadContent(Content);

            HUD.PlayersCards.Add(playerCard);
        }

        public void JoinGame()
        {
            Player = new Player("idan" + new Random().Next(1000));
            Player.LoadContent(Content);

            HUD.PlayerCard = new PlayerCard(Player.Name, Player.Health, Player.BulletsCapacity + "/" + Player.MaxBulletsCapacity);
            HUD.PlayerCard.LoadContent(Content);
 
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
                stream.Flush();

                Thread.Sleep(30);
            }
            catch (Exception)
            {

            }
        }

        public void SendPlayerDataToServer()
        {
            while (true)
            {
                string data = Player.ToString();
                try
                {
                    NetworkStream stream = Client.GetStream();
                    byte[] bytes = Encoding.ASCII.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();


                    Thread.Sleep(200);
                }
                catch (Exception)
                {

                }
            }
        }
        
        public void DisconnectFromServer()
        {
            try
            {
                ReceiveThread.Abort();
                SendPlayerDataThread.Abort();
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
                            Console.WriteLine(ReceivedDataString);
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
                        bool playerIsSwimming = bool.Parse(ReceivedDataArray[8].Split('=')[1]);
                        bool playerIsShield = bool.Parse(ReceivedDataArray[9].Split('=')[1]);
                        ShieldType playerShieldType = (ShieldType)int.Parse(ReceivedDataArray[10].Split('=')[1]);
                        int playerBulletsCapacity = int.Parse(ReceivedDataArray[11].Split('=')[1]);

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
                            Players[playerName].IsSwimming = playerIsSwimming;
                            Players[playerName].IsShield = playerIsShield;
                            Players[playerName].BulletsCapacity = playerBulletsCapacity;

                            Players[playerName].Shield = new Shield(new Vector2(0, 0), playerShieldType, 100);
                            Players[playerName].Shield.LoadContent(Content);

                            for(int i = 0; i < HUD.PlayersCards.Count; i++)
                            {
                                if (HUD.PlayersCards[i].PlayerName == playerName && (HUD.PlayersCards[i].Shield.ItemType == ShieldType.None || HUD.PlayersCards[i].Shield.ItemType != playerShieldType))
                                    HUD.PlayersCards[i].Shield = Players[playerName].Shield;
                            }

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
                        int itemCapacity = int.Parse(ReceivedDataArray[5].Split('=')[1].ToString());
                        string itemKey = ReceivedDataArray[6].Split('=')[1].ToString();
                        MaxItems = int.Parse(ReceivedDataArray[7].Split('=')[1].ToString());
                        Map.AddItem(ItemCategory, type, itemX, itemY, itemCapacity, itemKey);

                        Console.WriteLine(ReceivedDataString);
                    }
                    else if (ReceivedDataString.Contains("Remove Item"))
                    {
                        string itemKey = ReceivedDataArray[1];
                        Map.Items.Remove(itemKey);
                        Console.WriteLine(ReceivedDataString);
                    }
                    else if (ReceivedDataString.Contains("Update Item Capacity"))
                    {
                        string itemKey = ReceivedDataArray[2];
                        int receivedCapacity = int.Parse(ReceivedDataArray[1]);
                        if (Map.Items[itemKey] is GunAmmo)
                        {
                            ((GunAmmo)(Map.Items[itemKey])).Capacity = receivedCapacity;
                        }

                        Console.WriteLine(ReceivedDataString);
                    }
                    else if (ReceivedDataString.Contains("Load Items Completed"))
                    {
                        GameState = GameState.Game;

                        SendOneDataToServer(Player.Name + ",Connected");

                        Thread.Sleep(500);

                        SendPlayerDataThread = new Thread(() => SendPlayerDataToServer());
                        SendPlayerDataThread.Start();

                        Console.WriteLine(ReceivedDataString);
                    }
 
                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void CheckItemsIntersects(Dictionary<string, Item> items)
        {
            try
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (Player.Rectangle.Intersects(items.ElementAt(i).Value.Rectangle))
                    {
                        if (items.ElementAt(i).Value is Food)
                        {
                            if (Player.Health < 100)
                            {
                                int heal = ((Food)(items.ElementAt(i).Value)).GetHealth();
                                Player.Heal(heal);
                                string key = items.ElementAt(i).Key;

                                items.Remove(key);
                                SendOneDataToServer("Remove Item," + key);
                            }
                        }
                        else if (items.ElementAt(i).Value is GunAmmo)
                        {
                            int capacity = ((GunAmmo)(items.ElementAt(i).Value)).Capacity;

                            if (Player.BulletsCapacity + capacity <= Player.MaxBulletsCapacity)
                            {
                                Player.BulletsCapacity += capacity;
                                string key = items.ElementAt(i).Key;

                                items.Remove(key);
                                SendOneDataToServer("Remove Item," + key);
                            }
                            else
                            {
                                if (Player.BulletsCapacity + capacity > Player.MaxBulletsCapacity && Player.BulletsCapacity != Player.MaxBulletsCapacity)
                                {
                                    ((GunAmmo)(items.ElementAt(i).Value)).Capacity -= Player.MaxBulletsCapacity - Player.BulletsCapacity;
                                    Player.BulletsCapacity = Player.MaxBulletsCapacity;

                                    int itemCapacity = ((GunAmmo)(items.ElementAt(i).Value)).Capacity;
                                    string key = items.ElementAt(i).Key;
                                    SendOneDataToServer("Update Item Capacity," + itemCapacity + "," + key);
                                }
                            }
                        }
                        else if (items.ElementAt(i).Value is Shield)
                        {
                            if (Player.Shield == null || Player.Shield.ItemType < ((Shield)items.ElementAt(i).Value).ItemType)
                            {
                                Player.Shield = items.ElementAt(i).Value as Shield;
                                Player.IsShield = true;

                                string key = items.ElementAt(i).Key;
                                items.Remove(key);
                                SendOneDataToServer("Remove Item," + key);
                            }

                        }
                        else if (items.ElementAt(i).Value is Helmet)
                        {
                            string key = items.ElementAt(i).Key;
                            items.Remove(key);
                            SendOneDataToServer("Remove Item," + key);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new Camera(GraphicsDevice.Viewport);

            HUD = new HUD();
            HUD.LoadContent(Content);

            Random rndItem = new Random();

            MainMenu = new MainMenu(2);
            MainMenu.LoadContent(Content);

            Map = new Map(new Rectangle(0, 0, 5000, 5000));
            Map.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        public int GetHealthByPlayerName(string name)
        {
            foreach (KeyValuePair<string, Player> otherPlayer in Players)
                if (name == otherPlayer.Key) return otherPlayer.Value.Health;

            if (name == Player.Name)
                return Player.Health;

            return 0;
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

                CheckItemsIntersects(Map.Items);

                HUD.PlayerCard.SetPosition(new Vector2(0, 0));
                HUD.PlayerCard.HealthBar.SetHealth(Player.Health);
                HUD.PlayerCard.AmmoString = Player.BulletsCapacity + "/" + Player.MaxBulletsCapacity;

                for(int i = 0; i < HUD.PlayersCards.Count; i++)
                {
                    string playerName = HUD.PlayersCards[i].PlayerName;

                    HUD.PlayersCards[i].SetPosition(new Vector2(HUD.PlayersCards[i].CardPosition.X, HUD.PlayerCard.CardRectangle.Height + 10 + HUD.PlayersCards[i].CardRectangle.Height * i));
                    HUD.PlayersCards[i].HealthBar.SetHealth(GetHealthByPlayerName(playerName));
                    HUD.PlayersCards[i].AmmoString = Players[playerName].BulletsCapacity + "/" + Players[playerName].MaxBulletsCapacity;
    
                }

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
                for (int i = 0; i < Player.Bullets.Count; i++)
                {
                    if (!Player.Bullets[i].IsFinished)
                    {
                        Player.Bullets[i].Update();

                        for (int j = 0; j < Players.Count; j++)
                        {
                            if (Player.Bullets[i].Rectangle.Intersects(Players.ElementAt(j).Value.Rectangle))
                                Player.Bullets[i].IsFinished = true;

                        }
                    }
                    else
                        Player.Bullets.RemoveAt(i);
                }

                try
                {
                    for (int i = 0; i < Map.Items.Count; i++)
                    {
                        Map.Items.ElementAt(i).Value.Update();
                    }
                }
                catch (Exception)
                {

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
                    if (MainMenu.Buttons[1].Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                    {
                        Exit();
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

                Map.Draw(spriteBatch);

                try
                {
                    for (int i = 0; i < Map.Items.Count; i++)
                    {
                        Map.Items.ElementAt(i).Value.Draw(spriteBatch);
                        spriteBatch.DrawString(HUD.ItemsCapacityFont, Map.Items.ElementAt(i).Value.ToString(), new Vector2(Map.Items.ElementAt(i).Value.Position.X + 15, Map.Items.ElementAt(i).Value.Position.Y - 30), Color.Black);

                    }
                }
                catch (Exception)
                {

                }

                foreach (Bullet bullet in Player.Bullets)
                    bullet.Draw(spriteBatch);


                foreach (KeyValuePair<string, Player> otherPlayer in Players)
                {
                    if(!otherPlayer.Value.IsSwimming)
                        otherPlayer.Value.Draw(spriteBatch);

                    for (int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                        otherPlayer.Value.Bullets[i].Draw(spriteBatch);

                    HUD.DrawPlayersInfo(spriteBatch, otherPlayer.Value);
                }


                Player.Draw(spriteBatch);
                HUD.DrawPlayerInfo(spriteBatch, Player);

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


                HUD.DrawGameTitle(spriteBatch);

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


                HUD.DrawGameTitle(spriteBatch);
                HUD.DrawLoading(spriteBatch, MaxItems, Map.Items.Count);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
