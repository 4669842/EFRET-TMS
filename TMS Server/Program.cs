using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TMS_Server
{
    class Program
    {
        static LidgrenWorkThread workerObject;
        static Thread workerThread;
        static NetPeerConfiguration serverconfig;
        static NetPeerConfiguration clientconfig;
        static NetServer server;
        static NetClient client;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            NetPeerConfiguration serverconfig = new NetPeerConfiguration("EFRET");
            serverconfig.Port = 8081;
            serverconfig.MaximumConnections = 100;
            server = new NetServer(serverconfig);
            Thread serverthread = new Thread(StartServer);
            serverthread.Start();

            bool startServer = true;
            if (startServer)
            {
                //Create Thread
                workerObject = new LidgrenWorkThread();
                workerThread = new Thread(workerObject.SyncComps);
                //Start Thread
                workerThread.Start();             
            }


            #region use elsewhere
            //Loop until worker thread active
            //while (!workerThread.IsAlive) ;

            //put the main thread to sleep to allow worker thread to do some work
            //Thread.Sleep(1);

            //stop worker thread
            //workerObject.RequestStop();

            //use join method
            //workerThread.Join();
            #endregion
        }


        static void AcceptConsoleInput()
        {
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                NetOutgoingMessage om = client.CreateMessage(input);
                client.SendMessage(om, NetDeliveryMethod.ReliableSequenced);
                client.FlushSendQueue();
            }

            AcceptConsoleInput();
        }

        static void StartServer()
        {
            server.Start();
            NetIncomingMessage message;

            while (true)
            {
                message = server.WaitMessage(500);
                if (message != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage response = server.CreateMessage();
                            response.Write((byte)1); // Do I need to do this?
                            server.SendDiscoveryResponse(response, message.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("(Server) Debug: " + message.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();

                            string reason = message.ReadString();
                            Console.WriteLine(NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                            if (status == NetConnectionStatus.Connected)
                                Console.WriteLine("Remote hail: " + message.SenderConnection.RemoteHailMessage.ReadString());
                            break;
                        case NetIncomingMessageType.Data:
                            // incoming chat message from a client
                            string chat = message.ReadString();


                            // broadcast this to all connections, except sender
                            List<NetConnection> all = server.Connections; // get copy
                                                                          //all.Remove(message.SenderConnection);
                            Console.WriteLine(all + "hello ");

                            NetOutgoingMessage om = server.CreateMessage();
                            server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);

                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("(Server) Unrecognized message type! (" + message.MessageType + ")");
                            break;
                    }
                }
                server.Recycle(message);
            }
            Thread.Sleep(1);
            AcceptConsoleInput();
        }
    }
}
