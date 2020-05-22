using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public static int BufferSize = 4096;

    public int ID;
    public Player Player = null;
    public Dimension CurrentDimension = Dimension.overworld;
    private string GUID;

    public Tcp TCP;
    public Udp UDP;

    public Client(int _clientID)
    {
        ID = _clientID;

        TCP = new Tcp(_clientID);
        UDP = new Udp(_clientID);
    }

    public class Tcp
    {
        public TcpClient Socket;

        private NetworkStream stream;
        private Packet receivePacket;
        private byte[] receiveBuffer;

        private readonly int id;

        public Tcp(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            Socket = _socket;
            Socket.ReceiveBufferSize = BufferSize;
            Socket.SendBufferSize = BufferSize;

            stream = Socket.GetStream();

            receiveBuffer = new byte[BufferSize];
            receivePacket = new Packet();

            stream.BeginRead(receiveBuffer, 0, BufferSize, ReceiveCallback, null);

            ItemGenerator.GetRandomizerXY(out ulong genX, out ulong genY);
            ServerSend.Welcome(id, genX, genY);
            Logger.Print("Sent welcome. Awaiting response...", LogLevel.debug);
        }
        public void Disconnect()
        {
            Socket.Close();
            stream = null;
            receivePacket = null;
            receiveBuffer = null;
            Socket = null;
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (Socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Logger.Print($"Failed to send data to player {id}: {_ex}", LogLevel.error);
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLength = stream.EndRead(_result);
                if (byteLength <= 0)
                {
                    Server.Clients[id].Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];

                Array.Copy(receiveBuffer, data, byteLength);

                receivePacket.Reset(HandleData(data));

                stream.BeginRead(receiveBuffer, 0, BufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Logger.Print(_ex, LogLevel.error);
                Server.Clients[id].Disconnect();
            }
        }


        private bool HandleData(byte[] _data)
        {
            int packetLength = 0;

            receivePacket.SetBytes(_data);

            if (receivePacket.UnreadLength() >= 4)
            {
                packetLength = receivePacket.ReadInt();

                if (packetLength <= 0)
                    return true;
            }

            while (packetLength > 0 && packetLength <= receivePacket.UnreadLength())
            {
                byte[] packetBytes = receivePacket.ReadBytes(packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetID = packet.ReadInt();
                        Server.packetHandlers[packetID](id, packet);
                    }
                });

                packetLength = 0;
                if (receivePacket.UnreadLength() >= 4)
                {
                    packetLength = receivePacket.ReadInt();

                    if (packetLength <= 0)
                        return true;
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }
    public class Udp
    {
        public IPEndPoint EndPoint;

        private int id;

        public Udp(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            EndPoint = _endPoint;
        }
        public void Disconnect()
        {
            EndPoint = null;
        }

        public void SendData(Packet _packet)
        {
            Server.SendUDPData(EndPoint, _packet);
        }

        public void HandleData(Packet _packet)
        {
            int packetLength = _packet.ReadInt();
            byte[] data = _packet.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int packetID = packet.ReadInt();
                    Server.packetHandlers[packetID](id, packet);
                }
            });
        }
    }

    public void SendIntoGame(string _guid, PlayerSave _player)
    {
        GUID = _guid;

        if(Player == null)
        {
            Player = NetworkManager.Instance.InstantiatePlayer();
            Player.Initialize(ID, _player);
        }

        foreach (Client _client in Server.Clients.Values)
        {
            if (_client.Player != null)
            {
                if (_client.ID != ID && CurrentDimension == _client.CurrentDimension)
                {
                    ServerSend.SpawnPlayer(ID, _client.Player);
                }
            }
        }

        foreach (Client _client in Server.Clients.Values)
        {
            if (_client.Player != null && CurrentDimension == _client.CurrentDimension)
            {
                ServerSend.SpawnPlayer(_client.ID, Player);
            }
        }
    }

    private void Disconnect()
    {
        Logger.Print($"{TCP.Socket.Client.RemoteEndPoint} has disconnected.", LogLevel.info);

        ThreadManager.ExecuteOnMainThread(() =>
        {
            FileSystem.WriteCharacterFile(GUID, Player.Username, Player.Level, Player.Colour, (int)Player.Class);
            UnityEngine.Object.Destroy(Player.gameObject);
            Player = null;
        });

        TCP.Disconnect();
        UDP.Disconnect();

        ServerSend.PlayerDisconnected(ID, Player);
    }
}
