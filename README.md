# 🐟**Stockfish in VR**
---

_This project is a proof of concept for implementing a uci protocol chess engine in a virutal reality environment._

## Helpful Language
UCI [Universal Chess Interface]
:  Text based protocol used by engines to play chess games automatically. [uci protocol](http://wbec-ridderkerk.nl/html/UCIProtocol.html)

FEN [Forsyth–Edwards Notation]
: A description of a board state as a string, using letters and numbers to define the pieces position and empty spaces. Following is an example of a string depicting the beginning state of a board as a FEN string.
 **rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1**

**Stockfish** is one of many UCI compatible chess engines. It was chosen for use here due to its availability and prestige in the world of chess engines.

This test is accomplished in **Unity** using the **XR Interaction Toolkit** and tested in a **Quest 2**.

***Program Flow***
1. Read state of 3D chess board in Unity Engine
2. Convert state to valid FEN string that can be passed to engine
3. Run engine with given board state
4. Take results from engine computation and effect corresponding asset (Chess Piece on Board).

**TO ACCOMPLISH**
- Handle captures
- Handle end of game



