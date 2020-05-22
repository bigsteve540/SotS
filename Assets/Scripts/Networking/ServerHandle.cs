using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _clientID, Packet _packet)
    {
        int clientIDCheck = _packet.ReadInt();

        string userGUID = _packet.ReadString();
        string username = _packet.ReadString();
        int userClassType = _packet.ReadInt();

        Color tmp = _packet.ReadColour();
        float[] userColourAsArray = new float[3] { tmp.r, tmp.g, tmp.b };

        if (_clientID != clientIDCheck)
        {
            Logger.Print($"Player | {username} | has assumed the wrong client ID: {clientIDCheck}", LogLevel.error);
            return;
        }

        Logger.Print($"{Server.Clients[_clientID].TCP.Socket.Client.RemoteEndPoint} successfully connected, assigned ID: {_clientID}", LogLevel.info);

        if (!Server.CharacterDictionary.ContainsKey(userGUID))
        {
            Server.CharacterDictionary.Add(userGUID, new PlayerSave(username, 1, userClassType, userColourAsArray));
            FileSystem.WriteCharacterFile(userGUID, username, 1, userColourAsArray, userClassType);
        }
        Server.Clients[_clientID].SendIntoGame(userGUID, Server.CharacterDictionary[userGUID]);
    }

    public static void PlayerMovement(int _clientID, Packet _packet)
    {
        bool[] inputs = new bool[_packet.ReadInt()];

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = _packet.ReadBool();
        }

        Server.Clients[_clientID].Player.SetInput(inputs);
    }

    public static void PlayerInteract(int _clientID, Packet _packet)
    {
        Server.Clients[_clientID].Player.TryInteract();
    }

    public static void ChatMessage(int _clientID, Packet _packet)
    {
        string msg = _packet.ReadString();
        ServerSend.ChatMessage(_clientID, msg);
    }

    public static void InteractWithInventory(int _clientID, Packet _packet)
    {

    }
}
