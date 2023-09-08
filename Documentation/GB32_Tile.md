---
updated: 2023-09-08
resources: https://en.wikipedia.org/wiki/ISO/IEC_8859-1
---
#### GB32
Designed to contain a moderate amount of data per tile for complex game boards.
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
- Bit 0: No Zone
- Bit 1: Player 1 Zone
- Bit 2: Player 2 Zone
- Bit 3: Player 3 Zone
- Bit 4: Player 4 zone
- Bit 5: Team 1 Zone
- Bit 6: Team 2 Zone
- Bit 7: Team 3 Zone
##### Byte 3: Boundaries and Special byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-3: Zone style
	- 0: All
	- 1: Home
	- 2: End
	- 3: Safe
- Bits 4-7: Special Data
	- 4 bits for users to store custom data to their tiles