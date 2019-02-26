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

        public string[] ReceivedDataArray;

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
