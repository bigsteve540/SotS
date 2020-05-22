using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome { Crypts, Caves, Faewild, Boneyards, Soulstreams, Hell }

public static class ItemGenerator
{
    public static int GeneratorSeed = 1;

    private static bool initialized = false;
    private static XorShiftRNG randomizer;

    public static Item GenerateItem(GeneratorSettings _settings)
    {
        StartRandomizer();
        return null;
    }

    public static int GenerateNumber()
    {
        StartRandomizer();

        return randomizer.NextInt32();
    }

    private static void StartRandomizer()
    {
        if (!initialized)
        {
            randomizer = new XorShiftRNG((ulong)(GeneratorSeed == 1 ? DateTime.Now.GetHashCode() : GeneratorSeed));
            initialized = true;
        }
    }

    public static void GetRandomizerXY(out ulong _x, out ulong _y)
    {
        _x = randomizer.XorX;
        _y = randomizer.XorY;
    }
}

public struct GeneratorSettings
{
    public Biome Biome;
    public int ItemLevel;    
}
