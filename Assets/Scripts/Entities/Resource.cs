using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Resource
{
    public readonly Statistic BaseValue;
    public int Current { get; private set; }

    public readonly ReadOnlyDictionary<int, IEffectOverTime> Dots;

    private readonly Dictionary<int, IEffectOverTime> dots;
    private Func<ClassType, int, int> levelUpHandler;

    public Resource(int _baseValue) : this(ClassType.all, 1, null) { }
    public Resource(ClassType _class, int _level, Func<ClassType, int, int> _handler)
    {
        levelUpHandler = _handler;

        if (levelUpHandler != null)
            BaseValue = new Statistic(levelUpHandler(_class, _level));
        else
            BaseValue = new Statistic();

        dots = new Dictionary<int, IEffectOverTime>();
        Dots = new ReadOnlyDictionary<int, IEffectOverTime>(dots);
    }

    #region Add/Remove DoTs
    public int AddDot(IEffectOverTime _dot)
    {
        if (dots.Count == 0)
        {
            dots.Add(0, _dot);
            return 0;
        }

        for (int i = 0; i < dots.Keys.Count; i++)
        {
            if (!dots.ContainsKey(i))
            {
                dots.Add(i, _dot);
                return i;
            }
        }

        dots.Add(dots.Count + 1, _dot);
        return dots.Count + 1;
    }
    public bool RemoveDot(int _id)
    {
        if (dots.Remove(_id))
        {
            return true;
        }
        return false;
    }
    public bool RemoveDot(IEffectOverTime _dot)
    {
        for (int i = 0; i < dots.Values.Count; i++)
        {
            if (dots.ContainsKey(i) && dots[i] == _dot)
            {
                dots.Remove(i);
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Add/Remove Permanents
    public void AddPermanent(IStatModifier _mod)
    {
        BaseValue.AddPermanentModifier(_mod);
    }
    public bool RemovePermanent(IStatModifier _mod)
    {
        return BaseValue.RemovePermanentModifier(_mod);
    }
    public void RemovePermanent(int _index)
    {
        BaseValue.RemovePermanentModifier(_index);
    }
    #endregion

    public void TickModifiers()
    {
        List<IEffectOverTime> toTickThisFrame = new List<IEffectOverTime>();
        foreach (IEffectOverTime dot in dots.Values)
        {
            if (Mathf.Approximately(dot.CurrentTime % dot.TickRate, 0) && !(dot.CurrentTime >= dot.Duration))
            {
                dot.OnTriggerModifier(this);
                toTickThisFrame.Add(dot);
            }
            else if (dot.CurrentTime >= dot.Duration)
            {
                RemoveDot(dot);
            }
        }

        if (toTickThisFrame.Count > 0)
        {
            toTickThisFrame.Sort(CompareDotOrder);
            CalculateValue(toTickThisFrame);
        }
    }

    //method is reliant on user input. minus sign infront of the number implies damage. 
    protected virtual void CalculateValue(IEnumerable<IEffectOverTime> _dots)
    {
        int final = 0;
        foreach (IEffectOverTime dot in _dots)
        {
            switch (dot.EffectType)
            {
                case EffectType.flat:
                    final += dot.Value;
                    break;
                case EffectType.percentMax:
                    final += BaseValue.Current * dot.Value;
                    break;
                case EffectType.percentMissing:
                    int missing = BaseValue.Current - Current;
                    final += missing * dot.Value;
                    break;
                case EffectType.percentCurrent:
                    final += Current * dot.Value;
                    break;
            }
        }
        Current += final;
    }

    protected virtual int CompareDotOrder(IEffectOverTime a, IEffectOverTime b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }
}