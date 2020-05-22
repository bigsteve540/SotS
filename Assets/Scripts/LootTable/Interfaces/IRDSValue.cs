using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRDSValue<T> : IRDSObject
{
    T Value { get; }
}
