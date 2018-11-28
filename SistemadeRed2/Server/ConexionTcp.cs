using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ConexionTcp
    {
        public System.Net.Sockets.TcpClient TcpClient;
        public StreamReader StreamReader;
        public StreamWriter Streamwriter;
        public Thread ReadThread;

        public delegate void DataCarrier(string data);
        public event DataCarrier OnDataReceived;

        public delegate void DisconnectNotify();
        public event DisconnectNotify OnDisconnect;

        public delegate void ErrorCarrier(Exception e);
        public event ErrorCarrier OnError;

        public ConexionTcp(TcpClient client)
        {
            var ns = client.GetStream();
            StreamReader = new StreamReader(ns);
            Streamwriter = new StreamWriter(ns);
            TcpClient = client;
        }

        private void EscribirMsj(string mensaje)
        {
            try
            {
                Streamwriter.Write(mensaje + '\0');
                Streamwriter.Flush();
            }
            catch (Exception e)
            {
                if (OnError != null)
                    OnError(e);
            }
        }

        public void EnviarPaquete(Paquete paquete)
        {
            EscribirMsj(paquete);
        }
    }
}
