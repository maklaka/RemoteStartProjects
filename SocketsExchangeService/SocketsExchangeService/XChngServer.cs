using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Timers;
using System.Runtime.Remoting.Messaging;

namespace SocketsExchangeService 
{
    public class XChngServer 
    {
        List<ClientListener> RemCliConns;
        //public RemConCliListener RPIClient;
        frmServiceLog parentForm;

        LatestRPIInfo LatestRPI;
        
        private SockMsg MsgFromClient;
        private LogMsg TakeThisLogMsg;
        

        public XChngServer(frmServiceLog par)
        {
            parentForm = par;
            LatestRPI = new LatestRPIInfo();
            RemCliConns = new List<ClientListener>();
            

            TakeThisLogMsg = new LogMsg(OnLogMsg);
            MsgFromClient = new SockMsg(OnMsgFromClient);

            GlobSyn.gTakeThisLogMsg = TakeThisLogMsg;
            GlobSyn.gMsgFromClient = MsgFromClient;

            try
            {
                var temp = new ClientListener("127.0.0.1", 24235, ClientType.RPIProducerClient);
                RemCliConns.Add(temp);

                var temp2 = new ClientListener("127.0.0.1", 10001, ClientType.ConsumerClient);
                RemCliConns.Add(temp2);
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
        public void OnMsgFromClient(ClientType ctSource, uint CID, EndPoint ipep, string msg) 
        {
            if (ctSource == ClientType.ConsumerClient)
            {
                if (msg.Contains("ClientSetup"))
                {
                //ClientMsgCache.AddMessage(msg, ClientType.RPIProducerClient);
                    parentForm.lsvRemConClients.Items.Add("CID: " + CID.ToString() + " - " +ipep.ToString());

                //Package an ACK_Status of the RPI off to this client right away??!
                    LatestRPI.SendToClients();
                }
                else if(msg.Contains("ClientKilled"))
                {
                    string removelistitem = "CID: " + CID.ToString() + " - " + ipep.ToString();
                    foreach (ListViewItem item in parentForm.lsvRemConClients.Items)
                    { 
                        if (item.Text == removelistitem)
                        {
                            parentForm.lsvRemConClients.Items.Remove(item);
                            break;
                        }
                    }
                    //?? For now, turn off car when client DC's
                    //LatestRPI.CarState = "OFF";
                   
                }
                else if (msg.Contains("ACK_Status")) //meh, just for knowing the remconclient is alive...but respond with a full ack from rpi
                {
                    BackgroundBlipper bb = new BackgroundBlipper("CID: " + CID.ToString() + " - " + ipep.ToString(), ImaBlipAClient);
                    string removelistitem = "CID: " + CID.ToString() + " - " + ipep.ToString();
                    foreach (ListViewItem item in parentForm.lsvRemConClients.Items)
                    {
                        if (item.Text == removelistitem)
                        {
                            item.BackColor = Color.LightGreen;
                            break;
                        }
                    }

                    //LatestRPI.SendToClients();     naw...he just responded to a req     
                }
                else if (msg.Contains("StartCar"))
                {
                    ClientMsgCache.AddMessage(msg, ClientType.RPIProducerClient);
                    
                    //?? For now pass back to client that he started the car, SHHHHH.  He won't know
                    //LatestRPI.CarState = "ON";
                    //LatestRPI.SendToClients();
                }
                else
                {
                    return;
                }
            }
            else if (ctSource == ClientType.RPIProducerClient)
            {
                if (msg.Contains("ClientSetup"))  //
                {
                //update RPI info 
                    LatestRPI.Ipep = ipep.ToString(); 
                    LatestRPI.InfoTime = DateTime.Now.TimeOfDay.ToString(); ;
                    LatestRPI.RPIState = "UP";
                //prep/send message on to remcon clients
                    LatestRPI.SendToClients();
                //update form
                    parentForm.lblRPIStatus.Text = LatestRPI.RPIState;
                    parentForm.lblRPIStatus.BackColor = Color.Green;
                    parentForm.lblRPIEndPoint.Text = LatestRPI.Ipep;
                    parentForm.lblRPILastCom.Text = LatestRPI.InfoTime;
                }
                else if (msg.Contains("ClientKilled"))
                {
                // parentForm.lblRPIEndPoint.Text = "N/A";  leave the IP alone?  Will just update with next ACK_Status I s'pose
                    LatestRPI.InfoTime = DateTime.Now.TimeOfDay.ToString();
                    LatestRPI.RPIState = "DOWN";
                //prep/send message on to remcon clients
                    LatestRPI.SendToClients();
                //update form
                    parentForm.lblRPIStatus.Text = "DOWN";
                    parentForm.lblRPIStatus.BackColor = Color.Red;
                }
                else if (msg.Contains("ACK_Status"))  //Arrives With: "CarState:""ON" or "OFF" ----- Also leaves with: "RPIClientEndPoint:"IP:PORT   , "InfoTime:"LatestRPI.InfoTime    , "RPIState:""UP" or "DOWN"
                {
                //update RPI info 
                    LatestRPI.Ipep = ipep.ToString(); 
                    LatestRPI.InfoTime = DateTime.Now.TimeOfDay.ToString();                                                 
                    LatestRPI.RPIState = "UP";                                                                              
                    LatestRPI.CarState = msg.Substring(msg.IndexOf("CarState:")).Split(' ')[0].Replace("CarState:", "");   
                //prep/send message on to remcon clients
                    LatestRPI.SendToClients();
                //update form
                    parentForm.lblRPIEndPoint.Text = LatestRPI.Ipep;
                    parentForm.lblRPILastCom.Text = LatestRPI.InfoTime;
                    parentForm.pcbCarState.Image = LatestRPI.CarState == "ON" ? SocketsExchangeService.Properties.Resources.Car_On : SocketsExchangeService.Properties.Resources.Car_Off;

                //Log the transaction, MEH  probably shouldn't log every ACK_Status from the RPI.  updating the com time is fine
                    //parentForm.txtTraffic.Invoke((MethodInvoker)(() => parentForm.txtTraffic.Text = ">> " + now + " - " + "(RPI -> RemConClients)" + " ~~Contents:  " + msg + Environment.NewLine + parentForm.txtTraffic.Text));
                }
                else
                {


                }


            }
        }
        
        void ImaBlipAClient (object sender, RunWorkerCompletedEventArgs e)
        {
            string removelistitem = (string)e.Result;
            foreach (ListViewItem item in parentForm.lsvRemConClients.Items)
            {
                if (item.Text == removelistitem)
                {
                    item.BackColor = Color.White;
                    break;
                }
            }
        }
    }

    public class LatestRPIInfo
    {
        public string Ipep = "N/A";
        public string InfoTime = "N/A";
        public string RPIState = "N/A";
        public string CarState = "N/A";

        public LatestRPIInfo() { }

        public void SendToClients()
        {
            string msg = "ACK_Status <EOF>";
            //CarState already in here
            msg = msg.Insert(msg.IndexOf(" <EOF>"), " RPIClientEndPoint:" + Ipep);
            msg = msg.Insert(msg.IndexOf(" <EOF>"), " CarState:" + CarState);
            msg = msg.Insert(msg.IndexOf(" <EOF>"), " RPIState:" + RPIState);
            msg = msg.Insert(msg.IndexOf(" <EOF>"), " InfoTime:" + InfoTime);
            ClientMsgCache.AddMessage(msg, ClientType.ConsumerClient);
        }

    }
    public class BackgroundBlipper
    {
        private string clienttoBlip;
        BackgroundWorker bb;
        public BackgroundBlipper(string s, RunWorkerCompletedEventHandler eh)
        {
            clienttoBlip = s;
            bb = new BackgroundWorker();
            bb.RunWorkerCompleted += eh;
            bb.DoWork +=bb_DoWork;
            bb.RunWorkerAsync();
        }

        void bb_DoWork(object sender, DoWorkEventArgs e)
        {
 	        e.Result = clienttoBlip;
            Thread.Sleep(100);
        }

    }
}
