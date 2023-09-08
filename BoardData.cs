using System;

public static class BoardData
{

    #region Core
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

    public static const byte HeaderLength = 8;
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
        throw new Exception("Bonus Board Features Byte is meant to be overridden.")
    }


    public static const byte FooterLength = 128;
    public static byte[] Footer()
    {
        return new byte[1];
    }
    #endregion

    #region GameBoard64
    public const uint GB64HeightLink = 0xFF;
    public const uint GB64HeightLinkUnderground = 0x1;
    public const uint GB64HeightLinkLowground = 0x2;
    public const uint GB64HeightLinkMidground = 0x4;
    public const uint GB64HeightLinkHighground = 0x8;
    public const uint GB64HeightLinkElevatedground = 0x10;
    public const uint GB64HeightLinkAboveground = 0x20;
    public const uint GB64HeightLinkUnlinked = 0x40;
    public const uint GB64HeightLinkLinkedA = 0x80;
    public const uint GB64HeightLinkLinkedB = 0x80;
    public const uint GB64HeightLinkLinkedC = 0x80;
    
    public const uint GB64PlayerBlacklist = 0xFF00;
    public const uint GB64PlayerBlacklistP1 = 0x100;
    public const uint GB64PlayerBlacklistP2 = 0x200;
    public const uint GB64PlayerBlacklistP3 = 0x400;
    public const uint GB64PlayerBlacklistP4 = 0x800;
    public const uint GB64PlayerBlacklistFFAP1 = 0x1000;
    public const uint GB64PlayerBlacklistFFAP2 = 0x2000;
    public const uint GB64PlayerBlacklistFFAP3 = 0x4000;
    public const uint GB64PlayerBlacklistFFAP4 = 0x8000;
    
    public const uint GB64TeamBlacklist = 0xFF0000;
    public const uint GB64TeamBlacklistONEvtwo = 0x10000;
    public const uint GB64TeamBlacklistonevTWO = 0x20000;
    public const uint GB64TeamBlacklistONEvthree = 0x40000;
    public const uint GB64TeamBlacklistonevTHREE = 0x80000;
    public const uint GB64TeamBlacklistTWOvtwo = 0x100000;
    public const uint GB64TeamBlacklisttwovTWO = 0x200000;
    public const uint GB64TeamBlacklistIndividuals = 0x400000;
    public const uint GB64TeamBlacklistTeams = 0x800000;

    public const uint GB64HomeZone = 0xFF000000;
    public const uint GB64HomeZoneP1 = 0x10000;
    public const uint GB64HomeZoneP2 = 0x20000;
    public const uint GB64HomeZoneP3 = 0x40000;
    public const uint GB64HomeZoneP4 = 0x80000;
    public const uint GB64HomeZoneTeamA = 0x100000;
    public const uint GB64HomeZoneTeamB = 0x200000;
    public const uint GB64HomeZoneTeamC = 0x400000;
    public const uint GB64HomeZoneAll = 0x800000;

    public const uint GB64EndZone = 0xFF00000000;
    public const uint GB64EndZoneP1 = 0x100000000;
    public const uint GB64EndZoneP2 = 0x200000000;
    public const uint GB64EndZoneP3 = 0x400000000;
    public const uint GB64EndZoneP4 = 0x800000000;
    public const uint GB64EndZoneTeamA = 0x1000000000;
    public const uint GB64EndZoneTeamB = 0x2000000000;
    public const uint GB64EndZoneTeamC = 0x4000000000;
    public const uint GB64EndZoneAll = 0x8000000000;

    public const uint GB64SafeZone = 0xFF0000000000;
    public const uint GB64SafeZoneP1 = 0x10000000000;
    public const uint GB64SafeZoneP2 = 0x20000000000;
    public const uint GB64SafeZoneP3 = 0x40000000000;
    public const uint GB64SafeZoneP4 = 0x80000000000;
    public const uint GB64SafeZoneTeamA = 0x100000000000;
    public const uint GB64SafeZoneTeamB = 0x200000000000;
    public const uint GB64SafeZoneTeamC = 0x400000000000;
    public const uint GB64SafeZoneAll = 0x800000000000;

    public const uint GB64BoundariesAndBarriers = 0xFF000000000000;
    public const uint GB64BoundariesAndBarriersNotOnBoard = 0x0;
    public const uint GB64BoundariesAndBarriersOutOfBounds = 0x1000000000000;
    public const uint GB64BoundariesAndBarriersHazard = 0x2000000000000;
    public const uint GB64BoundariesAndBarriersInBounds = 0x3000000000000;
    public const uint GB64BoundariesAndBarriersZeroCurrencyBarrier = 0x0;
    public const uint GB64BoundariesAndBarriersThreeCurrencyBarrier = 0x4000000000000;
    public const uint GB64BoundariesAndBarriersSixCurrencyBarrier = 0x8000000000000;
    public const uint GB64BoundariesAndBarriersNineCurrencyBarrier = 0xC000000000000;
    public const uint GB64BoundariesAndBarriersZeroTurnBarrier = 0x0;
    public const uint GB64BoundariesAndBarriersFiveTurnBarrier = 0x10000000000000;
    public const uint GB64BoundariesAndBarriersTenTurnBarrier = 0x20000000000000;
    public const uint GB64BoundariesAndBarriersFifteenTurnBarrier = 0x30000000000000;
    public const uint GB64BoundariesAndBarriersZeroPieceBarrier = 0x0;
    public const uint GB64BoundariesAndBarriersTenPieceBarrier = 0x40000000000000;
    public const uint GB64BoundariesAndBarriersFifteenPieceBarrier = 0x80000000000000;
    public const uint GB64BoundariesAndBarriersTwentyPieceBarrier = 0xC0000000000000;

    public const uint GB64Special = 0xFF00000000000000;


    public static byte[] Tile64(byte heightLinkValues, byte blacklistPlayers, 
        byte blacklistTeams, byte homeZones, byte endZones, byte safeZones, 
        byte boundariesBarriers, byte special)
    {
        return { heightLinkValues, blacklistPlayers, blacklistTeams,
        homeZones, endZones, safeZones, boundariesBarriers, special };
    }
    public static ulong TileULONG(byte heightLinkValues, byte blacklistPlayers, 
        byte blacklistTeams, byte homeZones, byte endZones, byte safeZones, 
        byte boundariesBarriers, byte special)
    {
        byte[] tile = { heightLinkValues, blacklistPlayers, blacklistTeams,
        homeZones, endZones, safeZones, boundariesBarriers, special };
        return BitConverter.ToUInt64(tile);
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
        return { heightLinkValues, standardBlacklist, standardZones,
        boundariesSpecial };
    }
    public static uint TileUINT(byte heightLinkValues, byte standardBlacklist, 
        byte standardZones, byte boundariesSpecial)
    {
        byte[] tile = { heightLinkValues, standardBlacklist, standardZones,
        boundariesSpecial };
        return BitConverter.ToUInt32(tile);
    }
    #endregion
    
    #region GameBoard16
    public const uint GB16HeightLink = 0xFF;
    public const uint GB16HeightLinkUnderground = 0x1;
    public const uint GB16HeightLinkLowground = 0x2;
    public const uint GB16HeightLinkMidground = 0x4;
    public const uint GB16HeightLinkHighground = 0x8;
    public const uint GB16HeightLinkElevatedground = 0x10;
    public const uint GB16HeightLinkAboveground = 0x20;
    public const uint GB16HeightLinkUnlinked = 0x40;
    public const uint GB16HeightLinkLinkedA = 0x80;
    public const uint GB16HeightLinkLinkedB = 0x80;
    public const uint GB16HeightLinkLinkedC = 0x80;
    
    public const uint GB16CompactProperties = 0xFF00;
    public const uint GB16CompactPropertiesNotOnBoard = 0x0;
    public const uint GB16CompactPropertiesOutOfBounds = 0x100;
    public const uint GB16CompactPropertiesHazard = 0x200;
    public const uint GB16CompactPropertiesInBounds = 0x300;
    public const uint GB16CompactPropertiesZoneP1 = 0x0;
    public const uint GB16CompactPropertiesZoneP2 = 0x400;
    public const uint GB16CompactPropertiesZoneP3 = 0x800;
    public const uint GB16CompactPropertiesZoneP4 = 0xC00;
    public const uint GB16CompactPropertiesZoneAll = 0x0;
    public const uint GB16CompactPropertiesZoneHome = 0x1000;
    public const uint GB16CompactPropertiesZoneEnd = 0x2000;
    public const uint GB16CompactPropertiesZoneSafe = 0x4000;
    public const uint GB16CompactPropertiesSpecial = 0xC000;


    public static byte[] Tile16(byte heightLinkValues, byte compactProperties)
    {
        return { heightLinkValues, compactProperties };
    }
    public static ushort TileUSHORT(byte heightLinkValues, byte compactProperties)
    {
        return { heightLinkValues, compactProperties };
    }
    #endregion
    
    #region GameBoard08
    public const uint GB08 = 0xFF;
    public const uint GB08Underground = 0x0;
    public const uint GB08Lowground = 0x1;
    public const uint GB08Midground = 0x2;
    public const uint GB08Highground = 0x3;
    public const uint GB08BoundariesSpecialNotOnBoard = 0x0;
    public const uint GB08BoundariesSpecialOutOfBounds = 0x4;
    public const uint GB08BoundariesSpecialHazard = 0x8;
    public const uint GB08BoundariesSpecialInBounds = 0xC;
    public const uint GB08Zone = 0x10;
    public const uint GB08Special = 0x20;
    public const uint GB08Unlinked = 0x0;
    public const uint GB08LinkedA = 0x40;
    public const uint GB08LinkedB = 0x80;
    public const uint GB08LinkedC = 0xC0;
    #endregion
    
}
