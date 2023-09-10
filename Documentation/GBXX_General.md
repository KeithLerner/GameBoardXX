---
updated: 2023-09-10
---
### Board Header
The first 8 bytes of any GameBoardXX file stores critical Board information. 
- Bytes 0-3: File Format Header Data
	- Acts as a magic string; Confirms to a decoder that the file type is correct.
	- Byte 0: 0x47
	- Byte 1: 0x42
	- Byte 2-3: Format Variant (XX).
		- 08: 0x3038
			- Decodes to "08" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 16: 0x3136
			- Decodes to "16" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 32: 0x3332
			- Decodes to "32" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 64: 0x3634
			- Decodes to "64" using ISO/IEC8859-1[^ISO/IEC8859-1].
- Byte 4: Width.
    - Must be a non-zero, positive integer, less than or equal to 255.
- Byte 5: Length.
	- Must be a non-zero, positive integer, less than or equal to 255.
- Byte 6: Board Modifiers Flags.
	- Bit 0: Tallest Height Only Flag
		- When on: Only the highest bit of height is significant.
	- Bit 1: Hex Tiles Flag
		- Tiles are hexagon shaped.
		- This is achieved by alternating column heights. 
			- Zero-indexed even columns (and zero) remain the same height.
			- Zero-indexed odd columns are intepreted as a half height up from their coordinate.
	- Bit 2: Linked/Large Tiles Flag
		- Tiles may share area and neighboring faces.
			- Handled with 3 link groups.
	- Bit 3: Wrap Width Flag
		- The outermost tiles along the width axis are "connected" to their matching length tile at the opposite edge
	- Bit 4: Wrap Length Flag
		- The outermost tiles along the length axis are "connected" to their matching width tile at the opposite edge
	- Bit 5: 2 Player Flag
		- Indicates the board is designed for 2 player matches
	- Bit 6: 3 Player Flag
		- Indicates the board is designed for 3 player matches
	- Bit 7: 4 Player Flag
		- Indicates the board is designed for 4 player matches
- Byte 7: Bonus Board Features Flags.
	- Stores custom data about the Board
	- Intended to be used as a filter in board seach tools.


### Board Footer
The last 128 bytes of the file store authoring data for the Board.
- Byte 0: Board Version Number
	- 0-255 -> 1-256
- Bytes 1-2: Editor ID
	- 2 char editor symbol using ISO/IEC8859-1[^ISO/IEC8859-1].
	- allows for giving 3rd party editors credit for their part in Board creation.
- Bytes 3-7: Board Code
	- 5 char unique Board code for downloading/uploading.
	- Used to identify and store boards in a public look up table.
- Bytes 8-68: Board Title
	- An array containing the characters representing the Board's name
	- Using ISO/IEC8859-1[^ISO/IEC8859-1]
	- Length of 60 chars
- Bytes 69-128: Board Author
	- An array containing the characters representing the author's name
	- Using ISO/IEC8859-1[^ISO/IEC8859-1]
	- Length of 59 chars

[^ISO/IEC8859-1]: [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1)

### Board Body
The space between Header and Footer is the Body. This space is occupied by tiles.

The size of every tile in the body is determined by which variant of GameBoardXX is indicated in the Header section. The Header section also dictates that a board can not have dimensions greater than 255 in either width or length, meaning the body can be up to 65,025 tiles long. 

File size for a minimum area board is 137 bytes for a board with 8 bit tiles, and 200 bytes for a board with 64 bit tiles. This 'minimum' board would be nothing more than a bloated unsigned integer with built a in header and footer.

File size for a maximum area board is 65,161 bytes for a board with 8 bit tiles, and 520,336 bytes for a board with 64 bit tiles.

Find specific details of the different tile variants below:
- [64 bit tiles](GB64_Tile.md)
- [32 bit tiles](GB32_Tile.md)
- [16 bit tiles](GB16_Tile.md)
- [08 bit tiles](GB08_Tile.md)