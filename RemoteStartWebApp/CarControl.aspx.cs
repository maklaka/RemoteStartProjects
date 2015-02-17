using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Drawing;


namespace RemoteStartWebApp
{

    public partial class CarControl : System.Web.UI.Page
    {
        public void Page_PreInit(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                SessionManager.AddSession(Session.SessionID);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["New"] != null)
            {


            }
            else
                Response.Redirect("Default.aspx");

        }    

        private void MuderThatClientAtHisBehest(TCPClientConn conn)
        {
            //taking the object out of any scope should get her good and garbage collected...eventually >.V  hopefully the thread actually stopped
            conn = null;
        }

        protected void btnStartTheCar_Click(object sender, EventArgs e)
        {
            if (lblSrvrStatus.Text == "Rpi is up and running!")
            {
                SessionManager.MyCache(Session.SessionID).AddMessageToServer("StartCar <EOF>");
                lblMessageStatus.Text = "Message sent at: " + String.Format("{0:T}", DateTime.Now);
            }
            else
            {
                btnStartTheCar.Enabled = false;
                lblMessageStatus.Text = "Message staged until client up...";

            }
            
            
        }

        protected void timUpdateMe_Tick(object sender, EventArgs e)
        {
            string msg;
            msg = SessionManager.MyCache(Session.SessionID).ReadMsgForPage();
            if (msg != null)
            {
                if (msg.Contains("ACK_Status"))
                {
                    string ipep = msg.Substring(msg.IndexOf("RPIClientEndPoint:")).Split(' ')[0].Replace("RPIClientEndPoint:", "");
                    string carstat = msg.Substring(msg.IndexOf("CarState:")).Split(' ')[0].Replace("CarState:", ""); ;
                    string rpistat = msg.Substring(msg.IndexOf("RPIState:")).Split(' ')[0].Replace("RPIState:", "");
                    string rpitime = msg.Substring(msg.IndexOf("InfoTime:")).Split('~')[0].Replace("InfoTime:", "");

                    IPEndPoint.Text = ipep;
                    lblCarStatus.Text = carstat == "ON" ? "Car is on!" : "Car is off";
                    lblSrvrStatus.Text = rpistat == "UP" ? "Rpi is up and running!" : "Rpi is down :\\";
                    LastRPIInfo.Text = rpitime;

                    lblCarStatus.BackColor = carstat == "ON" ? Color.LightGreen : Color.Red;
                    lblSrvrStatus.BackColor = rpistat == "UP" ? Color.LightGreen : Color.Red;

                    if (lblMessageStatus.Text == "Message staged until client up..." && rpistat == "UP")   //start message has been staged - is the rpi client up?
                    {
                        //send the message off and reset form state
                        SessionManager.MyCache(Session.SessionID).AddMessageToServer("StartCar <EOF>");
                        btnStartTheCar.Enabled = true;
                        lblMessageStatus.Text = "Message sent at: " + String.Format("{0:T}", DateTime.Now); 
                    }
                }

                //respond with acknowledgement no matter what
                
            }
            SessionManager.MyCache(Session.SessionID).AddMessageToServer("ACK_Status <EOF>");
            timUpdateMe.Enabled = true;
        } 
    }
}