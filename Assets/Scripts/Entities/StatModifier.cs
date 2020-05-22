using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier : IStatModifier
{
    [SerializeField] private int value = default;
    public int Value { get { return value; } }
    [SerializeField] private ValueType type = default;
    public ValueType Type { get { return type; } }
    [SerializeField] private int order = default;
    public int Order { get { return order; } }
    [SerializeField] private object source = default;
    public object Source { get { return source; } }

    public void OnApplyModifier(Statistic _value)
    {
        throw new System.NotImplementedException();
    }

    public void OnRemoveModifier(Statistic _value)
    {
        throw new System.NotImplementedException();
    }
}
