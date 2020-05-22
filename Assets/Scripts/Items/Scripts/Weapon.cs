using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject, IWeapon, IRDSObject
{
    public abstract string ItemName { get; }
    public abstract int ItemLevel { get; }
    public abstract ClassType ClassType { get; }
    public abstract Rarity ItemRarity { get; }
    public abstract WeaponType WeaponType { get; }

    public abstract float BasicAttacksPerSecond { get; }
    public abstract bool IsTwoHanded { get; }

    public abstract EquipmentSlotType SlotType { get; }
    public EffectOverTime HealthEffector { get; }
    public EffectOverTime BarrierEffector { get; }
    public EffectOverTime ResourceEffector { get; }
    public StatModifier AttackModifier { get; }
    public StatModifier SpiteModifier { get; }
    public StatModifier RigorModifier { get; }
    public StatModifier ToughnessModifier { get; }
    public StatModifier ConstitutionModifier { get; }
    public StatModifier HasteModifier { get; }

    public float DropProbability { get; set; }
    public bool UniqueDrop { get; set; }
    public bool DropAlways { get; set; }
    public bool DropEnabled { get; set; }
    public RDSTable Table { get; set; }


    public event EventHandler EvaluatePreResult;
    public event EventHandler HitResult;
    public event EventHandler EvaluatePostResult;

    public abstract void BasicAttack();

    public abstract void CastSpell1();
    public abstract void CastSpell2();
    public virtual void CastSpell3()
    {
        if (!IsTwoHanded)
            return;
    }
    public virtual void CastSpell4()
    {
        if (!IsTwoHanded)
            return;
    }

    public void Equip(Inventory _inv)
    {
        throw new System.NotImplementedException();
    }
    public void Unequip(Inventory _inv)
    {
        throw new System.NotImplementedException();
    }

    public void OnEvaluatePostResult(EventArgs e)
    {
        throw new NotImplementedException();
    }
    public void OnEvaluatePreResult(EventArgs e)
    {
        throw new NotImplementedException();
    }
    public void OnHitResult(EventArgs e)
    {
        throw new NotImplementedException();
    }

    public string ToString(int _indentLevel)
    {
        throw new NotImplementedException();
    }


}
