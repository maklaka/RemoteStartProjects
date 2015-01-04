using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RemoteStartWebApp
{
    public enum ClientType { ConsumerClient, RPIProducerClient };
    public delegate void SockMsg(EndPoint ipep, string msg);

    public static class GlobSyn
    {
        public static ISynchronizeInvoke gSync;
        public static SockMsg gMsgFromServer;

        public static void MsgFromServer(EndPoint ipep, string msg)  //add type  (rpi and control client?)
        {
            GlobSyn.gSync.Invoke(gMsgFromServer, new object[2] { ipep, msg });
        }

    }
}
