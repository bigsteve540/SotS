using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    public static void Welcome(int _clientID, ulong _x, ulong _y)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcome))
        {
            packet.Write(_x);
            packet.Write(_y);
            packet.Write(_clientID);

            SendTCPData(_clientID, packet);
        }
    }
    public static void SpawnPlayer(int _clientID, Player _player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(_player.ID);
            packet.Write(_player.Username);
            packet.Write(_player.Level);

            packet.Write(new Color(_player.Colour[0], _player.Colour[1], _player.Colour[2], 1));
            packet.Write(_player.transform.position);

            SendTCPData(_clientID, packet);
        }
    }
    public static void DespawnPlayer(int _clientID, int _toDespawn)
    {
        using(Packet packet = new Packet((int)ServerPackets.despawnPlayer))
        {
            packet.Write(_toDespawn);

            SendTCPData(_clientID, packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(_player.ID);
            packet.Write(_player.transform.position);

            SendUDPDataToAll(packet);
        };
    }

    public static void PlayerDisconnected(int _clientID, Player _player)
    {
        using(Packet packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            packet.Write(_clientID);
            packet.Write(_player.Level);

            SendTCPDataToAll(packet);
        }
    }

    public static void TeleportPlayer(int _clientID, Vector2 _targetPosition)
    {
        using(Packet packet = new Packet((int)ServerPackets.teleportPlayer))
        {
            packet.Write(_clientID);
            packet.Write(_targetPosition);

            SendTCPDataToAll(packet);
        }
    }

    public static void ShiftDimension(int _clientID, int _z)
    {
        using (Packet packet = new Packet((int)ServerPackets.shiftDimension))
        {
            packet.Write(_clientID);
            packet.Write(_z);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemSpawned(int _dropID, Biome _biome, Vector3 _position)
    {
        using(Packet packet = new Packet((int)ServerPackets.itemSpawned))
        {
            packet.Write(_dropID);
            packet.Write((int)_biome);
            packet.Write(_position);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemPickedUp(int _dropID)
    {
        //used to remove specified object from the world client side
        using(Packet packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            packet.Write(_dropID);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemAddedToInventory(int _clientID, int _index, Item _item)
    {

    }

    public static void ChatMessage(int _exceptClient, string _msg)
    {
        using(Packet packet = new Packet((int)ServerPackets.chatMessage))
        {
            packet.Write(_exceptClient);
            packet.Write(_msg);
            SendTCPDataToAll(_exceptClient, packet);
        }
    }

    #region TCP Sending
    private static void SendTCPData(int _clientID, Packet _packet)
    {
        _packet.WriteLength();
        Server.Clients[_clientID].TCP.SendData(_packet);
    }
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].TCP.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
                Server.Clients[i].TCP.SendData(_packet);
        }
    }
    #endregion

    #region UDP Sending
    private static void SendUDPData(int _clientID, Packet _packet)
    {
        _packet.WriteLength();
        Server.Clients[_clientID].UDP.SendData(_packet);
    }
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].UDP.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
                Server.Clients[i].UDP.SendData(_packet);
        }
    }
    #endregion

}
