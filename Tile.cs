using System;
using System.Collections;
using System.Collections.Generic;

public class Tile 
{
    private byte[] m_Value;
    public byte[] Value => m_Value;

    private readonly byte m_TileSize;
    public byte TileSize => m_TileSize;


    public Tile(byte[] data)
    {
        if (data.length > 8) throw new Error("Invalid Tile Size.");
        m_Value = data;
        m_TileSize = data.length;
    }


    public enum Comparisons { LessThan, LessThanOrEqualTo, EqualTo, GreaterThanOrEqualTo, GreaterThan }
    public enum Traits { Height, PlayerBlacklist, TeamBlacklist, HomeZone, EndZone, SafeZone, Boundary, Barrier, Linked, Special }
    public static bool Compare(this Tile lhs, Tile rhs, Traits trait, Comparisons comparator)
    {
        switch (trait)
        {
            default:
                return comparator switch {
                    Comparisons.LessThan => lhs.Value < rhs.Value,
                    Comparisons.LessThanOrEqualTo => lhs.Value <= rhs.Value,
                    Comparisons.EqualTo => lhs.Value == rhs.Value,
                    Comparisons.GreaterThanOrEqualTo => lhs.Value >= rhs.Value,
                    Comparisons.GreaterThan => lhs.Value > rhs.Value,
                    _ => throw new Error("Undefined Behaviour in Value Comparison.")
                }
                break;
        }
    }
}