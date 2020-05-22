using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    blacksmith,
    church,
    auctionHouse,
    apothecary,
    farm,
    bank,
    house,
    wizardsTower,
    archeryRange,
    thievesGuild,
    outpost,
    artisansGuild
}

public static class WorldSettings
{
    public static int DayDuration = 27000;
    public static int NightDuration = 9000;

    public static int CurrentWorldTime = 18000;

    public static Dictionary<BuildingType, int> BuildingStates = new Dictionary<BuildingType, int>
    {
        { BuildingType.blacksmith,    0 },
        { BuildingType.church,        0 },
        { BuildingType.auctionHouse,  0 },
        { BuildingType.apothecary,    0 },
        { BuildingType.farm,          0 },
        { BuildingType.bank,          0 },
        { BuildingType.house,         0 },
        { BuildingType.wizardsTower,  0 },
        { BuildingType.archeryRange,  0 },
        { BuildingType.thievesGuild,  0 },
        { BuildingType.outpost,       0 },
        { BuildingType.artisansGuild, 0 },
    };

    public static void LoadWorld()
    {
        ItemGenerator.GeneratorSeed = Guid.NewGuid().GetHashCode();


        string[] data = FileSystem.ReadWorldFile();

        if(data != null)
        {
            DayDuration = Convert.ToInt32(data[0]);
            NightDuration = Convert.ToInt32(data[1]);
            CurrentWorldTime = Convert.ToInt32(data[2]);

            BuildingStates = new Dictionary<BuildingType, int>();

            for (int i = 0; i < Enum.GetValues(typeof(BuildingType)).Length; i++)
            {
                BuildingStates.Add((BuildingType)i, Convert.ToInt32(data[i + 3]));
            }
        }
    }
}
