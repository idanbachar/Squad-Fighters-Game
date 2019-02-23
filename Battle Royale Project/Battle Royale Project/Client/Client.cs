using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Battle_Royale_Project
{
    public class Client
    {
        public string ServerIp;
        public int ServerPort;

        public TcpClient client;
        public Thread ReceiveThread;

        private string[] ReceivedDataArray;

        public Client(string serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;

            try{
                client = new TcpClient(ServerIp, ServerPort);

                /*    Connection Succesfull     */
                SendData("idan" + new Random().Next(1000) + ",Connected.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendData(string data)
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

                    Console.WriteLine(ReceivedDataString);
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
                client.Close();
            }
            catch (Exception e)
            {

            }
            
        }
    }
}
