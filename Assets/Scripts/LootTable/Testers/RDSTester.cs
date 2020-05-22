using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RDSTester : MonoBehaviour
{
    public int TestNumber = 1;

    void Start()
    {
        switch (TestNumber)
        {
            case 0:
                break;
            case 1:
                DemoOne();
                break;
            case 2:
                DemoTwo();
                break;
            case 3:
                DemoThree();
                break;
            case 4:
                DemoFour();
                break;
            case 5:
                DemoFive();
                break;
            default:
                Logger.Print("Please select a test number between 0 - 5.", LogLevel.warning);
                break;
        }
    }

    //pick 2 out of 6 items. 2nd run item 6 is set to always drop.
    void DemoOne()
    {
        Logger.Print("** DEMO 1 STARTED **", LogLevel.info);
        Logger.Print("____________________", LogLevel.info);

        RDSTable main = new RDSTable();

        main.AddEntry(new TestItem("Item 1"), 10, false, false, true);
        main.AddEntry(new TestItem("Item 2"), 10, false, false, true);
        main.AddEntry(new TestItem("Item 3"), 10, false, false, true);
        main.AddEntry(new TestItem("Item 4"), 10, false, false, true);
        main.AddEntry(new TestItem("Item 5"), 10, false, false, true);

        TestItem item6 = new TestItem("Item 6");
        main.AddEntry(item6, 10);

        main.DropCount = 2;

        Logger.Print("Step 1: Loot 2 of 6 items || 3 runs:", LogLevel.info);
        for (int i = 0; i < 3; i++)
        {
            Logger.Print($"Run {i + 1}:", LogLevel.info);
            foreach (TestItem item in main.GetResult())
            {
                Logger.Print(item, LogLevel.info);
            }
        }

        item6.DropAlways = true;
        Logger.Print("Step 2: Item 6 is now set to drop always || 3 runs:", LogLevel.info);
        for (int i = 0; i < 3; i++)
        {
            Logger.Print($"Run {i + 1}:", LogLevel.info);
            foreach (TestItem item in main.GetResult())
            {
                Logger.Print(item, LogLevel.info);
            }
        }

        Logger.Print("_____________________", LogLevel.info);
        Logger.Print("** DEMO 1 COMPLETE **", LogLevel.info);
    }

    //table recursion
    void DemoTwo()
    {
        RDSTable main = new RDSTable();

        RDSTable sub1 = new RDSTable();
        RDSTable sub2 = new RDSTable();
        RDSTable sub3 = new RDSTable();

        sub1.AddEntry(new TestItem("Table 1 - Item 1"), 10);
        sub1.AddEntry(new TestItem("Table 1 - Item 2"), 10);
        sub1.AddEntry(new TestItem("Table 1 - Item 3"), 10);

        sub2.AddEntry(new TestItem("Table 2 - Item 1"), 10);
        sub2.AddEntry(new TestItem("Table 2 - Item 2"), 10);
        sub2.AddEntry(new TestItem("Table 2 - Item 3"), 10);

        sub3.AddEntry(new TestItem("Table 3 - Item 1"), 10);
        sub3.AddEntry(new TestItem("Table 3 - Item 2"), 10);
        sub3.AddEntry(new TestItem("Table 3 - Item 3"), 10);

        main.AddEntry(sub1, 10);
        main.AddEntry(sub2, 10);
        main.AddEntry(sub3, 10);

        main.DropCount = 3;

        Logger.Print($"Step 1: Loot 3 items || 3 runs:", LogLevel.info);
        for (int i = 0; i < 3; i++)
        {
            Logger.Print($"Run {i + 1}:", LogLevel.info);
            foreach (TestItem item in main.GetResult())
            {
                Logger.Print(item, LogLevel.info);
            }
        }

        sub2.UniqueDrop = true;
        main.DropCount = 10;

        Logger.Print("Step 2: Table 2 is now unique, loot 10 items || 3 runs:", LogLevel.info);
        for (int i = 0; i < 3; i++)
        {
            Logger.Print($"Run {i + 1}:", LogLevel.info);
            foreach (TestItem item in main.GetResult())
            {
                Logger.Print(item, LogLevel.info);
            }
        }
    }

    //dynamic value modification
    void DemoThree()
    {
        RDSTable main = new RDSTable();

        main.AddEntry(new TestItem("Item 1"), 10);
        main.AddEntry(new TestItem("Item 2"), 10);
        main.AddEntry(new TestItem("Item 3"), 10);
        main.AddEntry(new TestItem("Item 4"), 10);
        main.AddEntry(new TestItemDemo3("Dynamic Item 5", true), 10);

        int whileTicker = 0;
        bool hitDynamic = false;
        while(whileTicker <= 50)
        {
            foreach (TestItem item in main.GetResult())
            {
                if(item is TestItemDemo3)
                {
                    hitDynamic = true;
                    Logger.Print($"{item.ToString()} || after {whileTicker + 1} attempts.", LogLevel.info);
                }
            }
            if (hitDynamic)
                break;

            whileTicker++;
        }
    }

    //spawning monsters
    void DemoFour()
    {
        int areaLevel = 30;

        GoblinTable main = new GoblinTable(areaLevel)
        {
            DropCount = 10
        };

        Logger.Print($"Step 1: Spawn goblins based on area level {areaLevel}", LogLevel.info);

        string msg = string.Empty;
        foreach (TestMobs.Goblin goblin in main.GetResult())
        {
            msg += goblin.ToString() + "\n";
        }

        Logger.Print("\n" + msg, LogLevel.info); //unity cannot print more than like 7 lines, click on the log to view them all
    }

    //gold drops and other things
    void DemoFive()
    {
        int areaLevel = 30;
        int mobLevel = 31;
        int playerLevel = 35;


        RDSTable main = new RDSTable();
        main.AddEntry(new TestMobs.Gold(areaLevel, mobLevel, playerLevel), 1);
        Logger.Print($"Gold drop equivalent to: {((TestMobs.Gold)main.GetResult().First()).Value.ToString("n2")}", LogLevel.info);
    }
}
