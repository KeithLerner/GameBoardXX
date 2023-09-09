---
updated: 2023-09-08
---
#### GB64
Designed to contain a large amount of data per tile for complex Game boards.
##### Byte 0: Height and Link byte
- Bit 0: Underground
- Bit 1: Low Ground
- Bit 2: Mid Ground
- Bit 3: High Ground
- Bit 4: Elevated Ground
- Bit 5: Aboveground
- Bits 6-7: Linked
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group C
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
- The special byte allows for users to store custom data to their tiles
- Can store up to 256 traits, or 255 with a non-trait value
	- Can store a 16 by 16 array of options
