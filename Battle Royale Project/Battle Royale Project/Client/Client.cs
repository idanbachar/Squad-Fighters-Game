using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace Battle_Royale_Project
{
    public class Client
    {
        public string ServerIp;
        public int ServerPort;

        public TcpClient client;
        public Thread ReceiveThread;
        public Thread SendThread;

        private string[] ReceivedDataArray;
        private ContentManager Content;

        public Client(string serverIp, int serverPort, string playerName)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;

            try{
                client = new TcpClient(ServerIp, ServerPort);

                /*    Connection Succesfull     */
                SendMessage(playerName + ",Connected.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendData()
        {
            while (true)
            {
                string data = Game1.Player.ToString();
                SendMessage(data);

                Thread.Sleep(50);
            }
        }
   

        public void SendMessage(string data)
        {
            try{
                NetworkStream stream = client.GetStream();
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {

            }
        }

        public void AddPlayer(string CurrentConnectedPlayerName)
        {
            Player player = new Player(CurrentConnectedPlayerName);
            player.LoadContent(Game1.ContentManager);
            Game1.Players.Add(CurrentConnectedPlayerName, player);
        }

        public void ReceiveData()
        {
            while (true)
            {
                try
                {
                    NetworkStream netStream = client.GetStream();
                    byte[] bytes = new byte[1024];
                    netStream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.ASCII.GetString(bytes);
                    string ReceivedDataString = data.Substring(0, data.IndexOf("\0"));
                    ReceivedDataArray = ReceivedDataString.Split(',');

                    if (ReceivedDataString.Contains("Connected"))
                    {
                        string CurrentConnectedPlayerName = ReceivedDataString.Split(',')[0];

                        if (CurrentConnectedPlayerName != Game1.Player.Name)
                        {
                            AddPlayer(CurrentConnectedPlayerName);
                        }
                    }
                    else
                    {
                        //new Random().Next(0, Map.Rectangle.Width - 100), new Random().Next(0, Map.Rectangle.Height - 100)

                        string playerName = ReceivedDataArray[0].Split('=')[1];
                        float playerX = float.Parse(ReceivedDataArray[1].Split('=')[1]);
                        float playerY = float.Parse(ReceivedDataArray[2].Split('=')[1]);
                        float playerRotation = float.Parse(ReceivedDataArray[3].Split('=')[1]);
                        int playerHealth = int.Parse(ReceivedDataArray[4].Split('=')[1]);
                        bool playerIsShoot = bool.Parse(ReceivedDataArray[5].Split('=')[1]);
                        float playerDirectionX = float.Parse(ReceivedDataArray[6].Split('=')[1]);
                        float playerDirectionY = float.Parse(ReceivedDataArray[7].Split('=')[1]);

                        if (Game1.Players.ContainsKey(playerName))
                        {
                            Game1.Players[playerName].Name = playerName;
                            Game1.Players[playerName].Position.X = playerX;
                            Game1.Players[playerName].Position.Y = playerY;
                            Game1.Players[playerName].Rotation = playerRotation;
                            Game1.Players[playerName].Health = playerHealth;
                            Game1.Players[playerName].IsShoot = playerIsShoot;
                            Game1.Players[playerName].Direction.X = playerDirectionX;
                            Game1.Players[playerName].Direction.Y = playerDirectionY;

                            if (playerIsShoot)
                            {
                                Game1.Players[playerName].Shoot();
                            }
                        }
                        else
                        {
                            AddPlayer(playerName);
                        }

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

        public void CloseConnection()
        {
            try{
                ReceiveThread.Abort();
                SendThread.Abort();
                client.Close();
            }
            catch (Exception e)
            {

            }
            
        }
    }
}
