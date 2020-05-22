using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEventArgs : EventArgs
{
    public IEnumerable<IRDSObject> Result { get; private set; }

    public delegate void ResultEventHandler(object _sender, ResultEventArgs e);

    public ResultEventArgs(IEnumerable<IRDSObject> _result)
    {
        Result = _result;
    }
}
