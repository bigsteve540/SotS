using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    mundane = 0,
    uncommon = 10,
    rare = 20,
    epic = 30,
    exceptional = 40,
    mythic = 50,
    primordial = 60,
    celestial = 70
}

public enum ClassType { all = -1, warrior, ranger, sorcerer, rogue, monk }

public interface IItem
{
    string ItemName { get; }
    int ItemLevel { get; }

    ClassType ClassType { get; }
    Rarity ItemRarity { get; }
}
