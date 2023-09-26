---
updated: 2023-09-08
---
## GameBoardXX
### Best Practices 
- While GBXX supports up to 255 in any dimension, boards larger than 16 in any dimension may experience performance drops when interacting with board data at speed.
	- Currently, the all types are object oriented. Eventually a data oriented approach will be implimented to try to increase cache hits.

## The .GBXX File
### Board Header
The first 8 bytes of any GameBoardXX file stores critical Board information. 
- Bytes 0-3: File Format Header Data
	- Acts as a magic string; Confirms to a decoder that the file type is correct.
	- Byte 0: `0x47`
	- Byte 1: `0x42`
	- Byte 2-3: Format Variant (XX) / Tile bit depth.
		- 08: `0x3038`
			- Decodes to "08" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 16: `0x3136`
			- Decodes to "16" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 32: `0x3332`
			- Decodes to "32" using ISO/IEC8859-1[^ISO/IEC8859-1].
		- 64: `0x3634`
			- Decodes to "64" using ISO/IEC8859-1[^ISO/IEC8859-1].
- Byte 4: Width.
    - Must be a non-zero, positive integer, less than or equal to 255.
- Byte 5: Height.
	- Must be a non-zero, positive integer, less than or equal to 255.
- Byte 6: Length.
	- Must be a non-zero, positive integer, less than or equal to 255.
- Byte 7: File Modifiers.
	- Bit 0: Hex Shaped Tiles Toggle.
		- Tiles are hexagon shaped on the horizontal plane.
		- This is achieved by alternating column heights. 
			- Zero-indexed even columns (and zero) remain the same height.
			- Zero-indexed odd columns are intepreted as a half height up from their coordinate.
				- **THERE IS A NAME FOR THIS STYLE OF GRID; REDBLOBGAMES HEX SHIT HAS THE INFO**
	- Bits 1-2: Board Wrap Enum.
		- 0: No Wrapping.
		- 1: Width.
			- The outermost tiles (ones that share the same max or min width coordinate) along the width axis are "connected" to their matching length tile at the opposite edge.
		- 2: Wrap Length Flag.
			- The outermost tiles (ones that share the same max or min length coordinate) along the length axis are "connected" to their matching width tile at the opposite edge.
		- 3: Wrap Height Flag.
			- The outermost tiles (ones that share the same max or min height coordinate) along the height axis are "connected" to their matching width tile at the opposite edge.
	- Bits 3-7: **NEED STUFF HERE THAT MODIFIES HOW THE BOARD IS INTERACTED WITH**


### Board Body
The Body is the space between the Header and the Footer. This space is occupied by tiles.

The size of every tile in the body is determined by which variant of GameBoardXX is indicated in the Header section. The Header section also dictates that a board can not have dimensions greater than 255 in any dimension, meaning the body can range from 1 to 16,581,375 tiles in length. 

File size for a minimum area board is 137 bytes for a board with 8 bit tiles, and 200 bytes for a board with 64 bit tiles. This 'minimum' board would be nothing more than a bloated unsigned integer with built a in header and footer.

File size for a maximum area board is 16,581,375 bytes (about 16MB) for a board with 8 bit tiles, and 1,061,208,000 bytes (about 1GB) for a board with 64 bit tiles.

Find specific details of the different tile variants below:
- [64 bit tiles](GB64_Tile.md)
- [32 bit tiles](GB32_Tile.md)
- [16 bit tiles](GB16_Tile.md)
- [08 bit tiles](GB08_Tile.md)


### Board Footer
The last 128 bytes of the file store authoring data for the Board.
- Byte 0: Board Version Number
	- 0-255
		- 0 represents unstable dev builds
- Bytes 1-2: Editor ID
	- 2 char editor symbol using ISO/IEC8859-1[^ISO/IEC8859-1].
	- allows for giving 3rd party editors credit for their part in Board creation.
- Bytes 3-7: Board Code
	- 5 char universally unique Board code for downloading/uploading.
	- Used to identify and store boards in a public look up tables
	- This will require a package manager type solution and tighter file format security.
		- Not sure this is within the scope of this project, file can reach sizes too large to personally host or pay to have hosted for free. Maybe one day if this format becomes used enough, someone will impliment a solution for this. A trade off for a large user base is that unique ID's might require more than 5 chars.
- Bytes 8-68: Board Title
	- An array containing the characters representing the Board's name
	- Using ISO/IEC8859-1[^ISO/IEC8859-1]
	- Length of 60 chars
- Bytes 69-128: Board Author
	- An array containing the characters representing the author's name
	- Using ISO/IEC8859-1[^ISO/IEC8859-1]
	- Length of 59 chars



[^ISO/IEC8859-1]: [ISO/IEC 8859-1](https://en.wikipedia.org/wiki/ISO/IEC_8859-1)