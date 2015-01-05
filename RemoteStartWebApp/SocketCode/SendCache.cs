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

    public class ClientMsgCache //Produced at RPI via xchgserver, read/consumed by RemoteControlClients
    {
        private ReaderWriterLockSlim toPageLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim toSrvrLock = new ReaderWriterLockSlim();

        private List<CacheMessage> MessagesToServer = new List<CacheMessage>();
        private string MessageToPage;    //don't use a list...only the most recent message to the page is helpful anyways, just overwrite 

        public string ReadMsgForServer()   //CID 0 is reserved for RPI
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

        public string ReadMsgForPage()   //CID 0 is reserved for RPI
        {
            toPageLock.EnterReadLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                string temp = MessageToPage;
                MessageToPage = null;       //GC will cleanup, null the string
                return temp;//thread already consumed all existing messages
            }
            finally
            {
                toPageLock.ExitReadLock();
            }
        }

        public void AddMessageToServer(string cmsg)
        {
            toSrvrLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                MessagesToServer.Add(new CacheMessage(cmsg));
            }
            finally
            {
                toSrvrLock.ExitWriteLock();
            }
        }

        public void AddMessageToPage(string cmsg)
        {
            toPageLock.EnterWriteLock(); //thread will block here if there is already a thread in write mode below
            try
            {
                MessageToPage = cmsg;  //overwrite whatever is there!
            }
            finally
            {
                toPageLock.ExitWriteLock();
            }
        }
    }
}
