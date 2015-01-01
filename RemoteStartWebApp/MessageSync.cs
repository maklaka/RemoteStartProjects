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
    public delegate void SockMsg(ClientType ctSource, uint CID, EndPoint ipep, string msg);

    public static class GlobSyn
    {
        public static ISynchronizeInvoke gSync;
        public static SockMsg gMsgFromClient;

        public static void MsgFromClient(ClientType ctSource, uint CID, EndPoint ipep, string msg)  //add type  (rpi and control client?)
        {
            GlobSyn.gSync.Invoke(gMsgFromClient, new object[4] { ctSource, CID, ipep, msg });
        }

    }
}
