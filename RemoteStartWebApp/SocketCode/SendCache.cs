using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteStartWebApp
{

    class CacheMessage
    {
        string _msg;
        bool consumed;

        public CacheMessage(string msg)  //only created by main thread for the RPI, timeouts occur on main thread??
        {
            _msg = msg;
        }

        public string MessageString { get { return _msg; } }
        public bool IsMsgConsumed { get { return consumed; } }

        public bool ConsumeThisMsg()
        {
            consumed = true;
            return true;
        }

    }

    static class ClientMsgCache //Produced at RPI via xchgserver, read/consumed by RemoteControlClients
    {
        private static ReaderWriterLockSlim toPageLock = new ReaderWriterLockSlim();
        private static ReaderWriterLockSlim toSrvrLock = new ReaderWriterLockSlim();

        private static List<CacheMessage> MessagesToServer = new List<CacheMessage>();
        private static string MessageToPage;    //don't use a list...only the most recent message to the page is helpful anyways, just overwrite 


        //private static bool connected;

        //public static void ServerAdded()
        //{
        //    toSrvrLock.EnterWriteLock();
        //    connected = true;
        //    toSrvrLock.ExitWriteLock();
        //}
        //public static void ServerGone()
        //{
        //    toSrvrLock.EnterWriteLock();
        //    connected = false;
        //    toSrvrLock.ExitWriteLock();
        //}

        public static string ReadMsgForServer()   //CID 0 is reserved for RPI
        {
            toSrvrLock.EnterReadLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                for(int i=0; i < MessagesToServer.Count; i++)
                {
                    if (MessagesToServer[i].ConsumeThisMsg())  //returns true if just consumed by thread
                    {
                        string temp = MessagesToServer[i].MessageString;
                        MessagesToServer.RemoveAt(i);
                        return temp;
                    }
                }

                return null; //thread already consumed all existing messages
            }
            finally
            {
                toSrvrLock.ExitReadLock();
            }
        }


        public static string ReadMsgForPage()   //CID 0 is reserved for RPI
        {
            toPageLock.EnterReadLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                //for (int i = 0; i < MessagesToPage.Count; i++)
                //{
                //    if (MessagesToPage[i].ConsumeThisMsg())  //returns true if just consumed by thread
                //    {
                //        string temp = MessagesToPage[i].MessageString;
                //        MessagesToPage.RemoveAt(i);
                //        return temp;
                //    }
                //}
                string temp = MessageToPage;
                MessageToPage = null;       //GC will cleanup, null the string
                return temp;//thread already consumed all existing messages
            }
            finally
            {
                toPageLock.ExitReadLock();
            }
        }

        public static void AddMessageToServer(string cmsg)
        {
            toSrvrLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                //foreach (CacheMessage m in MessagesToServer.ToList())  //cleanup old messages
                //{
                //    if (m.IsMsgConsumed)
                //        MessagesToServer.Remove(m);
                //}

                MessagesToServer.Add(new CacheMessage(cmsg));
                //else
                //GlobSyn.Log("WARNING Ain't got no clients to talk to.. Why u talkin? " + Environment.NewLine + "~~Contents:" + cmsg);   meh...can still ack, no need
            }
            finally
            {
                toSrvrLock.ExitWriteLock();
            }
        }

        public static void AddMessageToPage(string cmsg)
        {
            toPageLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {

                //foreach (CacheMessage m in MessagesToPage.ToList())  //cleanup old messages
                //{
                //    if (m.IsMsgConsumed)
                //        MessagesToPage.Remove(m);
                //}

                MessageToPage = cmsg;  //overwrite whatever is there!
                //else
                //GlobSyn.Log("WARNING Ain't got no clients to talk to.. Why u talkin? " + Environment.NewLine + "~~Contents:" + cmsg);   meh...can still ack, no need
            }
            finally
            {
                toPageLock.ExitWriteLock();
            }
        }
    }

}
