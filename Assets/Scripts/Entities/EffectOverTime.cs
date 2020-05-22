using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectOverTime : IEffectOverTime
{
    [SerializeField] private int value = default;
    public int Value { get { return value; } }
    [SerializeField] private bool isConditionDamage = default;
    public bool IsConditionDamage { get { return isConditionDamage; } }
    [SerializeField] private EffectType effectType = default;
    public EffectType EffectType { get { return effectType; } }
    [SerializeField] private int order = default;
    public int Order { get { return order; } }
    [SerializeField] private object source = default;
    public object Source { get { return source; } }
    [SerializeField] private float tickRate = default;
    public float TickRate { get { return tickRate; } }
    [SerializeField] private float duration = default;
    public float Duration { get { return duration; } }
    public float CurrentTime { get; }

    public void OnApplyModifier(Resource _value)
    {
        throw new System.NotImplementedException();
    }

    public void OnRemoveModifier(Resource _value)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerModifier(Resource _value)
    {
        throw new System.NotImplementedException();
    }
}
