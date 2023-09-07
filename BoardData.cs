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

    public static byte HeaderModifiersByte(bool hexTiles, bool full3D, bool linkedTiles,
    bool wrapWidth, bool wrapLength, bool twoPlayer, bool threePlayer,
    bool fourPlayer)
    {
        return (byte)(
            (hexTiles ? 1 : 0) |
            (full3D ? 1 : 0) << 1 |
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
    public const uint GB64Height = 0xFF;
    public const uint GB64HeightUnderground = 0x1;
    public const uint GB64HeightLowground = 0x2;
    public const uint GB64HeightLowMidground = 0x4;
    public const uint GB64HeightMidground = 0x8;
    public const uint GB64HeightHighMidground = 0x10;
    public const uint GB64HeightHighground = 0x20;
    public const uint GB64HeightElevatedground = 0x40;
    public const uint GB64HeightAboveground = 0x80;
    
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

    public const uint GB64BoundariesAndBarriers = 0x00FF0000;
    public const uint GB64BoundariesAndBarriersNotOnBoard = 0x10000;
    public const uint GB64BoundariesAndBarriersOutOfBounds = 0x20000;
    public const uint GB64BoundariesAndBarriersHazard = 0x30000;
    public const uint GB64BoundariesAndBarriersInBounds = 0x40000;
    public const uint GB64BoundariesAndBarriersZeroCurrencyBarrier = 0x100000;
    public const uint GB64BoundariesAndBarriersThreeCurrencyBarrier = 0x200000;
    public const uint GB64BoundariesAndBarriersSixCurrencyBarrier = 0x400000;
    public const uint GB64BoundariesAndBarriersNineCurrencyBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersZeroTurnBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersFiveTurnBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersTenTurnBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersFifteenTurnBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersZeroPieceBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersTenPieceBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersFifteenPieceBarrier = 0x800000;
    public const uint GB64BoundariesAndBarriersTwentyPieceBarrier = 0x800000;

    public const uint GB64Special = 0xFF00000000000000;
    public const uint ExtraHillA = 0x100000000000000;
    public const uint ExtraHillB = 0x200000000000000;
    public const uint ExtraHillC = 0x400000000000000;
    public const uint ExtraHomeZone = 0x800000000000000;
    public const uint ExtraEndZone = 0x100000000000000;
    public const uint ExtraSafeZone = 0x200000000000000;
    public const uint ExtraUnlinkedTile = 0x1000000000000000;
    public const uint ExtraLinkedTileA = 0x2000000000000000;
    public const uint ExtraLinkedTileB = 0x4000000000000000;
    public const uint ExtraLinkedTileC = 0x8000000000000000;
    
    public static byte[] Tile64(byte heightValues, byte blacklistPlayers, 
        byte blacklistTeams, byte homeZones, byte endZones, byte safeZones, 
        byte boundariesBarriers, byte special)
    {
        byte[] tile = { heightValues, blacklistPlayers, blacklistTeams,
        homeZones, endZones, safeZones, boundariesBarriers, special };
        return tile;
    }
    #endregion

    #region GameBoard32
    public static byte[] Tile32(byte heightValues, byte standardBlacklist, 
        byte standardZones, byte boundariesSpecial)
    {
        byte[] tile = { heightValues, standardBlacklist, standardZones,
        boundariesSpecial };
        return tile;
    }
    #endregion
    
    #region GameBoard16
    public static byte[] Tile16(byte heightValues, byte boundariesSpecialCompact, 
        byte standardZones, byte boundariesSpecial)
    {
        byte[] tile = { heightValues, boundariesSpecialCompact };
        return tile;
    }
    #endregion
    
    #region GameBoard08
    
    #endregion
    
}
