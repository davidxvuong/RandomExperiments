using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading;

namespace Server
{
    class WebSocketService
    {
        Socket client;
        string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        string nameOfClient;

        public WebSocketService(Socket obj)
        {
            client = obj;
            nameOfClient = "";
        }
        
        //WebSocket Handshake
        public void Start()
        {
            try
            {
                byte[] buffer = new byte[1024];
                byte[] handshakeResponse;
                string headerResponse = "";
                
                client.Receive(buffer);
                
                headerResponse = Encoding.UTF8.GetString(buffer);

                int keyStartIndex = headerResponse.IndexOf("Sec-WebSocket-Key: ") + 19;
                int keyEndIndex = headerResponse.IndexOf("\r\n", keyStartIndex);
                
                string key = headerResponse.Substring(keyStartIndex, keyEndIndex - keyStartIndex) + guid;
                string hash = Convert.ToBase64String(ComputeHash(key));

                handshakeResponse = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols\r\n" +
                        "Connection: Upgrade\r\nUpgrade: websocket\r\nSec-WebSocket-Accept: " + hash + "\r\n\r\n");
                client.Send(handshakeResponse);

                client.Receive(buffer);

                nameOfClient = DecodeMessage(buffer);
                Console.WriteLine("{0}: {1} connected!",DateTime.Now, nameOfClient);
                client.Send(EncodeMessage("C#"));

                ListenForMessages();
            }
            catch {}
        }

        private byte[] ComputeHash(string key)
        {
            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            return sha1.ComputeHash(Encoding.ASCII.GetBytes(key));
        }
        
        public void ListenForMessages()
        {
            byte[] incoming = new Byte[1024];
            var i = client.Receive(incoming);
            string message = DecodeMessage(incoming);

            if (message != "Close")
            {
                Console.WriteLine("{0}: Echoing message back to " + nameOfClient + ": " + message, DateTime.Now);
                client.Send(EncodeMessage(message));
                ListenForMessages();
            }
            else
                DisposeThread();
        }

        private void DisposeThread()
        {
            client.Close();
            Console.WriteLine("{0}: " + nameOfClient + " disconnected. ", DateTime.Now);
            Program.Count--;
            Thread.CurrentThread.Abort();
        }

        #region Decode and Encode Messages for WebSocket
        private string DecodeMessage(byte[] data)
        {
            byte length = (byte)(data[1] & 127);
            byte[] decoded;
            byte[] mask = new byte[4];
            int maskIndex = 2;

            if (length == 126)
                maskIndex += 2;
            else if (length == 127)
                maskIndex += 8;

            Array.Copy(data, maskIndex, mask, 0, 4);

            int dataIndex = maskIndex + 4;
            decoded = new byte[data.Length - dataIndex];

            for (int i = dataIndex, j = 0; j < decoded.Length; i++, j++)
                decoded[j] = (byte)(data[i] ^ mask[j % 4]);

            string raw = Encoding.UTF8.GetString(decoded);
            int badDataIndex = raw.IndexOf("EOD");
            return raw.Remove(badDataIndex, raw.Length - badDataIndex);
        }

        private Byte[] EncodeMessage(string text)
        {
            Byte[] response;
            Byte[] bytesRaw = Encoding.UTF8.GetBytes(text);
            Byte[] frame = new Byte[10];

            Int32 indexStartRawData = -1;
            Int32 length = bytesRaw.Length;

            frame[0] = (Byte)129;
            if (length <= 125)
            {
                frame[1] = (Byte)length;
                indexStartRawData = 2;
            }
            else if (length >= 126 && length <= 65535)
            {
                frame[1] = (Byte)126;
                frame[2] = (Byte)((length >> 8) & 255);
                frame[3] = (Byte)(length & 255);
                indexStartRawData = 4;
            }
            else
            {
                frame[1] = (Byte)127;
                frame[2] = (Byte)((length >> 56) & 255);
                frame[3] = (Byte)((length >> 48) & 255);
                frame[4] = (Byte)((length >> 40) & 255);
                frame[5] = (Byte)((length >> 32) & 255);
                frame[6] = (Byte)((length >> 24) & 255);
                frame[7] = (Byte)((length >> 16) & 255);
                frame[8] = (Byte)((length >> 8) & 255);
                frame[9] = (Byte)(length & 255);

                indexStartRawData = 10;
            }

            response = new Byte[indexStartRawData + length];

            Int32 i, reponseIdx = 0;

            //Add the frame bytes to the reponse
            for (i = 0; i < indexStartRawData; i++)
            {
                response[reponseIdx] = frame[i];
                reponseIdx++;
            }

            //Add the data bytes to the response
            for (i = 0; i < length; i++)
            {
                response[reponseIdx] = bytesRaw[i];
                reponseIdx++;
            }

            return response;
        }
        #endregion 
    }
}
