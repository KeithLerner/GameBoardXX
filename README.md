---
updated: 2023-09-17
resources: https://en.wikipedia.org/wiki/ISO/IEC_8859-1
---
# GameBoardXX
GameBoardXX (GBXX) is a **work in progress** tile-based board creation library for digital board games.
GBXX is ***NOT* production-ready yet** and is ***LIKELY* to change unpredictably until v1.0**. A roadmap is in development to provide more clarity on features, versions, and release timings.
GBXX started as a personal project with a very specific use case, and as such contains strange mixes of generalized and specified data. Designing a more generalized and user-friendly format is currently the highest priority.

## GameBoardXX File Format
Different GameBoard size variants:
- GameBoard64 (GB64, 64 bit tiles) 
- GameBoard32 (GB32, 32 bit tiles)
- GameBoard16 (GB16, 16 bit tiles)
- GameBoard08 (GB08,  8 bit tiles)
GameBoard64 represents the highest detail a tile can have, all other GameBoard formats will compromise features for storage efficiency.
