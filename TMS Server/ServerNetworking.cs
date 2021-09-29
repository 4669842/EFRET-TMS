﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_Server
{
    class ServerNetworking
    {
        static NetPeerConfiguration config;
        NetServer server;

        NetConnection recipient;
        NetPeer netpeer;

        // schedule initial sending of position updates
        double nextSendUpdates;

        public ServerNetworking()
        {
            config = new NetPeerConfiguration("EFRET");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.Port = 14242;
            server = new NetServer(config);
            server.Start();
            if (server.Status == NetPeerStatus.Running)
            {
                Console.WriteLine("Server is running on port " + config.Port);
            }
            else
            {
                Console.WriteLine("Server not started...");
            }
            nextSendUpdates = NetTime.Now;
            netpeer = new NetPeer(config);
            recipient.Peer.DiscoverLocalPeers(config.Port);
        }

        public void Receive()
        {
            NetIncomingMessage msg;
            Dictionary<string, string> Stuff;
            var clients = new List<NetPeer>();
            while ((msg = server.ReadMessage()) != null)
            {
                Stuff = null;

                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        //
                        // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                        //
                        server.SendDiscoveryResponse(null, msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //
                        // Just print diagnostic messages to console
                        //
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine(msg.SenderConnection.Status);
                        if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine($"{msg.SenderConnection.Peer.Configuration.BroadcastAddress} has connected.");
                        }
                        if (msg.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            clients.Remove(msg.SenderConnection.Peer);
                            Console.WriteLine($"{msg.SenderConnection.Peer.Configuration.LocalAddress} has disconnected.");
                        }
                        break;
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        if (status == NetConnectionStatus.Connected)
                        {
                            clients.Add(msg.SenderConnection.Peer);

                            //
                            // A new player just connected!
                            //
                            Console.WriteLine("*******************************************");
                            Console.WriteLine("** User IP Address: " + msg.SenderEndPoint.Address + " connected! **");
                            Console.WriteLine("** Special Handshake Command: "+msg.SenderConnection.RemoteHailMessage);
                            Console.WriteLine("** Unique Instance ID (Fault Logging): " + NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " **");
                            Console.WriteLine("** Connected Peer: "+msg.SenderConnection.Peer + " **");
                            Console.WriteLine("** Avg Roundtrip for connection: "+msg.SenderConnection.AverageRoundtripTime + " **");
                            Console.WriteLine("** Server Connect Count: "+server.ConnectionsCount+ " **");
                            Console.WriteLine("** Server Status: "+server.Status + " **");
                            Console.WriteLine("*******************************************");
  
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        //
                        // The client sent input to the server
                        //

                        Console.WriteLine("i got smth!");
                        var data = DeserializeData(msg.ReadString());
                        Console.WriteLine(data);

                        break;
                    default:
                        //Transfer Data from message into string
                        Stuff = DeserializeData(msg.ReadString());
                        //CannonManager.ShootDoggy();
                        break;
                }


                //
                // send position updates 30 times per second
                //
                double now = NetTime.Now;
                if (now > nextSendUpdates)
                {
                    // Yes, it's time to send position updates

                    // for each player...
                    foreach (NetConnection player in server.Connections)
                    {
                        Send(player, "test");
  
                        // ... send information about every other player (actually including self)
                        foreach (NetConnection otherPlayer in server.Connections)
                        {

                        }

                        if (Stuff != null)
                            Send(player, SerializeData(Stuff));
                    }

                    // schedule next update
                    nextSendUpdates += (1.0 / 30.0);
                }


                server.Recycle(msg); //what this do
            }
        }
        public void Send(NetConnection recipient, string serializedData)
        {
            NetOutgoingMessage sendMsg = server.CreateMessage();

            sendMsg.Write(serializedData);

            server.SendMessage(sendMsg, recipient, NetDeliveryMethod.ReliableUnordered);
        }
 

        public static string SerializeData(Dictionary<string, string> data)
        {
            string pairs = "";

            foreach (string key in data.Keys)
            {
                pairs += ";" + key + "=" + data[key];
            }

            return pairs.TrimStart(';');
        }

        public static Dictionary<string, string> DeserializeData(string SerializedData)
        {
            Dictionary<string, string> DeserializeData = new Dictionary<string, string>();

            string[] pairs = SerializedData.Split(';');

            for (int i = 0; i < pairs.Length; i++)
            {
                string[] kv = pairs[i].Split('=');
                DeserializeData.Add(kv[0], kv[1]);
            }

            return DeserializeData;
        }
    }
}
