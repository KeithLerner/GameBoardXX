---
updated: 2023-09-23
---
#### GB64
Designed to contain a large amount of data per tile for complex Game boards. Contains extra data bytes for mailbox style piece/token representation.
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
##### Byte 1: Qualities byte
- Bits 0-2: Tile type
	- 0: Not on Board
		- Not a part of the visible board 
		- Not part of the interactable playspace
			- Tile can not be entered
	- 1: Out of Bounds
		- Part of the visible board
		- Not part of the interactable playspace 
			- Tile can not be entered
	- 2: In Bounds
		- Part of the visible board
		- Part of the interactable playspace
	- 3: Trap
		- Part of the visible board
		- Part of the interactable playspace
		- Trigger trap effect on tile enter
	- 4: Hazard
		- Part of the visible board
		- Part of the interactable playspace
		- Trigger hazard effect on end turn in tile
	- 5: Departed
		- Part of the visible board
		- Part of the interactable playspace
		- Trigger departed effect on tile exit
	- 6: Custom 1
		- Part of the visible board
		- Part of the interactable playspace
		- For custom trigger types
	- 7: Custom 2
		- Part of the visible board
		- Part of the interactable playspace
		- For custom trigger types
- Bits 3-4: Common Layer Selector
	- 0: None
	- 1: Start
	- 2: End
	- 3: Special
- Bit 5: 
- Bit 6: 
- Bit 7: 
##### Byte 2: Tile Style byte
- Bits 0-2: Decoration ID
- Bits 3-7: Tile Style
	- Based on a provided tile palette with 32 tiles
##### Byte 3: Special byte
- The special byte allows for users to attach custom data to their tiles
##### Bytes 4-7: Custom Data
