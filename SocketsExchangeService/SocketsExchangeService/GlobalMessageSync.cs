using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SocketsExchangeService
{
    public static class GlobSyn
    {
        public static ISynchronizeInvoke gSync;
        public static SockMsg gMsgFromClient;
        public static LogMsg gTakeThisLogMsg;


        public static void Log(string s)
        {
            GlobSyn.gSync.Invoke(GlobSyn.gTakeThisLogMsg, new object[1] { s });
        }

        public static void MsgFromClient(RemConCliListener cl, string s)  //add type  (rpi and control client?)
        {
            GlobSyn.gSync.Invoke(gMsgFromClient, new object[2] { cl, s });
        }
    }
}
