using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestMobs
{
    public class Goblin : RDSCreatableObject
    {
        public int Level = 0;

        public Goblin(int _level) { Level = _level; }

        public override string ToString()
        {
            return $"{GetType().Name} - Level: {Level.ToString()}";
        }
    }

    public class Shaman : Goblin
    {
        public Shaman(int _level) : base(_level) { }
        public override IRDSObject CreateInstance()
        {
            return new Shaman(Level);
        }
    }

    public class Warrior : Goblin
    {
        public Warrior(int _level) : base(_level) { }
        public override IRDSObject CreateInstance()
        {
            return new Warrior(Level);
        }
    }

    public class BOB : Goblin
    {
        public BOB(int _level) : base(_level) { UniqueDrop = true; }
        public override IRDSObject CreateInstance()
        {
            return new BOB(Level);
        }
    }

    public class Gold : RDSValue<int>
    {
        public Gold(int _areaLevel, int _mobLevel, int _playerLevel) : base(0, 1)
        {
            Value = 10 * _areaLevel + (_mobLevel - _areaLevel) + (_areaLevel - _playerLevel);
        }
    }
}


