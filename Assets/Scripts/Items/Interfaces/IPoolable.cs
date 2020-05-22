using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    event Action OnDestroyPoolObject;
}
