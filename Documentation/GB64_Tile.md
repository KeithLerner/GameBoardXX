---
updated: 2023-09-26
---
#### GB64
Designed to contain a large amount of data per tile for complex Game boards. Contains extra data bytes for mailbox style piece/token representation.
##### Byte 0: Height byte
- Stores height values.
##### Byte 1: Qualities byte
- Bits 0-2: Tile type
	- 0: Not on Board
		- Not a part of the visible board.
		- Not part of the interactable playspace.
			- Tile can not be entered.
	- 1: Out of Bounds
		- Part of the visible board.
		- Not part of the interactable playspace.
			- Tile can not be entered.
	- 2: In Bounds
		- Part of the visible board.
		- Part of the interactable playspace.
	- 3: Trap
		- Part of the visible board.
		- Part of the interactable playspace.
		- Trigger trap effect on tile enter.
	- 4: Hazard
		- Part of the visible board.
		- Part of the interactable playspace.
		- Trigger hazard effect on end turn in tile.
	- 5: Departed
		- Part of the visible board.
		- Part of the interactable playspace.
		- Trigger departed effect on tile exit.
	- 6: Custom 1
		- Part of the visible board.
		- Part of the interactable playspace.
		- For custom trigger types.
	- 7: Custom 2
		- Part of the visible board.
		- Part of the interactable playspace.
		- For custom trigger types.
- Bits 3-5: Common Layer Selector
	- 0: None.
	- 1: Start.
	- 2: End.
	- 3: Jail.
	- 4: Reward.
	- 5: A.
	- 6: B.
	- 7: Special.
- Bits 6-7: Grouped
	- 0: Ungrouped Tile / None.
	- 1: Group A.
	- 2: Group B.
	- 3: Group C.
##### Bytes 2-7: Custom Data
- Allows for users to attach custom data to their tiles.
- 64bit Tiles come with 6 bytes of storage for custom data.