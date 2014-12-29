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
    class XChngServer
    {
        List<RemoteControlClient> RemCliConns;
        RemoteControlClient RPIClient;
        frmServiceLog parentForm;
        private  static ISynchronizeInvoke _synch;


        public delegate void SockMsg(RemoteControlClient rem, String msg);
        //public delegate void LogMsg(String msg);

        public SockMsg MsgFromClient;
        public  LogMsg TakeThisLogMsg;




        public XChngServer(frmServiceLog par)
        {
            
            parentForm = par;
            RemCliConns = new List<RemoteControlClient>();
            RemCliConns.Add(new RemoteControlClient(this));  //start with one control client listener in a background thread
            RPIClient = new RemoteControlClient(this, "10.0.0.8", 1999);
           
            TakeThisLogMsg = new LogMsg(OnLogMsg);
            MsgFromClient = new SockMsg(OnMsgFromClient);
        }

        public void OnLogMsg(String msg)
        {
            parentForm.txtLog.Invoke((MethodInvoker)(() => parentForm.txtLog.Text += Environment.NewLine + msg));
        }
        public void OnMsgFromClient(RemoteControlClient rem, String msg)
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
