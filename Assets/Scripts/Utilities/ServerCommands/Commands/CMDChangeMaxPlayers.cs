using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDChangeMaxPlayers : ServerCommand
{
    public override string Name { get; } = "maxplayers";
    public override string Usage { get; } = "maxplayers <num>";

    public override void Execute(string[] args)
    {
        Logger.Print("Modifying the maximum player cap will require a server reboot. Continue? y/n", LogLevel.warning);

        ConsoleKey key = Console.ReadKey().Key;
        while (key != ConsoleKey.Y || key != ConsoleKey.N)
        {
            if (key == ConsoleKey.N)
                return;

            if (key == ConsoleKey.Y)
            {
                Logger.Print($"Restarting server with {args[0]} max players...", LogLevel.info);

                Server.Stop();
                Server.Start(Convert.ToInt32(args[0]), NetworkManager.Instance.Port);
                return;
            }


        }
    }
}
