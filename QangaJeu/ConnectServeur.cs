using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace LauncherQangaClient
{
    class ConnectServeur
    {
        private TcpClient client;
       // private string message;
        

        public string connect(string ip, int port,string message)
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);
                //byte[] send = System.Text.Encoding.UTF8.GetBytes("test stp\r\n");
                
                byte[] send = System.Text.Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();
                
                stream.Write(send, 0,send.Length);
                byte[] recv = new byte[256];
                int i;
                string fromServ="";
                while ((i = stream.Read(recv, 0, recv.Length)) != 0)
                {
                    fromServ = System.Text.Encoding.ASCII.GetString(recv, 0, i);
                    //Console.WriteLine(fromServ);
                }
               // Console.WriteLine(fromServ);
                
                client.Close();
                return fromServ;
            }
            catch (Exception e)
            {

            }

            return "-1";
        }
    }
}
