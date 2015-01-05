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
        static private List<RemConWebSession> sessions = new List<RemConWebSession>();

        public static void AddSession(string sid)
        {
            sessions.Add(new RemConWebSession(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001), sid));
            
        }
        public static ClientMsgCache MyCache(string sid)
        {
            for(int i=0; i<sessions.Count; i++)
            {
                if(sessions[i].SessionID == sid)
                {
                    return sessions[i].Cache;
                }             
            }
            return null;
        }
    }


    public class RemConWebSession
    {
        private ClientMsgCache _msgCache;
        private TCPClientConn _conn;
        private string _SessionID;


        public RemConWebSession(IPEndPoint ipep, string sid)
        {
            _msgCache = new ClientMsgCache();
            _conn = new TCPClientConn(ipep, _msgCache);
            _SessionID = sid;

        }

        public ClientMsgCache Cache
        {
            get { return _msgCache; }
            set { _msgCache = value; }
        }

        public TCPClientConn Connection
        {
            get { return _conn; }
            set { _conn = value; }
        }

        public string SessionID 
        { 
            get { return _SessionID; } 
            set { _SessionID = value; } 
        }
    }
}