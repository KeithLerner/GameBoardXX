---
updated: 2023-09-06
resources: https://en.wikipedia.org/wiki/ISO/IEC_8859-1
---
# GameBoardXX
GameBoardXX is a tile based board creation library for digital board games.

## GameBoardXX File Format
Different GameBoard format variants exist:
- GameBoard64 (GB64, 64 bit tiles) 
- GameBoard32 (GB32, 32 bit tiles)
- GameBoard16 (GB16, 16 bit tiles)
- GameBoard08 (GB08,  8 bit tiles)

GameBoard64 represents the highest detail a tile can have, all other GameBoard formats will compromise features for storage efficiency.

Game Board file extensions are GBXX where XX is the number of bits per tile.

### Board Header
The first 8 bytes of any GameBoardXX file stores critical Board information. 
- Bytes 0-3: File Format Header Data
	- Acts as a magic string; Confirms to a decoder that the file type is correct.
	- Byte 0: 0x47
	- Byte 1: 0x42
	- Byte 2-3: Format Variant (XX).
		- 08: 0x3038
			- Decodes to "08" using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1).
		- 16: 0x3136
			- Decodes to "16" using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1).
		- 32: 0x3332
			- Decodes to "32" using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1).
		- 64: 0x3634
			- Decodes to "64" using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1).
- Byte 4: Width.
	- 0-255 -> 1-256
- Byte 5: Length.
	- 0-255 -> 1-256
- Byte 6: Board Modifiers Flags.
	- Bit 0: Hex Tiles Flag
	- Bit 1: Full 3D Tiles Flag
	- Bit 2: Linked/Large Tiles Flag
	- Bit 3: Wrap Width Flag
	- Bit 4: Wrap Length Flag
	- Bit 5: 2 Player Flag
	- Bit 6: 3 Player Flag
	- Bit 7: 4 Player Flag
- Byte 7: Bonus Board Features Flags.
	- Stores custom data about the Board
	- Intended to be used as a filter in board seach tools.


### Board Footer
The last 128 bytes of the file store authoring data for the Board.
- Byte 0: Board Version Number
	- 0-255 -> 1-256
- Bytes 1-2: Editor ID
	- 2 char editor symbol using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1).
	- allows for giving 3rd party editors credit for their part in Board creation.
- Bytes 3-7: Board Code
	- 5 char unique Board code for downloading/uploading.
	- Used to identify and store boards in a public look up table.
- Bytes 8-68: Board Title
	- An array containing the characters representing the Board's name
	- Using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1)
	- Length of 60 chars
- Bytes 69-128: Board Author
	- An array containing the characters representing the author's name
	- Using [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1)
	- Length of 59 chars


### Board Body
The space between Header and Footer is the Body. The body can be up to 65,536 bytes long. The size of any chunk of data for the body is determined by which variant of GameBoardXX is signified in the Header section

#### GB64
Designed to contain a large amount of data per tile for complex Game boards.
##### Byte 0: Height byte
- Bit 0: Underground
- Bit 1: Low Ground
- Bit 2: Low-Mid Ground
- Bit 3: Mid Ground
- Bit 4: High-Mid Ground
- Bit 5: High Ground
- Bit 6: Elevated Ground
- Bit 7: Aboveground 
##### Byte 1: Player Blacklist byte
- Bit 0: **Player 1** blacklisted
- Bit 1: **Player 2** blacklisted
- Bit 2: **Player 3** blacklisted
- Bit 3: **Player 4** blacklisted
- Bit 4: **1**v1v1v1 **Player 1** blacklisted
- Bit 5: 1v**1**v1v1 **Player 2** blacklisted
- Bit 6: 1v1v**1**v1 **Player 3** blacklisted
- Bit 7: 1v1v1v**1** **Player 4** blacklisted
##### Byte 2: Team Blacklist byte
- Bit 0: **1**v2 **Team 1** blacklisted
- Bit 1: 1v**2** **Team 2** blacklisted
- Bit 2: **1**v3 **Team 1** blacklisted
- Bit 3: 1v**3** **Team 2** blacklisted
- Bit 4: **2**v2 **Team 1** blacklisted
- Bit 5: 2v**2** **Team 2** blacklisted
- Bit 6: **1**v**1**vX **Individuals** blacklisted
- Bit 7: 1v1v**X** **Team** blacklisted
##### Byte 3: Home Zone byte
- Bit 0: Player 1 Home Zone
- Bit 1: Player 2 Home Zone
- Bit 2: Player 3 Home Zone
- Bit 3: Player 4 Home zone
- Bit 4: Team A Home Zone
- Bit 5: Team B Home Zone
- Bit 6: Team C Home Zone
- Bit 7: All Home Zone
##### Byte 4: End Zone byte
- Bit 0: Player 1 End Zone
- Bit 1: Player 2 End Zone
- Bit 2: Player 3 End Zone
- Bit 3: Player 4 End zone
- Bit 4: Team A End Zone
- Bit 5: Team B End Zone
- Bit 6: Team C End Zone
- Bit 7: All End Zone
##### Byte 5: Safe Zone byte
- Bit 0: Player 1 Safe Zone
- Bit 1: Player 2 Safe Zone
- Bit 2: Player 3 Safe Zone
- Bit 3: Player 4 Safe zone
- Bit 4: Team A Safe Zone
- Bit 5: Team B Safe Zone
- Bit 6: Team C Safe Zone
- Bit 7: All Safe Zone
##### Byte 6: Boundaries and Barriers byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-3: Currency Barriers
	- 0: No Currency Barrier
	- 1: 3 Currency Barrier
	- 2: 6 Currency Barrier
	- 3: 9 Currency Barrier
- Bits 4-5: Turn Barriers
	- 0: No Turn Barrier
	- 1: 5 Turn Barrier
	- 2: 10 Turn Barrier
	- 3: 15 Turn Barrier
- Bits 6-7: Piece Sum Barriers
	- 0: No Piece Sum Barrier
	- 1: 10 Piece Sum Barrier
	- 2: 15 Piece Sum Barrier
	- 3: 20 Piece Sum Barrier
##### Byte 7: Special byte
- Bits 0-2: Special Zones
	- 0: No Zone
	- 1: Zone A 
	- 2: Zone B 
	- 3: Zone C 
	- 4: Zone D
	- 5: Zone E
	- 6: Site A
	- 7: Site B
- Bit 3: KotH Hill
- Bits 4-5: Special Spawns
	- 0: No Special Spawn
	- 1: Special Spawn A
	- 2: Special Spawn B
	- 3: Special Spawn C
- Bits 6-7: Linked/Large tile ID
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group C


#### GB32
Designed to contain a moderate amount of data per tile for complex game boards.
##### Byte 0: Height byte
- Bit 0: Underground
- Bit 1: Low Ground
- Bit 2: Low-Mid Ground
- Bit 3: Mid Ground
- Bit 4: High-Mid Ground
- Bit 5: High Ground
- Bit 6: Elevated Ground
- Bit 7: Aboveground 
##### Byte 1: Standard Blacklist byte
- Bit 0: **Player 1** blacklisted
- Bit 1: **Player 2** blacklisted
- Bit 2: **Player 3** blacklisted
- Bit 3: **Player 4** blacklisted
- Bit 4: **A**vBvCvD **Team A** blacklisted
- Bit 5: Av**B**vCvD **Team B** blacklisted
- Bit 6: AvBv**C**vD **Team C** blacklisted
- Bit 7: AvBvCv**D** **Team D** blacklisted
##### Byte 2: Standard Zones byte
- Bit 0: Team A Home Zone
- Bit 1: Team B Home Zone
- Bit 2: Team C Home Zone
- Bit 3: Team D Home zone
- Bit 4: Team A End Zone
- Bit 5: Team B End Zone
- Bit 6: Team C End Zone
- Bit 7: Team D End zone
##### Byte 3: Boundaries and Special Zones byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-4: Special Zones
	- 0: No Zone
	- 1: Zone A 
	- 2: Zone B 
	- 3: Zone C 
	- 4: Zone D
	- 5: Zone E
	- 6: Site A
	- 7: Site B
- Bits 4-5: Special Spawns
	- 0: No Special Spawn
	- 1: Special Spawn A
	- 2: Special Spawn B
	- 3: Special Spawn C
- Bits 6-7: Linked/Large tile ID
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group C


#### GB16
Designed to contain a moderate amount of data per tile for game boards.
##### Byte 0: Height byte
- Bit 0: Underground
- Bit 1: Low Ground
- Bit 2: Low-Mid Ground
- Bit 3: Mid Ground
- Bit 4: High-Mid Ground
- Bit 5: High Ground
- Bit 6: Elevated Ground
- Bit 7: Aboveground 
##### Byte 1: Boundaries and Special Properties byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-4: Home Zones
	- 0: Player 1 Home Zone
	- 1: Player 2 Home Zone
	- 2: Player 3 Home Zone
	- 3: Player 4 Home zone
	- 4: Team 1 Home Zone
	- 5: Team 2 Home Zone
	- 6: Team 3 Home Zone
	- 7: All Home Zone
- Bits 5-6: Special Tiles
	- 0: No Special Tile
	- 1: Special Tile A
	- 2: Special Tile B
	- 3: Special Tile C
- Bits 6-7: Linked/Large tile ID
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group C


#### GB08
Designed to contain a small amount of data per tile for game boards.
##### The Tile byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-3: Height
	- 0: Low ground
	- 1: Low-Mid Ground
	- 2: High-Mid Ground
	- 3: High Ground
- Bit 4: Home Zone
- Bit 5: Special Tile
- Bits 6-7: Linked/Large tile ID
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group GB