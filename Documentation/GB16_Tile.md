---
updated: 2023-09-08
---
#### GB16
Designed to contain a moderate amount of data per tile for game boards.
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
##### Byte 1: Compact Properties byte
- Bits 0-1: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bits 2-3: Zones Type
	- 0: Player 1 Zone
	- 1: Player 2 Zone
	- 2: Player 3 Zone
	- 3: Player 4 zone
- Bits 4-5: Zone Style
	- 0: All
	- 1: Home
	- 2: End
	- 3: Safe
- Bits 6-7: Special Data
	- 2 bits for users to store custom data to their tiles