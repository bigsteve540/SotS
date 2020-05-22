using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Resource
{
    private Statistic toughness;
    private Statistic constitution;

    public Health(ClassType _class, int _level, Func<ClassType,int,int> _handler, Statistic _toughness, Statistic _constitution): base(_class, _level, _handler)
    {
        toughness = _toughness;
        constitution = _constitution;
    }

    protected override void CalculateValue(IEnumerable<IEffectOverTime> _dots)
    {
        int final = 0;
        int current;

        foreach (IEffectOverTime dot in _dots)
        {
            current = 0;
            switch (dot.EffectType)
            {
                case EffectType.flat:
                    current += dot.Value;
                    break;
                case EffectType.percentMax:
                    current += BaseValue.Current * dot.Value;
                    break;
                case EffectType.percentMissing:
                    int missing = BaseValue.Current - Current;
                    current += missing * dot.Value;
                    break;
                case EffectType.percentCurrent:
                    current += Current * dot.Value;
                    break;
            }

            if(Mathf.Sign(dot.Value) == -1)
            {
                if (dot.IsConditionDamage)
                    current *= 1 - (constitution.Current / 100);    
                else
                    current *= 1 - (toughness.Current / 100);
            }

            final += current;
        }
    }
}
