using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static int count = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Server started. Accepting connections.\n\n");
            Restart();
            Console.ReadLine();
        }

        public static int Count
        {
            set
            {
                count = value;
            }
            get
            {
                return count;
            }
        }

        private static void Restart()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 2048);
            Socket client = null;
            listener.Start();

            while (count < 10)
            {
                client = listener.AcceptSocket();
                ThreadPool.QueueUserWorkItem(WebSocketThread, client);
                count++;
            }

            Console.WriteLine("{0}: All threads used by clients. No new threads will be created.", DateTime.Now);
        }

        private static void WebSocketThread(object state)
        {
            Socket client = (Socket)(state);
            WebSocketService instance = new WebSocketService(client);
            instance.Start();
        }
    }
}
