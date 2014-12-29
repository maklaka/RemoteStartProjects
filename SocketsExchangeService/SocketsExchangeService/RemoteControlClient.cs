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
using System.Timers;
using System.Runtime.Remoting.Messaging;

namespace SocketsExchangeService
{
    class RemoteControlClient
    {
        public Thread ServeThread;
        RecSock receiver;
        XChngServer parentThread;

        string svrIP;
        int svrPort;

        //public BackgroundWorker ClientThread;
        SendSock sendy;
        private  ISynchronizeInvoke _synch;

        
        
        public RemoteControlClient(XChngServer owner)
        {
            //start backgroundthread and socket connection to client
            parentThread = owner;

            svrIP = "10.0.0.8";
            svrPort = 10000;

            ServeThread = new Thread(new ThreadStart(ConnThread_DoWork));
            ServeThread.Start();
            //ServeThread.DoWork += ConnThread_DoWork;
            //ServeThread.WorkerSupportsCancellation = true;
            //ServeThread.RunWorkerAsync();
        }

        public RemoteControlClient(XChngServer owner, string ip, int port)
        {
            //start backgroundthread and socket connection to client
            parentThread = owner;

            svrIP = ip;
            svrPort = port;

            ServeThread = new Thread(new ThreadStart(ConnThread_DoWork));
            ServeThread.Start();

            //ServeThread = new BackgroundWorker();
            //ServeThread.DoWork += ConnThread_DoWork;
            //ServeThread.WorkerSupportsCancellation = true;
            //ServeThread.RunWorkerAsync();
        }
        
        public void SetupSendToClientSocket(string ip, int port)
        {
            sendy = new SendSock(ip, port);

            SendToClient("Hello! I configured you as a server and I can bug you with messages now!");
            //configure to heartbeat every 20 seconds?
        }
        
        public void SendToClient(string msg)
        {
            sendy.TransmitMessage(msg);

        }

        void ConnThread_DoWork()
        {
            try
            {   
                string retstr = null;
                receiver = new RecSock(svrIP, svrPort);  
                receiver.AcceptAConnection();
                //log msg waiting for a request
                //FactorizingAsyncDelegate 

                parentThread.TakeThisLogMsg.Invoke("Waiting for client to talk on: " + receiver.connEndPoint.ToString());
                //stall on first message, need return ip/port of client's "server"
                do
                {
                    retstr = receiver.ReceiveWholeMsg();
                } while (retstr == null);
                //HERE tell parent to set up connection to client's server
                //_synch.Invoke(parentThread.TakeThisLogMsg, new object[1] { "Initial Message received from client " + receiver.remEP.ToString() });
                parentThread.TakeThisLogMsg.Invoke("Initial Message received from client " + receiver.remEP.ToString());
                
                parentThread.MsgFromClient.Invoke(this, retstr);


                while(true)
                {
                
                    
                }
                 
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            //throw new NotImplementedException();
        }

        void derp(IAsyncResult  res)
        {


        }
    }
}
