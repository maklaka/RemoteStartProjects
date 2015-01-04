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
    public enum ClientType { ConsumerClient, RPIProducerClient };
    public delegate void ClientKiller(TCPClientConn conn);

    public class ClientListener
    {
        public Thread ServeThread;  
        private List<TCPClientConn> clients;
        private ClientType cType;

        public int NumClients { get { return clients.Count; } }
        RecSock receiver;
        
        string svrIP;
        int svrPort;

        public ClientListener(string ip, int port, ClientType ct)
        {
            //start backgroundthread and socket connection to client
            //parentThread = owner;
            clients = new List<TCPClientConn>();
            cType = ct;
            svrIP = ip;
            svrPort = port;

            ServeThread = new Thread(new ThreadStart(TCP_Listener));
            ServeThread.Start();

            //ServeThread = new BackgroundWorker();
            //ServeThread.DoWork += ConnThread_DoWork;
            //ServeThread.WorkerSupportsCancellation = true;
            //ServeThread.RunWorkerAsync();
        }

        void TCP_Listener()
        {
            receiver = new RecSock(svrIP, svrPort);  //shouldn't new up each loop
            
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

                    GlobSyn.Log("Initial Message received from client " + receiver.remEP.ToString());
                    

                    try
                    {
                        TCPClientConn temp = new TCPClientConn(receiver.handler, cType);
                        temp.SelfDestruct += new ClientKiller(MuderThatClientAtHisBehest);
                        clients.Add(temp); //starts a new threaded conversation                   
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

        private void MuderThatClientAtHisBehest(TCPClientConn conn)
        {
            //taking the object out of any scope should get her good and garbage collected...eventually >.V  hopefully the thread actually stopped
            clients.Remove(conn);
            conn = null;
        }
    }

    public class TCPClientConn
    {
        private Thread connThread;
        private Socket connection;
        private System.Timers.Timer timSendChk;
        private System.Timers.Timer timKeepAlive;
        private System.Timers.Timer timRecMsgEscape;

        private bool KeepAliveEnabled = true;
        private bool waitingForAck = false;
        private bool transmittingNow = false;
        private byte[] bytes;
        private uint CID;
        private ClientType cType;
       
        
        private bool disposing = false;

        public ClientKiller SelfDestruct;

        public TCPClientConn(Socket handle, ClientType ct)
        {
            connection = handle;
            connection.ReceiveTimeout = 500;   //not presently used? maybe?  I'm in "non-blocking" sychronous mode..whatever that means?!!????
            cType = ct;
            CID = ClientMsgCache.GenerateClientID(cType);
            connThread = new Thread(new ThreadStart(BidiChat));
            connThread.Start();  

        }

        private void BidiChat()
        {
            var syn = new System.Windows.Forms.Form();               //wow...I actually need a freaking form object instantiated here to get these timers to run on this thread -__-
            ISynchronizeInvoke synob = (ISynchronizeInvoke)syn;

            ClientMsgCache.ClientAdded(cType);
            GlobSyn.MsgFromClient(cType, CID, connection.RemoteEndPoint, "ClientSetup <EOF>");
            //GlobSyn.MsgFromClient(cType, CID, connection.RemoteEndPoint, "ACK_Status <EOF>");

            timKeepAlive = new System.Timers.Timer();
            timKeepAlive.SynchronizingObject = synob;
            timKeepAlive.Elapsed += new ElapsedEventHandler(KeepAliveTO);
            timKeepAlive.Interval = 1000 * 20;  //at 20 seconds, 18 bytes per exchange, should be 2.3MB/month
            timKeepAlive.Start();

            timSendChk = new System.Timers.Timer();
            timSendChk.SynchronizingObject = synob;
            timSendChk.Elapsed += new ElapsedEventHandler(GrabTransmissionInCache); //check for message to send every 100ms
            timSendChk.Interval = 100;
            timSendChk.Start();

            timRecMsgEscape = new System.Timers.Timer();
            timRecMsgEscape.SynchronizingObject = synob;
            timRecMsgEscape.Elapsed += new ElapsedEventHandler(recMsgTO);
            timRecMsgEscape.Interval = 1000;
            
            connection.Blocking = false;

            try
            {
                while (true)
                {
                    string retstr = null;

                    do
                    {
                        retstr = ReceiveWholeMsg();
                    } while (retstr == null);

                    GlobSyn.MsgFromClient(cType, CID, connection.RemoteEndPoint, retstr);  

                    //if (retstr.Contains("ACK_Status "))
                    //{
                        timKeepAlive.Stop();
                        timKeepAlive.Interval = 20000;
                        waitingForAck = false;
                        timKeepAlive.Start();
                    //}
                }
            }     
            catch(Exception Ex)
            {
                if(connection != null)
                    GlobSyn.Log("FAILED TCP Client Connection reading from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);    
            }
        }

        public string ReceiveWholeMsg()
        {
            bool msghere = false;
            string data = null;
            int bytesRec = 0;
            bytes = new byte[1024];
            try
            {
                do
                {
                    bytesRec = connection.Receive(bytes);// - 2; //bytes rec getting set to two higher than contents of bytes? hmm. Does CR LF not get stored in bytes but is somehow in bytesRec??

                    if (bytesRec > 0) //lock into receiving message until <EOF> detected
                    {
                        msghere = true;
                        timRecMsgEscape.Start();
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    }

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        timRecMsgEscape.Stop();
                        return data;
                    }
                } while (msghere);
            }
            catch (SocketException Ex)
            {
                if (Ex.ErrorCode == 10060)
                {
                    GlobSyn.Log("FAILURE! Incomplete message! 500ms TIMEOUT from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
                    bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
                }
            }
            catch (Exception Ex)
            {
                GlobSyn.Log("FAILURE! Error while receving message from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
                bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
            }

            return null; //no message here *weeps*  WHY WON'T ANYBODY TALK TO ME?!  ACK <EOF>

        }

        private void GrabTransmissionInCache(object derp, ElapsedEventArgs e)
        {
            string msg;
            msg = ClientMsgCache.Read(CID, cType);
            
            try
            {
                if (msg != null)
                {
                    transmittingNow = true;
                    bytes = Encoding.ASCII.GetBytes(msg);
                    connection.Send(bytes);
                    transmittingNow = false;
                }
 
                timSendChk.Start();
            }
            catch(Exception Ex)
            {
                GlobSyn.Log("FAILURE! Error while xmitting to client " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
            }
        }



        private void KeepAliveTO(object derp, ElapsedEventArgs e)
        {
            timKeepAlive.Stop();
            if (KeepAliveEnabled)
            {
                //if (waitingForAck)  //was not disabled by ACK, CONNECTION IS DEAD
                //{
                    //kill this whole thang?
                    GlobSyn.Log("FAILURE! Client is dead at " + connection.RemoteEndPoint.ToString() + " he isn't keeping up with ACKs :'(");
                    waitingForAck = false;
                    bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
                //}
                //else if (!transmittingNow) //only send ackreq if not actively transmitting already
                //{
                //    try
                //    {
                //        //bytes = Encoding.ASCII.GetBytes("AM I BEING SENT TO THE FUCKING SAME PC?! aint no localhost socket, son <EOF>");
                //        bytes = Encoding.ASCII.GetBytes("REQ <EOF>");
                //        connection.Send(bytes);
                //        waitingForAck = true;
                //        timKeepAlive.Interval = 1000 * 5; //expect reply within 400ms
                //        timKeepAlive.Start();
                //    }
                //    catch(Exception Ex) 
                //    {
                //        GlobSyn.Log("FAILURE! I was tryna send an ACK_Status but something got weird with:"  + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
                //    }
                //}
                //else
               // {
               //     timKeepAlive.Start();
                //}
            }
            
        }
        
        private void recMsgTO(object derp, ElapsedEventArgs e)
        {
            bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
        }
        
        private byte ThisConnectionSucks()
        {
            disposing = true;// don't let this thread call this thang more than once!
            ClientMsgCache.ClientRemoved(cType);
            GlobSyn.MsgFromClient(cType, CID, connection.RemoteEndPoint, "ClientKilled <EOF>");

            timKeepAlive.Stop();
            timRecMsgEscape.Stop();
            timSendChk.Stop();

            timKeepAlive.Dispose();
            timRecMsgEscape.Dispose();
            timSendChk.Dispose();

            SelfDestruct(this); //if I abort the thread I am running on...wat..wait BRAIN MELT??!?!? will this be reached? put abort afterwards...lesse
            connThread.Abort();
            
            return 1;
        }
    }

}



       
