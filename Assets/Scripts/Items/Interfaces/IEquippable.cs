using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType { head, body, leftWeapon, rightWeapon, leftAccessory, rightAccessory }

public interface IEquippable : IItem
{
    EquipmentSlotType SlotType { get; }

    EffectOverTime HealthEffector { get; }
    EffectOverTime BarrierEffector { get; }
    EffectOverTime ResourceEffector { get; }

    StatModifier AttackModifier { get; }
    StatModifier SpiteModifier { get; }
    StatModifier RigorModifier { get; }
    StatModifier ToughnessModifier { get; }
    StatModifier ConstitutionModifier { get; }
    StatModifier HasteModifier { get; }

    void Equip(Inventory _inv);
    void Unequip(Inventory _inv);
}
