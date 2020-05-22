using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDSNullValue : RDSValue<object>
{
    public RDSNullValue(float _probability) : base(null, _probability, false, false, true, null) { }
}
