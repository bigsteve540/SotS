using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : RDSObject
{
    public string Name = "";

    public TestItem(string _name)
    {
        Name = _name;
    }

    public override string ToString()
    {
        return Name;
    }
}

public class TestItemDemo3 : TestItem
{
    private bool dynamic = false;

    public override void OnEvaluatePreResult(EventArgs e)
    {
        if (dynamic)
            DropProbability *= 1.05f;
    }

    public override void OnHitResult(EventArgs e)
    {
        if (dynamic)
        {
            DropProbability = 1;
            Logger.Print("Dynamic hit! Resetting its probability.", LogLevel.info);
        }
    }

    public TestItemDemo3(string _name, bool _dynamic) : base(_name)
    {
        dynamic = _dynamic;
        DropProbability = (dynamic ? 1 : 100);
    }

    public override string ToString()
    {
        return base.ToString() + (dynamic ? " @ " + DropProbability.ToString("n4") : "");
    }
}
