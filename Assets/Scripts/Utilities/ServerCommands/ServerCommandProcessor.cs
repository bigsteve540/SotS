using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ServerCommandProcessor
{
    private static Dictionary<string, ServerCommand> commands;

    public static void LoadCommands()
    {
        commands = new Dictionary<string, ServerCommand>();

        IEnumerable<Type> types = Assembly.GetAssembly(typeof(ServerCommand)).GetTypes()
               .Where(type => !type.IsAbstract && type.IsClass && type.IsSubclassOf(typeof(ServerCommand)));

        foreach (Type command in types)
        {
            ServerCommand instance = (Activator.CreateInstance(command) as ServerCommand);
            commands.Add(instance.Name.Trim(), instance);
        }
    }

    public static void WaitForCommand()
    {
        string input = Console.ReadLine();
        ParseInput(input);
        WaitForCommand();
    }

    private static void ParseInput(string msg)
    {
        string[] input = msg.Split();

        if (input.Length == 0
            || input == null
            || !commands.ContainsKey(input[0]))
        {
            Logger.Print("Command not Recognized.", LogLevel.warning);
            return;
        }

        if (commands[input[0]].RequiresArgs)
        {
            if (input.Length - 1 == 0)
            {
                Logger.Print($"Command {input[0]} requires arguments: {commands[input[0]].Usage}", LogLevel.warning);
                return;
            }
        }

        try
        {
            commands[input[0]].Execute(input.Skip(1).ToArray());
        }
        catch (Exception e)
        {
            Logger.Print(e.ToString(), LogLevel.error);
        }

    }
}
