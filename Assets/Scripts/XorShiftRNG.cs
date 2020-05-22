using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XorShiftRNG
{
    private const double DOUBLE_UNIT = 1.0 / (int.MaxValue + 1.0);

    public ulong XorX;
    public ulong XorY;

    private ulong buffer;
    private ulong bufferMask;

    public XorShiftRNG()
    {
        XorX = (ulong)Guid.NewGuid().GetHashCode();
        XorY = (ulong)Guid.NewGuid().GetHashCode();
    }
    public XorShiftRNG(ulong _seed)
    {
        XorX = _seed << 3;
        XorY = _seed >> 3; //this may be wrong, may be XorX
    }
    public XorShiftRNG(ulong _x, ulong _y)
    {
        XorX = _x;
        XorY = _y;
    }

    public bool NextBoolean()
    {
        bool final;

        if(bufferMask > 0)
        {
            final = (buffer & bufferMask) == 0;
            bufferMask >>= 1;

            return final;
        }

        ulong tempX;
        ulong tempY;

        tempX = XorY;

        XorX ^= XorX << 23;

        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        buffer = tempY + XorY;

        XorX = tempX;
        XorY = tempY;

        bufferMask = 0x8000000000000000;
        return (buffer & 0xF000000000000000) == 0;
    }

    public byte NextByte()
    {
        if(bufferMask >= 8)
        {
            byte final = (byte)buffer;
            buffer >>= 8;
            bufferMask >>= 8;

            return final;
        }

        ulong tempX;
        ulong tempY;

        tempX = XorY;

        XorX ^= XorX << 23;

        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        buffer = tempY + XorY;

        XorX = tempX;
        XorY = tempY;

        bufferMask = 0x8000000000000;
        return (byte)(buffer >>= 8);
    }

    public short NextInt16()
    {
        short final;
        ulong tempX;
        ulong tempY;

        tempX = XorY;
        XorX ^= XorX << 23;
        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        final = (short)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
    public int NextInt32()
    {
        int final;
        ulong tempX;
        ulong tempY;

        tempX = XorY;

        XorX ^= XorX << 23;

        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        final = (int)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
    public long NextInt64()
    {
        long final;
        ulong tempX, tempY;

        tempX = XorY;
        XorX ^= XorX << 23;
        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        final = (long)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }

    public ushort NextUInt16()
    {
        ushort final;
        ulong tempX, tempY;

        tempX = XorY;

        XorX ^= XorX << 23;

        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorX >> 26);

        final = (ushort)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
    public uint NextUInt32()
    {
        uint final;
        ulong tempX, tempY;

        tempX = XorY;
        XorX ^= XorX << 23;
        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        final = (uint)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
    public ulong NextUInt64()
    {
        ulong final;
        ulong tempX, tempY;

        tempX = XorY;
        XorX ^= XorX << 23; tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        final = (ulong)(tempY + XorY);

        XorX = tempX;
        XorY = tempY;

        return final;
    }

    public double NextDouble()
    {
        double final;
        ulong tempX, tempY, tempZ;

        tempX = XorY;
        XorX ^= XorX << 23;
        tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        tempZ = tempY + XorY;
        final = DOUBLE_UNIT * (0x7FFFFFFF & tempZ);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
    public decimal NextDecimal()
    {
        decimal final;
        int lo, mid, hi;
        ulong tempX, tempY, tempZ;

        tempX = XorY;
        XorX ^= XorX << 23; tempY = XorX ^ XorY ^ (XorX >> 17) ^ (XorY >> 26);

        tempZ = tempY + XorY;

        hi = (int)(tempZ & 0x1FFFFFFF);
        mid = (int)(tempZ >> 16);
        lo = (int)(tempZ >> 32);

        final = new decimal(lo, mid, hi, false, 28);

        XorX = tempX;
        XorY = tempY;

        return final;
    }
}
