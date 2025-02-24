using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC12_SFTPClient
{
    public class ConnectionData
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 22;

        public ConnectionData() { }

        public ConnectionData(string host, string username, string password, int port = 22)
        {
            Host = host;
            Username = username;
            Password = password;
            Port = port;
        }
    }
}
