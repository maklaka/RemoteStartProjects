using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Net;

namespace RemoteStartWebApp
{
    public static class SessionManager
    {
        static private List<TCPClientConn> sessions = new List<TCPClientConn>();

        public static void AddSession(string sid)
        {
            sessions.Add(new TCPClientConn(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001), sid));
            
        }
    }
}