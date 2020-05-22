using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogLevel
{
    info,
    debug,
    warning,
    error
}

public static class Logger
{
    public static void Print(object _msg, LogLevel _severity)
    {
        string message = string.Empty;

#if UNITY_EDITOR
        switch (_severity)
        {
            case LogLevel.info:
                message += "[INFO] ";
                break;
            case LogLevel.debug:
                message += "<color=#3CE115>[DEBUG]</color> ";
                break;
            case LogLevel.warning:
                message += "<color=#E8E613>[WARN]</color> ";
                break;
            case LogLevel.error:
                message += "<color=#E52222>[ERROR]</color> ";
                break;
        }

        message += _msg;
        Debug.Log(message);
#else
        message = message.Insert(0, GetTimeStamp(DateTime.Now));
        message += _msg;
        Console.WriteLine(message);
#endif

    }

    private static string GetTimeStamp(DateTime _time)
    {
        return _time.ToString("[HH:mm:ss] ");
    }
}
