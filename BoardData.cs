using System;

public static class BoardData
{

    #region Core
    /*
    //   Could be temp/deleted/movedtoanotherregion/reworked for the 
    //   gameBoard 32 bit -> gameBoardXX transition
    //
    public const uint Environments = 0x000000FF;
    public const uint EnvironmentNotOnBoard = 0x1;
    public const uint EnvironmentOutOfBounds = 0x2;
    public const uint EnvironmentCosmetic = 0x4;
    public const uint EnvironmentLowGround = 0x8;
    public const uint EnvironmentLowLinkGround = 0x10;
    public const uint EnvironmentMidGround = 0x20;
    public const uint EnvironmentHighLinkGround = 0x40;
    public const uint EnvironmentHighGround = 0x80;
    //
    public const uint BlacklistPlayers = 0x0000FF00;
    public const uint BlacklistFFAPlayer1 = 0x100;
    public const uint BlacklistFFAPlayer2 = 0x200;
    public const uint BlacklistFFAPlayer3 = 0x400;
    public const uint BlacklistFFAPlayer4 = 0x800;
    public const uint Blacklist1vXPlayer1 = 0x1000;
    public const uint Blacklist1vXPlayer2 = 0x2000;
    public const uint Blacklist1vXPlayer3 = 0x4000;
    public const uint Blacklist1vXPlayer4 = 0x8000;
    //
    public const uint BlacklistTeams = 0x00FF0000;
    public const uint Blacklist1v2Team1 = 0x10000;
    public const uint Blacklist1v2Team2 = 0x20000;
    public const uint Blacklist1v3Team1 = 0x40000;
    public const uint Blacklist1v3Team2 = 0x80000;
    public const uint Blacklist2v2Team1 = 0x100000;
    public const uint Blacklist2v2Team2 = 0x200000;
    public const uint Blacklist1v1vXIndividuals = 0x400000;
    public const uint Blacklist1v1vXTeam = 0x800000;
    //
    //
    public const uint Extras = 0xFF000000;
    public const uint ExtraHillA = 0x1000000;
    public const uint ExtraHillB = 0x2000000;
    public const uint ExtraHillC = 0x4000000;
    public const uint ExtraHomeZone = 0x8000000;
    public const uint ExtraEndZone = 0x10000000;
    public const uint ExtraSafeZone = 0x20000000;
    public const uint ExtraUnlinkedTile = 0x0;
    public const uint ExtraLinkedTileA = 0x40000000;
    public const uint ExtraLinkedTileB = 0x80000000;
    public const uint ExtraLinkedTileC = 0xC0000000;
    //
    */
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
