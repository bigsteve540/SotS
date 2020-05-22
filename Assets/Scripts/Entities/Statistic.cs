using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Statistic
{
    public int BaseValue;
    public virtual int Current
    {
        get
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                current = CalculateValue();
                isDirty = false;
            }
            return current;
        }

        protected set { }
    }

    public readonly ReadOnlyCollection<IStatModifier> StatModifiers;

    protected bool isDirty = true;
    protected int current;
    protected int lastBaseValue = int.MinValue;

    protected readonly List<IStatModifier> statModifiers;

    public Statistic() : this(0)
    {
        statModifiers = new List<IStatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    public Statistic(int _baseValue)
    {
        BaseValue = _baseValue;
        statModifiers = new List<IStatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public virtual void AddPermanentModifier(IStatModifier _mod)
    {
        isDirty = true;
        _mod.OnApplyModifier(this);
        statModifiers.Add(_mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public virtual bool RemovePermanentModifier(IStatModifier _mod)
    {
        if (statModifiers.Remove(_mod))
        {
            isDirty = true;
            _mod.OnRemoveModifier(this);
            return true;
        }
        return false;
    }
    public virtual void RemovePermanentModifier(int _index)
    {
        isDirty = true;
        statModifiers[_index]?.OnRemoveModifier(this);
        statModifiers.RemoveAt(_index);
    }
    public virtual bool PurgeModifiersFromSource(object _source)
    {
        bool success = false;

        for (int i = statModifiers.Count - 1; i >= 0; i++)
        {
            if(statModifiers[i].Source == _source)
            {
                isDirty = true;
                success = true;
                statModifiers[i].OnRemoveModifier(this);
                statModifiers.RemoveAt(i);
            }
        }
        return success;
    }

    protected virtual int CalculateValue()
    {
        int final = BaseValue;
        int sumAdditive = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            IStatModifier mod = statModifiers[i];

            switch (mod.Type)
            {
                case ValueType.flat:
                    final += mod.Value;
                    break;
                case ValueType.additivePercent:
                    sumAdditive += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != ValueType.additivePercent)
                    {
                        final *= 1 + sumAdditive;
                        sumAdditive = 0;
                    }
                    break;
                case ValueType.multiplicativePercent:
                    final *= 1 + mod.Value;
                    break;
            }      
        }
        return final;
    }

    protected virtual int CompareModifierOrder(IStatModifier a, IStatModifier b)
    {
        if(a.Order < b.Order)
            return -1;
        else if(a.Order > b.Order)
            return 1;
        return 0;
    }
}
