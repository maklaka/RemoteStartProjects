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
    public delegate void SockMsg(ClientListener rem, String msg);

    public class XChngServer 
    {
        List<ClientListener> RemCliConns;
        //public RemConCliListener RPIClient;
        frmServiceLog parentForm;
        
        private SockMsg MsgFromClient;
        private LogMsg TakeThisLogMsg;
        

        public XChngServer(frmServiceLog par)
        {
            parentForm = par;

            RemCliConns = new List<ClientListener>();

            TakeThisLogMsg = new LogMsg(OnLogMsg);
            MsgFromClient = new SockMsg(OnMsgFromClient);

            GlobSyn.gTakeThisLogMsg = TakeThisLogMsg;
            GlobSyn.gMsgFromClient = MsgFromClient;

            try
            {
                var temp = new ClientListener("10.0.0.8", 10000, ClientType.ConsumerClient);
                RemCliConns.Add(temp);
            }
            catch (Exception ex)
            {
                GlobSyn.Log("FAILED to create client listener" + Environment.NewLine + "~~See Exception:" + ex.Message);
            }
            //RPIClient = new RemConCliListener("10.0.0.8", 1999, MsgFromClient, TakeThisLogMsg, (ISynchronizeInvoke)this);
           
            
        }

        public void OnLogMsg(String msg)
        {
            parentForm.txtLog.Invoke((MethodInvoker)(() => parentForm.txtLog.Text = ">> " + DateTime.Now.TimeOfDay + " - " + msg + Environment.NewLine + parentForm.txtLog.Text));
        }
        public void OnMsgFromClient(ClientListener rem, String msg)
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
