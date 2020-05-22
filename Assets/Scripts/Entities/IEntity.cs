using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity : IRDSObjectCreator
{
    int Level { get; }

    ClassType Class { get; }

    bool EffectImmune { get; }
    bool DamageImmune { get; }

    Health Health { get; }
    Resource Barrier { get; }
    Resource Resource { get; } //mana, adrenaline, rage etc

    Statistic AttackPower { get; } //attack damage
    Statistic Spite { get; } //condition damage

    Statistic Rigor { get; } //crit chance 1-100

    Statistic Toughness { get; } //attack resistance
    Statistic Constitution { get; } //condition resistance

    Statistic Haste { get; } //movement speed
}
