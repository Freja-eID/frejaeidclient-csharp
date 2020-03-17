using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http.Tests
{
    [TestClass]
    public class HttpServer
    {
        private readonly HttpListener listener;
        private readonly string LOCALHOST_URL = "http://localhost";
        private readonly Action<HttpListenerContext> handler;

        public HttpServer(int port, Action<HttpListenerContext> handler)
        {
            this.handler = handler;
            this.listener = new HttpListener();
            this.listener.Prefixes.Add(LOCALHOST_URL + ":" + port + "/");
        }

        public async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync().ConfigureAwait(false);
                handler.Invoke(ctx);
            }
        }

        public void Start()
        {
            listener.Start();

            Task listenTask = HandleIncomingConnections();
        }

        public void Stop()
        {
            listener.Abort();
        }

    }
}
