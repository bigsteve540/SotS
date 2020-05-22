using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  

public static class RDSRandom
{
    public static float GetRandomFloat(float _max)
    {
        return Random.Range(0f, _max);
    }
    public static float GetRandomFloat(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }

    public static int GetRandomInt(float _max)
    {
        return (int)Random.Range(0f, _max);
    }
    public static int GetRandomInt(float _min, float _max)
    {
        return (int)Random.Range(_min, _max);
    }

    public static IEnumerable<int> RollDice(int _num, int _sides)
    {
        List<int> rolls = new List<int>();
        for (int i = 0; i < _num; i++)
        {
            rolls.Add(Random.Range(1, _sides));
        }
        rolls.Insert(0, rolls.Sum());
        return rolls;
    }

    public static bool Percent(float _percentToHit)
    {
        return Random.Range(0f, 1f) <= _percentToHit ? true : false;
    }
}
