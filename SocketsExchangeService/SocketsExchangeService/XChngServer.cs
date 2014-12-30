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

    public delegate void LogMsg(String msg);
    public delegate void SockMsg(RemConCliListener rem, String msg);

    public class XChngServer 
    {
        List<RemConCliListener> RemCliConns;
        //public RemConCliListener RPIClient;
        frmServiceLog parentForm;
        


        
        //public delegate void LogMsg(String msg);

        private ISynchronizeInvoke Sync;
        private SockMsg MsgFromClient;
        private LogMsg TakeThisLogMsg;
        

        public XChngServer(frmServiceLog par, ISynchronizeInvoke s)
        {
            parentForm = par;
            Sync = s;
            //MsgFromClient = sock;
            //TakeThisLogMsg = log;

            RemCliConns = new List<RemConCliListener>();
            //RemCliConns.Add(new RemConCliListener("127.0.0.1", 10000, MsgFromClient, TakeThisLogMsg, (ISynchronizeInvoke)this));  //start with one control client listener in a background thread

            TakeThisLogMsg = new LogMsg(OnLogMsg);
            MsgFromClient = new SockMsg(OnMsgFromClient);

            GlobSyn.gTakeThisLogMsg = TakeThisLogMsg;
            GlobSyn.gMsgFromClient = MsgFromClient;

            try
            {
                var temp = new RemConCliListener("10.0.0.8", 10000);
                RemCliConns.Add(temp);
            }
            catch (Exception ex)
            {
                while (true) { parentForm = par; }
            }
            //RPIClient = new RemConCliListener("10.0.0.8", 1999, MsgFromClient, TakeThisLogMsg, (ISynchronizeInvoke)this);
           
            
        }

        public void OnLogMsg(String msg)
        {
            parentForm.txtLog.Invoke((MethodInvoker)(() => parentForm.txtLog.Text = ">> " + DateTime.Now.TimeOfDay + " - " + msg + Environment.NewLine + parentForm.txtLog.Text));
        }
        public void OnMsgFromClient(RemConCliListener rem, String msg)
        {
            if(msg.Contains("ClientSetup"))
            {
                string ip;
                int port;
                ip = msg.Substring(msg.IndexOf("IP:")).Split(' ')[0].Replace("IP:","");
                port = Convert.ToInt32(msg.Substring(msg.IndexOf("PORT:")).Split(' ')[0].Replace("PORT:",""));
                rem.SetupSendToClientSocket(ip, port);
            }
            else if(msg.Contains("rpiSetup"))
            {
                string ip;
                int port;
                ip = msg.Substring(msg.IndexOf("IP:")).Split(' ')[0].Replace("IP:", "");
                port = Convert.ToInt32(msg.Substring(msg.IndexOf("PORT:")).Split(' ')[0].Replace("PORT:", ""));
                rem.SetupSendToClientSocket(ip, port);
            }
            else if(msg.Contains("StartCar"))
            {

            }
            else
            {

            }
        }

    }
}
