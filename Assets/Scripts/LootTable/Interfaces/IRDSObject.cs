using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRDSObject
{
    float DropProbability { get; set; }
    bool UniqueDrop { get; set; }
    bool DropAlways { get; set; }
    bool DropEnabled { get; set; }
    RDSTable Table { get; set; }

    event EventHandler EvaluatePreResult;
    event EventHandler HitResult;
    event EventHandler EvaluatePostResult;

    void OnEvaluatePreResult(EventArgs e);
    void OnHitResult(EventArgs e);
    void OnEvaluatePostResult(EventArgs e);

    string ToString(int _indentLevel);
}
