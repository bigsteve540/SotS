using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestMobs;

public class GoblinTable : RDSTable
{
    public GoblinTable(int arealevel)
    {
        // Shamans with different level based on the arealevel
        // With a probability curve peaked at the area level
        AddEntry(new Shaman(arealevel - 2), 100);
        AddEntry(new Shaman(arealevel - 1), 200);
        AddEntry(new Shaman(arealevel), 500);
        AddEntry(new Shaman(arealevel + 1), 200);
        AddEntry(new Shaman(arealevel + 2), 100);
        // Same for Warriors
        AddEntry(new Warrior(arealevel - 2), 100);
        AddEntry(new Warrior(arealevel - 1), 200);
        AddEntry(new Warrior(arealevel), 500);
        AddEntry(new Warrior(arealevel + 1), 200);
        AddEntry(new Warrior(arealevel + 2), 100);
        // BOB is double the arealevel - a real hard one!
        AddEntry(new BOB(arealevel * 2), 1);
        DropProbability = 10;
    }
}
