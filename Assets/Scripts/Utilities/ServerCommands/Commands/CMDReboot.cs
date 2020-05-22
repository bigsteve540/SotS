using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDReboot : ServerCommand
{
    public override string Name => "restart";
    public override string Usage { get; }

    public override void Execute(string[] args)
    {
        Logger.Print("Using this command will cause the server to reboot. Continue? y/n", LogLevel.warning);

        ConsoleKey key = Console.ReadKey().Key;
        while (key != ConsoleKey.Y || key != ConsoleKey.N)
        {
            if (key == ConsoleKey.N)
                return;

            if (key == ConsoleKey.Y)
            {
                Logger.Print($"Restarting server...", LogLevel.info);

                Server.Stop();
                Server.Start(NetworkManager.Instance.MaxPlayers, NetworkManager.Instance.Port);
                return;
            }
        }
    }
}
