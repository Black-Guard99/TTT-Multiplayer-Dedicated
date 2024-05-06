using System;
using System.Net;
using LiteNetLib;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Network_Shared;
using TTT.Server.Network_Shared.Registery;
using TTT.Server.Packet_Handlers;
using TTT.Server.Games;
using LiteNetLib.Utils;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server
{
    public class NetworkServer : INetEventListener {
        private NetManager _netManager;
        //private Dictionary<int, NetPeer> connectionRequestsDictionary = new Dictionary<int, NetPeer>();
        private readonly ILogger<NetworkServer> logger;
        private readonly IServiceProvider provider;
        private UsersManager usersManager;
        private readonly NetDataWriter _cashedWriter = new NetDataWriter();
        public NetworkServer(ILogger<NetworkServer> logger,IServiceProvider provider) {
            this.logger = logger;
            this.provider = provider;
        }
        public void Start() {
            //connectionRequestsDictionary = new Dictionary<int, NetPeer>();
            _netManager = new NetManager(this)
            {
                DisconnectTimeout = 100000
            };
            _netManager.Start(9050);
            usersManager = provider.GetRequiredService<UsersManager>();
            Console.WriteLine("Server Listion on Port 9050");
        }
        public void PollEvent() {
            _netManager.PollEvents();
        }
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {

            using (var scope = provider.CreateScope()) {
                try {
                    var packetType = (PacketType)reader.GetByte();
                    var packet = ResolvePacket(packetType, reader);
                    var handler = ResolveHandler(packetType);

                    handler.Handle(packet, peer.Id);


                    reader.Recycle();

                }catch (Exception ex)
                {
                    logger.LogError(ex, "Error Processing message of type xx");
                }
            }

            /*var data = Encoding.UTF8.GetString(reader.RawData);

            Console.WriteLine($"Data Recved From Client: {data}");

            // Replying to Client Data is Recived............
            string reply = "Black Guard";
            var byteData = Encoding.UTF8.GetBytes(reply);
            peer.Send(byteData, DeliveryMethod.ReliableOrdered);*/

        }
        public void SendClient(int peerId, INetPacket packet,DeliveryMethod method = DeliveryMethod.ReliableOrdered) {
            var peer = usersManager.GetConnection(peerId).peer;
            peer.Send(WriteSerializeable(packet),method);
        }

        
        private NetDataWriter WriteSerializeable(INetPacket packet) {
            _cashedWriter.Reset();
            packet.Serialize(_cashedWriter);
            return _cashedWriter;
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            Console.WriteLine("Incoming Message From " + request.RemoteEndPoint);
            request.Accept();
        }

        

        public void OnPeerConnected(NetPeer peer) {
            //Console.WriteLine($"Client Connected to server : {peer.Port}, id {peer.Id} ");
            //connectionRequestsDictionary.Add(peer.Id, peer);
            logger.LogInformation($"Clinte Conneted to server : {peer.Address}, Id : {peer.Id}");
            usersManager.AddConnections(peer);

        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            //Console.WriteLine($"Client DisConnected to server : {peer.Port}, id {peer.Id} ");
            //connectionRequestsDictionary.Remove(peer.Id);
            var connection = usersManager.GetConnection(peer.Id);
            _netManager.DisconnectPeer(peer);
            usersManager.Disconnect(peer.Id);

            logger.LogInformation($"{connection?.user?.id} disconnnected: {peer.Address}");
        }
        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }
        public IPacketHandler ResolveHandler(PacketType packetType)
        {
            var registery = provider.GetRequiredService<HandleRegistery>();
            var type = registery.Handlers[packetType];
            return (IPacketHandler) provider.GetRequiredService(type);
        }

        private INetPacket ResolvePacket(PacketType packetType,NetPacketReader reader) {
            var registery = provider.GetRequiredService<PacketRegistery>();
            var type = registery.PacketTypes[packetType];
            var packet = (INetPacket) Activator.CreateInstance(type);
            packet.Deserialize(reader);
            return packet;
        }

        
    }
}
