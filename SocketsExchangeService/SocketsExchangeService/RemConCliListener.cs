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
    public class RemConCliListener
    {
        public Thread ServeThread;

        
        private List<TCPClientConn> clients;
        public int NumClients { get { return clients.Count; } }

        RecSock receiver;
        
        string svrIP;
        int svrPort;

        public RemConCliListener(string ip, int port)
        {
            //start backgroundthread and socket connection to client
            //parentThread = owner;
            clients = new List<TCPClientConn>();

            svrIP = ip;
            svrPort = port;

            ServeThread = new Thread(new ThreadStart(TCP_Listener));
            ServeThread.Start();

            //ServeThread = new BackgroundWorker();
            //ServeThread.DoWork += ConnThread_DoWork;
            //ServeThread.WorkerSupportsCancellation = true;
            //ServeThread.RunWorkerAsync();
        }
        
        public void SetupSendToClientSocket(string ip, int port)
        {
            //sendy = new SendSock(ip, port);

            //SendToClient("Hello! I configured you as a server and I can bug you with messages now!");
            //configure to heartbeat every 20 seconds?
        }

        void TCP_Listener()
        {
            receiver = new RecSock(svrIP, svrPort);  //shouldn't new up each loop????
            
            while (true)
            {
                try
                {
                
                    string retstr = null;
                    
                    receiver.AcceptAConnection();

                    GlobSyn.Log("Waiting for connected client to talk to: " + receiver.connEndPoint.ToString());
                    //stall on first message - WHAT IS YOUR PURPOSE HERE, SON?!  IDENTIFY YOURSELF
                    do
                    {
                        retstr = receiver.ReceiveWholeMsg();
                    } while (retstr == null);
                    //HERE tell parent to set up connection to client's server
                    //_synch.Invoke(parentThread.TakeThisLogMsg, new object[1] { "Initial Message received from client " + receiver.remEP.ToString() });

                    ControlClientCache.NumTotalClients += 1;   //??? only do this if control client...not RPI
                    GlobSyn.Log("Initial Message received from client " + receiver.remEP.ToString());
                    GlobSyn.MsgFromClient(this, retstr);


                    try
                    {
                        clients.Add(new TCPClientConn(receiver.handler)); //starts a new threaded conversation
                    }
                    catch (Exception exIn)
                    {
                        GlobSyn.Log("FAILED trying to start TCP Client Connection thread" + Environment.NewLine + "~~See Exception: " + exIn.Message);
                    }

                }
                catch (Exception ex)
                {
                    GlobSyn.Log("FAILED TCP Listener " + receiver.connEndPoint.ToString() + " has crashed" + Environment.NewLine + "~~See Exception: " + ex.Message);
                }
            }
        }
    }

    public class TCPClientConn : IDisposable
    {
        private Thread connThread;
        private Socket connection;
        private System.Timers.Timer t;
        private byte[] bytes;
        private uint CID;

        private ISynchronizeInvoke Sync;
        private SockMsg MsgFromClient;
        private LogMsg TakeThisLogMsg;

        private bool disposed = false;

        public TCPClientConn(Socket handle)
        {
            connection = handle;


            CID = ControlClientCache.GenerateClientID();

            connThread = new Thread(new ThreadStart(BidiChat));
            connThread.Start();

        }

        private void BidiChat()
        {
            try
            {
                t = new System.Timers.Timer();
                t.Elapsed += new ElapsedEventHandler(CheckForMsg); //check for message to send ever 100ms
                t.Interval = 100;
                t.Enabled = true;

                while (true)
                {
                    //rCount = Convert.ToString(requestCount);
                    //serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                    //bytes = Encoding.ASCII.GetBytes("I'ma talk back at your face every single loop through this thang");
                    //connection.Send(bytes);
                }
            }
            catch(Exception Ex)
            {
                GlobSyn.Log("FAILED TCP Client Connection reading from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);    

            }
        }

        
        private void CheckForMsg(object derp, ElapsedEventArgs e)
        {
            string msg;
            msg = ControlClientCache.Read(CID);

            if(msg != null)
            {
                bytes = Encoding.ASCII.GetBytes(msg);
                connection.Send(bytes);
            }

            t.Enabled = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    connThread.Abort();
                    connection.Dispose();
                    t.Dispose();
                    //bytes = null;
                }
                // Note disposing has been done.
                disposed = true;

            }
        }
    }

    }



       
