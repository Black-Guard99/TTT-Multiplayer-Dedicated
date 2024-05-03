using LiteNetLib;
using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Network_Shared.Registery;
using UnityEngine;

public class NetworkClient : MonoBehaviour,INetEventListener {
    public static NetworkClient instance { get; private set; }

    private NetManager netManager;
    private NetPeer server;
    private NetDataWriter writer;
    private PacketRegistery packetRegistery;
    private HandleRegistery registeryHandler;
    public Action OnPeerConnectdAction;


    private void Awake() {
        if(instance == null) {
            instance = this;
        }else {
            Destroy(instance);
        }
        
    }
    private void Start() {
        Init();
    }
    private void Update() {
        netManager.PollEvents();
    }
    private void Init() {
        packetRegistery = new PacketRegistery();
        registeryHandler = new HandleRegistery();
        writer = new NetDataWriter();
        netManager = new NetManager(this)
        {
            DisconnectTimeout = 10000000
        };
        netManager.Start();
    }

    public void Connect() {
        netManager.Connect("localhost", 9050, "");
    }
    public void SendServer<G> (G packet,DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where G : INetSerializable {
        if(server == null) {
            return;
        }
        writer.Reset();
        packet.Serialize(writer);
        server.Send(writer, deliveryMethod);
    }
    public void OnConnectionRequest(ConnectionRequest request) {
        
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
        //var data = Encoding.UTF8.GetString(reader.RawData).Replace("\0", "");
        //Debug.Log("Data Recived from Server " +  data);
        var packetType = (PacketType)reader.GetByte();
        var packet = ResolvePacket(packetType,reader);

        var handler = ResolveHandler(packetType);

        handler.Handle(packet,peer.Id);

        reader.Recycle();
    }

    private IPacketHandler ResolveHandler(PacketType packetType) {
        var handlerType = registeryHandler.Handlers[packetType];
        return (IPacketHandler) Activator.CreateInstance(handlerType);
    }

    private INetPacket ResolvePacket(PacketType packetType, NetPacketReader reader) {
        var type = packetRegistery.PacketTypes[packetType];
        var packet = (INetPacket)Activator.CreateInstance(type);

        packet.Deserialize(reader);
        return packet;
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        
    }

    public void OnPeerConnected(NetPeer peer) {
        Debug.Log("Connected to Server " +  peer);
        server = peer;
        OnPeerConnectdAction?.Invoke();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("Lost Connection to Sever");
    }

    
}
