using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServerCommand
{
    public abstract string Name { get; }
    public abstract string Usage { get; }
    public bool RequiresArgs { get { if (Usage == string.Empty) return false; return true; } }

    public abstract void Execute(string[] args);
}
