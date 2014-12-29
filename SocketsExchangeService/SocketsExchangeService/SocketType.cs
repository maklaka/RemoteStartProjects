using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace SocketsExchangeService
{
    class SocketType
    {
        protected IPAddress ipAddress;
        public IPEndPoint connEndPoint;
        protected byte[] bytes;
    }

    class SendSock : SocketType
    {
        Socket sender;
        public SendSock(string ip, int port)
        {
            bytes = new Byte[1024];
            ipAddress = IPAddress.Parse(ip);
            connEndPoint = new IPEndPoint(ipAddress, port);
            sender = new Socket(AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(connEndPoint.Address, connEndPoint.Port);
            //LOG MSG Connected to client
        }

        public void TransmitMessage(string msg)
        {
            bytes = Encoding.ASCII.GetBytes(msg +  "  " + sender.LocalEndPoint.ToString());
            sender.Send(bytes);
            //delete bytes?
        }
    }

    class RecSock : SocketType
    {
        
        Socket listener;
        Socket handler;
        public EndPoint remEP;
        string data = null;

        public RecSock(string ip, int port)
        {
            bytes = new Byte[1024];
            ipAddress = IPAddress.Parse(ip);
            connEndPoint = new IPEndPoint(ipAddress, port);
            listener = new Socket(AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(connEndPoint);
            listener.Listen(1);

            //WHERE SHOULD I PUT THESE?!
            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();

        }
        public void AcceptAConnection()
        {
             handler = listener.Accept();
             remEP = handler.RemoteEndPoint;
             // delete listener here?
             //handler.Blocking = false;  thought this would have to be enabled? hrmm ???
        }
        public string ReceiveWholeMsg()
        {
            bool msghere = false;
            do
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                if (bytesRec > 0) //lock into receiving message until <EOF> detected
                {
                    msghere = true;
                }
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    return data;
                }
            } while (msghere);

            return null;
        }

    }
}
