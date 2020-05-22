using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    flat = 100,
    percentMax = 200,
    percentMissing = 300,
    percentCurrent = 400
}
public enum ValueType
{
    flat = 100,
    additivePercent = 200,
    multiplicativePercent = 300
}

public interface IStatModifier
{
    int Value { get; }
    ValueType Type { get; }
    int Order { get; }
    object Source { get; }

    void OnApplyModifier(Statistic _value);
    void OnRemoveModifier(Statistic _value);
}

public interface IEffectOverTime
{
    int Value { get; }
    EffectType EffectType { get; }
    bool IsConditionDamage { get; }
    int Order { get; }
    object Source { get; }

    float TickRate { get; }

    float Duration { get; }
    float CurrentTime { get; }

    void OnApplyModifier(Resource _value);
    void OnTriggerModifier(Resource _value);
    void OnRemoveModifier(Resource _value);
}
