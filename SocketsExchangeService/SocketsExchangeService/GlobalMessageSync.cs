using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketsExchangeService
{
    public delegate void LogMsg(String msg);
    public delegate void SockMsg(ClientType ctSource, uint CID, EndPoint ipep, string msg);

    public static class GlobSyn
    {
        public static ISynchronizeInvoke gSync;
        public static SockMsg gMsgFromClient;
        public static LogMsg gTakeThisLogMsg;

        public static void Log(string s)
        {
            if (gTakeThisLogMsg != null && gSync != null)
            {
                GlobSyn.gSync.Invoke(GlobSyn.gTakeThisLogMsg, new object[1] { s });
            }
        }

        public static void MsgFromClient(ClientType ctSource, uint CID, EndPoint ipep, string msg)  //add type  (rpi and control client?)
        {
            if (gTakeThisLogMsg != null && gSync != null)
            {
                GlobSyn.gSync.Invoke(gMsgFromClient, new object[4] { ctSource, CID, ipep, msg });
            }
        }

    }
}
