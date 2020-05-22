using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject, IItem, IRDSObject
{
    #region IItem Implementation
    [SerializeField] private string itemName = "New Item";
    public string ItemName { get { return itemName; } }
    [SerializeField] private int itemLevel = 0;
    public int ItemLevel { get { return itemLevel; } }
    [Space]
    [SerializeField] private ClassType classType = default;
    public ClassType ClassType { get { return classType; } }
    [SerializeField] private Rarity itemRarity = default;
    public Rarity ItemRarity { get { return itemRarity; } }
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
