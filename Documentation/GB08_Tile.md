---
updated: 2023-09-08
resources: https://en.wikipedia.org/wiki/ISO/IEC_8859-1
---
#### GB08
Designed to contain a small amount of data per tile for game boards.
##### The Tile byte
- Bits 0-1: Height
	- 0: Underground
	- 1: Low Ground
	- 2: Mid Ground
	- 3: High Ground
- Bits 2-3: Boundaries
	- 0: Not on Board (NoB) / Not a Tile (Not a part of the visible board or physical playspace)
	- 1: Out of Bounds (OoB) / Not a Playable Tile (Part of the visible board, not the physical playspace)
	- 2: Trap / Hazard tile (Can be traveled across, discards pieces that land on)
	- 3: In Bounds (InB) / Playable Tile (Part of the physical playspace)
- Bit 4: Designated Zone flag
- Bit 5: Special Tile flag
- Bits 6-7: Linked/Large tile ID
	- 0: Unlinked Tile / None
	- 1: link Group A
	- 2: Link Group B
	- 3: Link Group C