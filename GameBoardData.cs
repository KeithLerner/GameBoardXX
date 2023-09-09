using System;

public static class GameBoardData
{

    #region Core
    public const byte HeaderLength = 8;
    public enum GameBoardXXVariant { _08, _16, _32, _64 }
    public static byte[] Header(GameBoardXXVariant gbxxv, byte BoardWidth, byte BoardLength, byte modifiers,
    byte bonusFeatures)
    {
        byte[] header = 
        {
            0x47, 
            0x42,
            gbxxv switch
            {
                GameBoardXXVariant._08 => 0x30,
                GameBoardXXVariant._16 => 0x31,
                GameBoardXXVariant._32 => 0x33,
                GameBoardXXVariant._64 => 0x36,
                _ => throw new Exception("Invalid Tile Size")
            },
            gbxxv switch
            {
                GameBoardXXVariant._08 => 0x38,
                GameBoardXXVariant._16 => 0x36,
                GameBoardXXVariant._32 => 0x32,
                GameBoardXXVariant._64 => 0x34,
                _ => throw new Exception("Invalid Tile Size")
            },
            BoardWidth, 
            BoardLength, 
            modifiers, 
            bonusFeatures
        };
        return header;
    }
    public static byte[] Header(byte tileBitSize, byte BoardWidth, byte BoardLength, byte modifiers,
    byte bonusFeatures)
    {
        byte[] header = 
        {
            0x47, 
            0x42,
            tileBitSize switch
            {
                08 => 0x30,
                16 => 0x31,
                32 => 0x33,
                64 => 0x36,
                _ => throw new Exception("Invalid Tile Size")
            },
            tileBitSize switch
            {
                08 => 0x38,
                16 => 0x36,
                32 => 0x32,
                64 => 0x34,
                _ => throw new Exception("Invalid Tile Size")
            },
            BoardWidth, 
            BoardLength, 
            modifiers, 
            bonusFeatures
        };
        return header;
    }

    public static byte HeaderModifiersByte(bool specialData, bool hexTiles, bool linkedTiles,
    bool wrapWidth, bool wrapLength, bool twoPlayer, bool threePlayer,
    bool fourPlayer)
    {
        return (byte)(
            (specialData ? 1 : 0) |
            (hexTiles ? 1 : 0) << 1 |
            (linkedTiles ? 1 : 0) << 2 |
            (wrapWidth ? 1 : 0) << 3 |
            (wrapLength ? 1 : 0) << 4 |
            (twoPlayer ? 1 : 0) << 5 |
            (threePlayer ? 1 : 0) << 6 |
            (fourPlayer ? 1 : 0) << 7
            );
    }

    public static byte BonusBoardFeaturesByte()
    {
        throw new Exception("Bonus GameBoard Features Byte is meant to be " +
            "overridden.");
    }


    public const byte FooterLength = 128;
    public static byte[] Footer(byte boardVersionNumber, byte[] editorID, 
        byte[] boardCode, byte[] title, byte[] author)
    {
        byte[] footer = new byte[128];
        footer[0] = boardVersionNumber;
        Array.Copy(editorID, 0, footer, 1, 2);
        Array.Copy(boardCode, 0, footer, 3, 5);
        Array.Copy(title, 0, footer, 6, 60);
        Array.Copy(author, 0, footer, 69, 59);
        return footer;
    }

    public struct ConversionAssist
    {
        public readonly byte Width;
        public readonly byte TileSize;

        public ConversionAssist(byte width, byte tileSize)
        {
            this.Width = width;
            this.TileSize = tileSize;
        }
    }
    public static uint CoordinateToRawIndex((byte, byte) coordinates, byte width)
    {
        return (ushort)(HeaderLength +
            (coordinates.Item1 + coordinates.Item2 * width));
    }

    public static ushort CoordinateToTileIndex((byte, byte) coordinates, byte width, byte tileSize)
    {
        return (ushort)(HeaderLength +
            (coordinates.Item1 + coordinates.Item2 * width) * tileSize);
    }

    public static (byte, byte) RawIndexToCoordinate(uint tileIndex, byte width)
    {
        throw new NotImplementedException("THIS FUNCTION HAS NOT BEEN " +
            "IMPLEMENTED YET");


    }

    public static (byte, byte) TileIndexToCoordinate(ushort tileIndex, byte width)
    {
        /*
        throw new NotImplementedException("THIS FUNCTION HAS NOT BEEN " +
            "IMPLEMENTED YET");
        */
        // STARTED TESTING THIS, DID IT FREE HAND, DON'T KNOW IF IT WORKED
        return (
            (byte)((tileIndex - HeaderLength) / width),
            (byte)((tileIndex - HeaderLength) % width)
            );

    }
    #endregion

    #region GameBoard64
    public const ulong GB64HeightLink = 0xFF;
    public const ulong GB64HeightLinkUnderground = 0x1;
    public const ulong GB64HeightLinkLowground = 0x2;
    public const ulong GB64HeightLinkMidground = 0x4;
    public const ulong GB64HeightLinkHighground = 0x8;
    public const ulong GB64HeightLinkElevatedground = 0x10;
    public const ulong GB64HeightLinkAboveground = 0x20;
    public const ulong GB64HeightLinkUnlinked = 0x40;
    public const ulong GB64HeightLinkLinkedA = 0x80;
    public const ulong GB64HeightLinkLinkedB = 0x80;
    public const ulong GB64HeightLinkLinkedC = 0x80;
    
    public const ulong GB64PlayerBlacklist = 0xFF00;
    public const ulong GB64PlayerBlacklistP1 = 0x100;
    public const ulong GB64PlayerBlacklistP2 = 0x200;
    public const ulong GB64PlayerBlacklistP3 = 0x400;
    public const ulong GB64PlayerBlacklistP4 = 0x800;
    public const ulong GB64PlayerBlacklistFFAP1 = 0x1000;
    public const ulong GB64PlayerBlacklistFFAP2 = 0x2000;
    public const ulong GB64PlayerBlacklistFFAP3 = 0x4000;
    public const ulong GB64PlayerBlacklistFFAP4 = 0x8000;
    
    public const ulong GB64TeamBlacklist = 0xFF0000;
    public const ulong GB64TeamBlacklistONEvtwo = 0x10000;
    public const ulong GB64TeamBlacklistonevTWO = 0x20000;
    public const ulong GB64TeamBlacklistONEvthree = 0x40000;
    public const ulong GB64TeamBlacklistonevTHREE = 0x80000;
    public const ulong GB64TeamBlacklistTWOvtwo = 0x100000;
    public const ulong GB64TeamBlacklisttwovTWO = 0x200000;
    public const ulong GB64TeamBlacklistIndividuals = 0x400000;
    public const ulong GB64TeamBlacklistTeams = 0x800000;

    public const ulong GB64HomeZone = 0xFF000000;
    public const ulong GB64HomeZoneP1 = 0x10000;
    public const ulong GB64HomeZoneP2 = 0x20000;
    public const ulong GB64HomeZoneP3 = 0x40000;
    public const ulong GB64HomeZoneP4 = 0x80000;
    public const ulong GB64HomeZoneTeamA = 0x100000;
    public const ulong GB64HomeZoneTeamB = 0x200000;
    public const ulong GB64HomeZoneTeamC = 0x400000;
    public const ulong GB64HomeZoneAll = 0x800000;

    public const ulong GB64EndZone = 0xFF00000000;
    public const ulong GB64EndZoneP1 = 0x100000000;
    public const ulong GB64EndZoneP2 = 0x200000000;
    public const ulong GB64EndZoneP3 = 0x400000000;
    public const ulong GB64EndZoneP4 = 0x800000000;
    public const ulong GB64EndZoneTeamA = 0x1000000000;
    public const ulong GB64EndZoneTeamB = 0x2000000000;
    public const ulong GB64EndZoneTeamC = 0x4000000000;
    public const ulong GB64EndZoneAll = 0x8000000000;

    public const ulong GB64SafeZone = 0xFF0000000000;
    public const ulong GB64SafeZoneP1 = 0x10000000000;
    public const ulong GB64SafeZoneP2 = 0x20000000000;
    public const ulong GB64SafeZoneP3 = 0x40000000000;
    public const ulong GB64SafeZoneP4 = 0x80000000000;
    public const ulong GB64SafeZoneTeamA = 0x100000000000;
    public const ulong GB64SafeZoneTeamB = 0x200000000000;
    public const ulong GB64SafeZoneTeamC = 0x400000000000;
    public const ulong GB64SafeZoneAll = 0x800000000000;

    public const ulong GB64BoundariesBarriers = 0xFF000000000000;
    public const ulong GB64BoundariesBarriersNotOnBoard = 0x0;
    public const ulong GB64BoundariesBarriersOutOfBounds = 0x1000000000000;
    public const ulong GB64BoundariesBarriersHazard = 0x2000000000000;
    public const ulong GB64BoundariesBarriersInBounds = 0x3000000000000;
    public const ulong GB64BoundariesBarriersZeroCurrencyBarrier = 0x0;
    public const ulong GB64BoundariesBarriersThreeCurrencyBarrier = 0x4000000000000;
    public const ulong GB64BoundariesBarriersSixCurrencyBarrier = 0x8000000000000;
    public const ulong GB64BoundariesBarriersNineCurrencyBarrier = 0xC000000000000;
    public const ulong GB64BoundariesBarriersZeroTurnBarrier = 0x0;
    public const ulong GB64BoundariesBarriersFiveTurnBarrier = 0x10000000000000;
    public const ulong GB64BoundariesBarriersTenTurnBarrier = 0x20000000000000;
    public const ulong GB64BoundariesBarriersFifteenTurnBarrier = 0x30000000000000;
    public const ulong GB64BoundariesBarriersZeroPieceBarrier = 0x0;
    public const ulong GB64BoundariesBarriersTenPieceBarrier = 0x40000000000000;
    public const ulong GB64BoundariesBarriersFifteenPieceBarrier = 0x80000000000000;
    public const ulong GB64BoundariesBarriersTwentyPieceBarrier = 0xC0000000000000;

    public const ulong GB64Special = 0xFF00000000000000;


    public static byte[] Tile64(byte heightLinkValues, byte blacklistPlayers, 
        byte blacklistTeams, byte homeZones, byte endZones, byte safeZones, 
        byte boundariesBarriers, byte special)
    {
        return new byte[]{ heightLinkValues, blacklistPlayers, blacklistTeams,
        homeZones, endZones, safeZones, boundariesBarriers, special };
    }
    public static byte[] Tile64(ulong[] traits)
    {
        ulong t64 = 0;
        for (byte i = 0; i < traits.Length; i++)
        {
            t64 |= (ulong)traits[i];
        }
        return BitConverter.GetBytes(t64);
    }
    public static byte[] Tile64(ulong t64)
    {
        return BitConverter.GetBytes(t64);
    }
    public static ulong TileULONG(byte heightLinkValues, byte blacklistPlayers, 
        byte blacklistTeams, byte homeZones, byte endZones, byte safeZones, 
        byte boundariesBarriers, byte special)
    {
        return BitConverter.ToUInt64(new byte[]{
            heightLinkValues, blacklistPlayers, blacklistTeams,
        homeZones, endZones, safeZones, boundariesBarriers, special }, 0);
    }
    #endregion

    #region GameBoard32
    public const uint GB32HeightLink = 0xFF;
    public const uint GB32HeightLinkUnderground = 0x1;
    public const uint GB32HeightLinkLowground = 0x2;
    public const uint GB32HeightLinkMidground = 0x4;
    public const uint GB32HeightLinkHighground = 0x8;
    public const uint GB32HeightLinkElevatedground = 0x10;
    public const uint GB32HeightLinkAboveground = 0x20;
    public const uint GB32HeightLinkUnlinked = 0x40;
    public const uint GB32HeightLinkLinkedA = 0x80;
    public const uint GB32HeightLinkLinkedB = 0x80;
    public const uint GB32HeightLinkLinkedC = 0x80;
    
    public const uint GB32StandardBlacklist = 0xFF00;
    public const uint GB32StandardBlacklistP1 = 0x100;
    public const uint GB32StandardBlacklistP2 = 0x200;
    public const uint GB32StandardBlacklistP3 = 0x400;
    public const uint GB32StandardBlacklistP4 = 0x800;
    public const uint GB32StandardBlacklistTA = 0x1000;
    public const uint GB32StandardBlacklistTB = 0x2000;
    public const uint GB32StandardBlacklistTC = 0x4000;
    public const uint GB32StandardBlacklistTD = 0x8000;
    
    public const uint GB32StandardZone = 0xFF0000;
    public const uint GB32StandardZoneTAHome = 0x10000;
    public const uint GB32StandardZoneTBHome = 0x20000;
    public const uint GB32StandardZoneTCHome = 0x40000;
    public const uint GB32StandardZoneTDHome = 0x80000;
    public const uint GB32StandardZoneTAEnd = 0x100000;
    public const uint GB32StandardZoneTBEnd = 0x200000;
    public const uint GB32StandardZoneTCEnd = 0x400000;
    public const uint GB32StandardZoneTDEnd = 0x800000;

    public const uint GB32BoundariesSpecial = 0xFF000000;
    public const uint GB32BoundariesSpecialBoundaries = 0xF000000;
    public const uint GB32BoundariesSpecialNotOnBoard = 0x0;
    public const uint GB32BoundariesSpecialOutOfBounds = 0x1000000;
    public const uint GB32BoundariesSpecialHazard = 0x2000000;
    public const uint GB32BoundariesSpecialInBounds = 0x3000000;
    public const uint GB32BoundariesSpecialLink = 0xC000000;
    public const uint GB32BoundariesSpecialUnlinked = 0x0;
    public const uint GB32BoundariesSpecialLinkedA = 0x4000000;
    public const uint GB32BoundariesSpecialLinkedB = 0x8000000;
    public const uint GB32BoundariesSpecialLinkedC = 0xC000000;
    public const uint GB32BoundariesSpecialSpecial = 0xF0000000;


    public static byte[] Tile32(byte heightLinkValues, byte standardBlacklist, 
        byte standardZones, byte boundariesSpecial)
    {
        return new byte[]{ heightLinkValues, standardBlacklist, standardZones,
        boundariesSpecial };
    }
    public static byte[] Tile32(uint[] traits)
    {
        uint t32 = 0;
        for (byte i = 0; i < traits.Length; i++)
        {
            t32 |= traits[i];
        }
        return BitConverter.GetBytes(t32);
    }
    public static byte[] Tile32(uint t32)
    {
        return BitConverter.GetBytes(t32);
    }
    public static uint TileUINT(byte heightLinkValues, byte standardBlacklist, 
        byte standardZones, byte boundariesSpecial)
    {
        byte[] tile = { heightLinkValues, standardBlacklist, standardZones,
        boundariesSpecial };
        return BitConverter.ToUInt32(tile, 0);
    }
    #endregion
    
    #region GameBoard16
    public const ushort GB16HeightLink = 0xFF;
    public const ushort GB16HeightLinkUnderground = 0x1;
    public const ushort GB16HeightLinkLowground = 0x2;
    public const ushort GB16HeightLinkMidground = 0x4;
    public const ushort GB16HeightLinkHighground = 0x8;
    public const ushort GB16HeightLinkElevatedground = 0x10;
    public const ushort GB16HeightLinkAboveground = 0x20;
    public const ushort GB16HeightLinkUnlinked = 0x40;
    public const ushort GB16HeightLinkLinkedA = 0x80;
    public const ushort GB16HeightLinkLinkedB = 0x80;
    public const ushort GB16HeightLinkLinkedC = 0x80;
    
    public const ushort GB16CompactProperties = 0xFF00;
    public const ushort GB16CompactPropertiesNotOnBoard = 0x0;
    public const ushort GB16CompactPropertiesOutOfBounds = 0x100;
    public const ushort GB16CompactPropertiesHazard = 0x200;
    public const ushort GB16CompactPropertiesInBounds = 0x300;
    public const ushort GB16CompactPropertiesZoneP1 = 0x0;
    public const ushort GB16CompactPropertiesZoneP2 = 0x400;
    public const ushort GB16CompactPropertiesZoneP3 = 0x800;
    public const ushort GB16CompactPropertiesZoneP4 = 0xC00;
    public const ushort GB16CompactPropertiesZoneAll = 0x0;
    public const ushort GB16CompactPropertiesZoneHome = 0x1000;
    public const ushort GB16CompactPropertiesZoneEnd = 0x2000;
    public const ushort GB16CompactPropertiesZoneSafe = 0x4000;
    public const ushort GB16CompactPropertiesSpecial = 0xC000;


    public static byte[] Tile16(byte heightLinkValues, 
        byte compactProperties)
    {
        return new byte[]{ heightLinkValues, compactProperties };
    }
    public static byte[] Tile16(ushort[] traits)
    {
        ushort t16 = 0;
        for (byte i = 0; i < traits.Length; i++)
        {
            t16 |= traits[i];
        }
        return BitConverter.GetBytes(t16);
    }
    public static byte[] Tile16(ushort t16)
    {
        return BitConverter.GetBytes(t16);
    }
    public static ushort TileUSHORT(byte heightLinkValues, 
        byte compactProperties)
    {
        return BitConverter.ToUInt16(new byte[]{ heightLinkValues, 
            compactProperties }, 0);
    }
    #endregion
    
    #region GameBoard08
    public const byte GB08 = 0xFF;
    public const byte GB08Underground = 0x0;
    public const byte GB08Lowground = 0x1;
    public const byte GB08Midground = 0x2;
    public const byte GB08Highground = 0x3;
    public const byte GB08BoundariesNotOnBoard = 0x0;
    public const byte GB08BoundariesOutOfBounds = 0x4;
    public const byte GB08BoundariesHazard = 0x8;
    public const byte GB08BoundariesInBounds = 0xC;
    public const byte GB08DesignatedZone = 0x10;
    public const byte GB08SpecialTile = 0x20;
    public const byte GB08Unlinked = 0x0;
    public const byte GB08LinkedA = 0x40;
    public const byte GB08LinkedB = 0x80;
    public const byte GB08LinkedC = 0xC0;

    public static byte[] Tile08(byte[] traits)
    {
        byte t08 = 0;
        for (byte i = 0; i < traits.Length; i++)
        {
            t08 |= traits[i];
        }
        return BitConverter.GetBytes(t08);
    }
    #endregion

}
