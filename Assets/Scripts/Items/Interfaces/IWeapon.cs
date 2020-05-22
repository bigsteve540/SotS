using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    shield,
    shortbow,
    scepter,
    dagger,
    shortsword,
    longbow,
    bat,
    tonfa,
    hooks,
    katana,
    quarterStaff,
    staff,
    spear,
    bastardSword,
    halberd
}

public interface IWeapon : IEquippable
{
    float BasicAttacksPerSecond { get; }

    WeaponType WeaponType { get; }

    bool IsTwoHanded { get; }
    void BasicAttack();

    void CastSpell1();
    void CastSpell2();
    void CastSpell3();
    void CastSpell4();
}
