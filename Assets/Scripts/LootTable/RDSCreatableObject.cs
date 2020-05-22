using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDSCreatableObject : RDSObject, IRDSObjectCreator
{
    public virtual IRDSObject CreateInstance()
    {
        return (IRDSObject)Activator.CreateInstance(GetType()); //don't rly know what this is for, will need to be over ridden in the derived class
    }
}
