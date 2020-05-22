using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRDSTable : IRDSObject
{
    int DropCount { get; set; }
    IEnumerable<IRDSObject> Contents { get; }
    IEnumerable<IRDSObject> GetResult();
}
