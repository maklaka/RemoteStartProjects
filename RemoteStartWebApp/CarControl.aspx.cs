using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace RemoteStartWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private SockMsg MsgFromServer;
        Socket sendy;
        byte[] bytes = new byte[1024];
        string data;
        bool woo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["New"] != null)
            {
                //lblWelcome.Text = "Welcome, friend!";
                //create a socket client to talk to sockets service


                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);

                // Create a TCP/IP  socket.
                sendy = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sendy.Connect(remoteEP);
                byte[] msg = Encoding.ASCII.GetBytes("This is my connection message <EOF>");

                // Send the data through the socket.
                int bytesSent = sendy.Send(msg);

                // Receive the response from the remote device.
                int bytesRec = sendy.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                woo = true;

            }
            else
                Response.Redirect("Default.aspx");



        }

        
    }
}