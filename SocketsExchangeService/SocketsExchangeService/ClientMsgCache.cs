using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

namespace SocketsExchangeService
{
    class CacheMessage
    {
        string _msg;
        int NumClientsAtCreation;
        List<uint> ThreadNums;
        ClientType targetClientType;

        System.Timers.Timer t;
        bool expired = false;

        public CacheMessage(string msg, int num, ClientType ct)  //only created by main thread for the RPI, timeouts occur on main thread??
        {
            ThreadNums = new List<uint>();
            _msg = msg;
            NumClientsAtCreation = num;
            targetClientType = ct;

            t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(IAmOld);
            t.Interval = 3000;
            t.Enabled = true;
              
        }
        private void IAmOld(object derp, ElapsedEventArgs e)
        {
            if (!expired)
            {
                expired = true;
                GlobSyn.Log("WARNING A message timed out before reaching all clients " + Environment.NewLine + "~~Contents:" + _msg);
            }
        }

        public void DestroyTimeout()
        {
            t.Stop();
            t = null;
        }

        
        public bool Expired{get{return expired;}}
        public string MessageString{get{ return _msg;}}
        public bool IsMsgConsumed{ get { return (ThreadNums.Count >= NumClientsAtCreation); } }
        public ClientType TargetClientType { get { return targetClientType; } }

        public bool ConsumeThisMsg(uint ClientID)
        {
            if (!ThreadNums.Contains(ClientID))
            {
                ThreadNums.Add(ClientID);
                return true;
            }
            return false;    
        }

    }

    static class ClientMsgCache //Produced at RPI via xchgserver, read/consumed by RemoteControlClients
    {
        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private static List<CacheMessage> MessagesToClients = new List<CacheMessage>();
        private static int numControlClients = 0;
        private static int numRPIClients = 0;
        private static uint lastClientID = 0;

        public static void ClientAdded(ClientType ct)
        {
            cacheLock.EnterWriteLock();
            if (ct == ClientType.ConsumerClient)
                numControlClients++;
            else if (ct == ClientType.RPIProducerClient)
                numRPIClients++;
            cacheLock.ExitWriteLock();
        }
        public static void ClientRemoved(ClientType ct)
        {
            cacheLock.EnterWriteLock();
            if (ct == ClientType.ConsumerClient)
                numControlClients--;
            else if (ct == ClientType.RPIProducerClient)
                numRPIClients--;
            cacheLock.ExitWriteLock();
        }
        public static int NumTotalClients
        {
            get { return numControlClients; }
        }

        public static uint GenerateClientID()
        {
            cacheLock.EnterWriteLock(); 
            lastClientID += 1;
            cacheLock.ExitWriteLock();
            return lastClientID;   
        }

        public static string Read(uint CID, ClientType ct)   //CID 0 is reserved for RPI
        {
            cacheLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                foreach (CacheMessage cmsg in MessagesToClients.ToList())
                {
                    if (cmsg.TargetClientType == ct)
                    {
                        if (cmsg.ConsumeThisMsg(CID))  //returns true if just consumed by thread
                            return cmsg.MessageString;

                        if (cmsg.IsMsgConsumed)         //check here to see if everybody ate this message already
                        {
                            cmsg.DestroyTimeout();
                            MessagesToClients.Remove(cmsg);
                        }
                    }
                }
 
                return null; //thread already consumed all existing messages
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public static void AddMessage(string cmsg, ClientType ct)
        {
            cacheLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {   if(numControlClients > 0)
                    MessagesToClients.Add(new CacheMessage(cmsg, numControlClients, ct));
                else
                    GlobSyn.Log("WARNING Ain't got no clients to talk to.. Why u talkin? " + Environment.NewLine + "~~Contents:" + cmsg);   
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public static void RemoveConsumedMessages(int key)  //check here every 2 seconds for messages that have expired (3 second expiration)
        {
            
            cacheLock.EnterWriteLock();
            foreach (CacheMessage cmsg in MessagesToClients)
            {
                if(cmsg.Expired)
                    MessagesToClients.Remove(cmsg);
            }
            cacheLock.ExitWriteLock();
        }
    }
 
}
