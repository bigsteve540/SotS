using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDSValue<T> : IRDSValue<T>
{
    private T valueBacker;
    public T Value { get { return valueBacker; } set { valueBacker = value; } }
    public float DropProbability { get; set; }
    public bool UniqueDrop { get; set; }
    public bool DropAlways { get; set; }
    public bool DropEnabled { get; set; }
    public RDSTable Table { get; set; }

    public event EventHandler EvaluatePreResult;
    public event EventHandler HitResult;
    public event EventHandler EvaluatePostResult;

    public RDSValue(T _value, float _probability) : this(_value, _probability, false, false, true, null) { }
    public RDSValue(T _value, float _probability, bool _unique, bool _always, bool _enabled, RDSTable _table)
    {
        Value = _value;
        DropProbability = _probability;
        UniqueDrop = _unique;
        DropAlways = _always;
        DropEnabled = _enabled;
        Table = _table;
    }

    public virtual void OnEvaluatePostResult(EventArgs e)
    {
        EvaluatePostResult?.Invoke(this, e);
    }

    public virtual void OnEvaluatePreResult(EventArgs e)
    {
        EvaluatePreResult?.Invoke(this, e);
    }

    public virtual void OnHitResult(EventArgs e)
    {
        HitResult?.Invoke(this, e);
    }

    public string ToString(int _indentLevel)
    {
        string indent = "".PadRight(4 * _indentLevel, ' ');

        return string.Format(indent + "(RDSObject){0} Prob:{1},UAE:{2}{3}{4}",
                 GetType().Name,
                 DropProbability,
                (UniqueDrop ? "1" : "0"),
                (DropAlways ? "1" : "0"),
                (DropEnabled ? "1" : "0")
        );
    }
}
