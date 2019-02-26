using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace SquadFighters
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
                string data = SquadFighters.Player.ToString();
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
            player.LoadContent(SquadFighters.ContentManager);
            SquadFighters.Players.Add(CurrentConnectedPlayerName, player);
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

                        if (CurrentConnectedPlayerName != SquadFighters.Player.Name)
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

                        if (SquadFighters.Players.ContainsKey(playerName))
                        {
                            SquadFighters.Players[playerName].Name = playerName;
                            SquadFighters.Players[playerName].Position.X = playerX;
                            SquadFighters.Players[playerName].Position.Y = playerY;
                            SquadFighters.Players[playerName].Rotation = playerRotation;
                            SquadFighters.Players[playerName].Health = playerHealth;
                            SquadFighters.Players[playerName].IsShoot = playerIsShoot;
                            SquadFighters.Players[playerName].Direction.X = playerDirectionX;
                            SquadFighters.Players[playerName].Direction.Y = playerDirectionY;

                            if (playerIsShoot)
                            {
                                SquadFighters.Players[playerName].Shoot();
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
