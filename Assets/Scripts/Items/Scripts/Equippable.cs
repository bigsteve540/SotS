using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equippable")]
public class Equippable : ScriptableObject, IEquippable, IRDSObject
{
    #region IItem Implementation
    [SerializeField] private string itemName = "New Equipment";
    public string ItemName { get { return itemName; } }
    [SerializeField] private int itemLevel = 0;
    public int ItemLevel { get { return itemLevel; } }
    [Space]
    [SerializeField] private ClassType classType = default;
    public ClassType ClassType { get { return classType; } }
    [SerializeField] private Rarity itemRarity = default;
    public Rarity ItemRarity { get { return itemRarity; } }
    #endregion

    #region IEquippable Implementation
    [Space]
    [SerializeField] private EquipmentSlotType equipmentType = default;
    public EquipmentSlotType SlotType { get { return equipmentType; } }
    [Space]
    [SerializeField] private EffectOverTime healthEffector = default;
    public EffectOverTime HealthEffector { get { return healthEffector; } }
    [SerializeField] private EffectOverTime barrierEffector = default;
    public EffectOverTime BarrierEffector { get { return barrierEffector; } }
    [SerializeField] private EffectOverTime resourceEffector = default;
    public EffectOverTime ResourceEffector { get { return resourceEffector; } }
    [Space]
    [SerializeField] private StatModifier attackModifier = default;
    public StatModifier AttackModifier { get { return attackModifier; } }
    [SerializeField] private StatModifier spiteModifier = default;
    public StatModifier SpiteModifier { get { return spiteModifier; } }
    [SerializeField] private StatModifier rigorModifier = default;
    public StatModifier RigorModifier { get { return rigorModifier; } }
    [SerializeField] private StatModifier toughnessModifier = default;
    public StatModifier ToughnessModifier { get { return toughnessModifier; } }
    [SerializeField] private StatModifier constitutionModifier = default;
    public StatModifier ConstitutionModifier { get { return constitutionModifier; } }
    [SerializeField] private StatModifier hasteModifier = default;
    public StatModifier HasteModifier { get { return hasteModifier; } }

    public void Equip(Inventory _inv)
    {
        throw new System.NotImplementedException();
    }

    public void Unequip(Inventory _inv)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IRDSObject Implementation
    [Space]
    [Range(0, 1)] [SerializeField] private float dropProbability;
    public float DropProbability
    {
        get { return dropProbability; }
        set { dropProbability = value; }
    }
    [SerializeField] private bool uniqueDrop;
    public bool UniqueDrop
    {
        get { return uniqueDrop; }
        set { uniqueDrop = value; }
    }
    [SerializeField] private bool dropAlways;
    public bool DropAlways
    {
        get { return dropAlways; }
        set { dropAlways = value; }
    }
    [SerializeField] private bool allowDrop = true;
    public bool DropEnabled
    {
        get { return allowDrop; }
        set { allowDrop = value; }
    }
    [SerializeField] private RDSTable dropTable;
    public RDSTable Table
    {
        get { return dropTable; }
        set { dropTable = value; }
    }

    public event EventHandler EvaluatePreResult;
    public event EventHandler HitResult;
    public event EventHandler EvaluatePostResult;

    public virtual void OnEvaluatePostResult(EventArgs e) { }

    public virtual void OnEvaluatePreResult(EventArgs e) { }

    public virtual void OnHitResult(EventArgs e) { }

    public virtual string ToString(int _indentLevel) { return ItemName; }
    #endregion
}
