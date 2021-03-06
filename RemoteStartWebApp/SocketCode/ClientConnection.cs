﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;
using System.Timers;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;



namespace RemoteStartWebApp
{

    public delegate void ClientKiller(TCPClientConn conn);

    public class TCPClientConn
    {
        private Thread connThread;
        private System.Timers.Timer timSendChk;
        //private System.Timers.Timer timRecMsgEscape;

        //private bool waitingForAck = false;
        //private bool transmittingNow = false;
        private byte[] bytes;
        Socket connection;
        IPEndPoint _remoteEP;
        public ClientKiller SelfDestruct;

        private bool disposing = false;
        ClientMsgCache _msgCache;

        //public ClientKiller SelfDestruct;

        public TCPClientConn(IPEndPoint ipep, ClientMsgCache cache)
        {
            _remoteEP = ipep;
            _msgCache = cache;
            connThread = new Thread(new ThreadStart(BidiChat));
            connThread.Start();
        }

        private void BidiChat()
        {
            connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connection.Connect(_remoteEP);
            bytes = Encoding.ASCII.GetBytes("This is my connection message <EOF>");
            // Send the data through the socket.
            int bytesSent = connection.Send(bytes);

            timSendChk = new System.Timers.Timer();
            timSendChk.Elapsed += new ElapsedEventHandler(GrabTransmissionInCache); //check for message to send every 100ms
            timSendChk.Interval = 1000;
            timSendChk.Start();

            //timRecMsgEscape = new System.Timers.Timer();
            //timRecMsgEscape.SynchronizingObject = synob;
            //timRecMsgEscape.Elapsed += new ElapsedEventHandler(recMsgTO);
            //timRecMsgEscape.Interval = 1000;

            connection.Blocking = false;

            try
            {
                while (true)
                {
                    string retstr = null;

                    do
                    {
                        Thread.Sleep(10);
                        retstr = ReceiveWholeMsg();
                    } while (retstr == null);


                    _msgCache.AddMessageToPage(retstr);
                    //Cac.MsgFromServer(sendy.RemoteEndPoint, retstr);
                
                }
            }
            catch (Exception Ex)
            {
                //if (connection != null)
                    //GlobSyn.Log("FAILED TCP Client Connection reading from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
            }
        }

        public string ReceiveWholeMsg()
        {
            bool msghere = false;
            string data = null;
            int bytesRec = 0;
            bytes = new byte[1024];
            try
            {
                do
                {
                    bytesRec = connection.Receive(bytes); //bytes rec getting set to two higher than contents of bytes? hmm. Does CR LF not get stored in bytes but is somehow in bytesRec??

                    if (bytesRec > 0) //lock into receiving message until <EOF> detected
                    {
                        msghere = true;
                        //timRecMsgEscape.Start();
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    }

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        _msgCache.AddMessageToServer("ACK_Status <EOF>");
                        return data;
                    }
                } while (msghere);
            }
            catch (SocketException Ex)
            {
                if (Ex.ErrorCode == 10060)
                {
                   // GlobSyn.Log("FAILURE! Incomplete message! 500ms TIMEOUT from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
                    //bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
                }
            }
            catch (Exception Ex)
            {
                //GlobSyn.Log("FAILURE! Error while receving message from " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
                //bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
            }

            return null; //no message here *weeps*  WHY WON'T ANYBODY TALK TO ME?!  ACK <EOF>

        }

        private void GrabTransmissionInCache(object derp, ElapsedEventArgs e)
        {
            string msg;
            msg = _msgCache.ReadMsgForServer();

            try
            {
                if (msg != null)
                {
                    bytes = Encoding.ASCII.GetBytes(msg);
                    connection.Send(bytes);
                }

                timSendChk.Start();
            }
            catch (Exception Ex)
            {
                //GlobSyn.Log("FAILURE! Error while xmitting to client " + connection.RemoteEndPoint.ToString() + Environment.NewLine + "~~See Exception: " + Ex.Message);
            }
        }

        private void recMsgTO(object derp, ElapsedEventArgs e)
        {
            //bytes[0] = disposing ? (byte)0 : ThisConnectionSucks();
        }

        private byte ThisConnectionSucks()
        {
            disposing = true;// don't let this thread call this thang more than once!
            //GlobSyn.MsgFromServer(connection.RemoteEndPoint, "ClientKilled <EOF>");

            //timRecMsgEscape.Stop();
            timSendChk.Stop();

            //timRecMsgEscape.Dispose();
            timSendChk.Dispose();

            //syn.Dispose();

            SelfDestruct(this); //if I abort the thread I am running on...wat..wait BRAIN MELT??!?!? will this be reached? put abort afterwards...lesse
            connThread.Abort();

            return 1;
        }
    }
}