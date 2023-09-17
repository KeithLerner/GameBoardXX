---
updated: 2023-09-08
resources: https://en.wikipedia.org/wiki/ISO/IEC_8859-1
---
# GameBoardXX
GameBoardXX is a work-in-progress tile-based board creation library for digital board games.

## GameBoardXX File Format
Different GameBoard format variants exist:
- GameBoard64 (GB64, 64 bit tiles) 
- GameBoard32 (GB32, 32 bit tiles)
- GameBoard16 (GB16, 16 bit tiles)
- GameBoard08 (GB08,  8 bit tiles)

GameBoard64 represents the highest detail a tile can have, all other GameBoard formats will compromise features for storage efficiency.

Game Board file extensions are GBXX where XX is the number of bits per tile.
