using System;
using System.Net;
using System.Net.Sockets;

namespace EmbedInfluxDB
{
    public class InfluxServerBuilder
    {
        public static int FreePort = -1;

        private int _port;
        private string _user;
        private string _pass;
        private bool _ssl;

        public InfluxServerBuilder()
        {
            _port = FreePort;
        }

        public InfluxServerBuilder With_Port(int port)
        {
            // todo : if free port, pick a port
            _port = port;
            return this;
        }

        //public InfluxServerBuilder With_Auth(string user, string pass)
        //{
        //    _user = user;
        //    _pass = pass;
        //    return this;
        //}

        //public InfluxServerBuilder With_Ssl()
        //{
        //    _ssl = true;
        //    return this;
        //}

        public InfluxServer Build()
        {
            if(_port == FreePort)
            {
                static int FreeTcpPort()
                {
                    var l = new TcpListener(IPAddress.Loopback, 0);
                    l.Start();
                    int port = ((IPEndPoint)l.LocalEndpoint).Port;
                    l.Stop();
                    return port;
                }

                _port = FreeTcpPort();
            }

            return new InfluxServer
            {
                Port = _port,
                User = _user,
                Pass = _pass,
                UseSsl = _ssl
            };
        }
        
    }
}
