# 🐟**Stockfish in VR**
---

_This project is a proof of concept for implementing a uci protocol chess engine in a virutal reality environment._

![Unity Preview Screenshot with debugger output](/images/stockfish_screengrab.PNG)

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

### Big Learning Experience
So up until this point, most games and experiences I have created are small enough where optimization is simple and performance is not a huge concern. For this tool I had to dive in to mutlithreading. The stockfish "terminal"
can not run on the main thread. My first attempts to do so resulted in a horrific lag or complete freeze in the VR headset and caused my first in headset bout with nausea. Digging into multithreading, per suggestion of a friend, we have remedies the solution. Now, when stockfish is called, a seperate thread process is created and runs stockfish as a windowless program. Once stockfish has a suggested move, the thread return the move and closes down.

## About Me
Hi! I am a student at Holberton. I am finishing the Full Stack Software Engineering program with a specialization in Augmented and Virtual Reality Development. I have found a healthy interest in creating behind the scenes tools for VR experiences. Welcome and feel free to click around. Please contact me with any thoughts, suggestions, or job opportunities!

🐦 [twitter](https://twitter.com/whoziwhatzit_)
💼 [linkedin](https://www.linkedin.com/in/aydentownsley/)



