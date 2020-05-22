using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassConstants
{
    //max level : 120

    public static int WARRIOR_BASE_HP = 423;
    public static int MONK_BASE_HP = 349;
    public static int RANGER_BASE_HP = 255;
    public static int ROGUE_BASE_HP = 221;
    public static int SORCERER_BASE_HP = 190;

    public static Func<ClassType, int, int> HPPerLevel = (ClassType _class, int _level) =>
    {
        switch (_class)
        {
            case ClassType.all:
                return 0;
            case ClassType.warrior: //base hp cap at ~23.5k
                return 192 * _level + WARRIOR_BASE_HP;
            case ClassType.monk: //base hp cap at ~18.3k
                return 150 * _level + MONK_BASE_HP;
            case ClassType.ranger: //base hp cap at ~15.2k
                return 125 * _level + RANGER_BASE_HP;
            case ClassType.rogue: //base hp cap at ~13.2k
                return 108 * _level + ROGUE_BASE_HP;
            case ClassType.sorcerer: //base hp cap at ~11.2k
                return 92 * _level + SORCERER_BASE_HP;
            default:
                return 0;
        }
    };

    public static Func<ClassType, int, int> ResourcePerLevel = (ClassType _class, int _level) =>
    {
        switch (_class)
        {
            case ClassType.all:
                return 0;
            case ClassType.monk:
                return 0;
            case ClassType.ranger:
                return 0;
            case ClassType.rogue:
                return 0;
            case ClassType.sorcerer:
                return 0;
            case ClassType.warrior:
                return 0;
            default:
                return 0;
        }
    };


    #region Warrior

    #endregion
    #region Ranger

    #endregion
    #region Sorcerer

    #endregion
    #region Rogue

    #endregion
    #region Monk

    #endregion
}
