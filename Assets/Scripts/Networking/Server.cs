using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public struct PlayerSave
{
    public string Name;
    public int Level;
    public int CharacterClass;
    public Color Color;

    public PlayerSave( string _name, int _level, int _class, float[] _color)
    {
        Name = _name;
        Level = _level;
        CharacterClass = _class;
        Color = new Color(_color[0], _color[1], _color[2], 1);
    }
}

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> Clients;
    public static Dictionary<string, PlayerSave> CharacterDictionary;

    public delegate void PacketHandler(int _clientID, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _maxPlayers, int _port)
    {
        MaxPlayers = _maxPlayers;
        Port = _port;

       Logger.Print("Starting server...", LogLevel.info);

        InitializeData();

        Logger.Print("Loading player data...", LogLevel.info);

        CharacterDictionary = FileSystem.ReadCharacterFiles();

        Logger.Print("Loaded player data successfully.", LogLevel.info);
        Logger.Print("Loading commands...", LogLevel.info);

        ServerCommandProcessor.LoadCommands();

        Logger.Print("Loaded commands successfully.", LogLevel.info);
        Logger.Print("Loading world data...", LogLevel.info);

        WorldSettings.LoadWorld();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Logger.Print($"Server started on port: {Port}", LogLevel.info);
#if UNITY_SERVER
        //ServerCommandProcessor.WaitForCommand();
#endif
    }
    public static void Stop()
    {
        if(tcpListener != null)
            tcpListener.Stop();
        if(udpListener != null)
            udpListener.Close();

        FileSystem.WriteWorldFile("world");
    }

    public static void SendUDPData(IPEndPoint _endPoint, Packet _packet)
    {
        try
        {
            if (_endPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _endPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Logger.Print($"Error sending UDP to {_endPoint}: {_ex}", LogLevel.error);
        }
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        Logger.Print($"Attempted connection from: {_client.Client.RemoteEndPoint}...", LogLevel.info);

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (Clients[i].TCP.Socket == null)
            {
                Clients[i].TCP.Connect(_client);
                Logger.Print("Allowing connection...", LogLevel.debug);
                return;
            }
        }

        Logger.Print("Failed to connect: Server full.", LogLevel.warning);
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet packet = new Packet(data))
            {
                int clientID = packet.ReadInt();

                if (clientID == 0)
                    return;

                if (Clients[clientID].UDP.EndPoint == null)
                {
                    Clients[clientID].UDP.Connect(clientEndPoint);
                    return; //don't handle data, this is initial empty connection packet
                }

                if (Clients[clientID].UDP.EndPoint.ToString() == clientEndPoint.ToString())
                    Clients[clientID].UDP.HandleData(packet);
            }
        }
        catch (Exception _ex)
        {
            if (_ex is ObjectDisposedException)
                return;

            Logger.Print($"Error receiving UDP: {_ex}", LogLevel.error);
        }

    }

    private static void InitializeData()
    {
        Clients = new Dictionary<int, Client>();

        for (int i = 1; i <= MaxPlayers; i++)
        {
            Clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
            { (int)ClientPackets.playerMovement,  ServerHandle.PlayerMovement  },
            { (int)ClientPackets.playerInteract,  ServerHandle.PlayerInteract  },
            { (int)ClientPackets.chatMessage,     ServerHandle.ChatMessage     }
        };

        Logger.Print("Initialized Packets", LogLevel.info);
    }
}
