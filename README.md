# Othello Minimax Algorithm

Othello 

## Table of Contents

- [Othello Minimax Algorithm](#Othello-Minimax-Algorithm)
  - [Table of Contents](#table-of-contents)
  - [About the Project](#about-the-project)
  - [Prerequisites](#prerequisites)
  - [Instructions](#instructions)
  - [Rules](#rules)
  - [License](#license)
  - [Contact](#contact)

## About the Project

This game AI was written for a Computer Science assignment in Algoritms and Datastructures at Stockholm University.  
Algorithm is Minimax with α-β pruning and iterative deepening using a heap datastructure.  
It searches 7-10 moves ahead for the optimal move.  

Use this repository to learn about Minimax, α-β pruning and iterative deepening.  

[bergsten.itch.io/othello](https://bergsten.itch.io/othello)

![image](https://github.com/CaretSoftware/Othello-Minimax/assets/69549081/0b4d8d98-306e-4f9d-b63c-f0a9f06c871d)

## Prerequisites

Unity 2022.2.12f1

## Instructions

Click to place tile.
You play ⚪
AI plays  ⚫
Objective is to have the majority of discs with your color facing up at the end of the game.
(If no valid white placement, click anywhere on board to secede turn)

## Rules

White makes first move.*
The opponents discs is flipped if outflanked by two other players discs. 
Valid Disc placement is that it must flip at least one of the opponents discs.
If no valid move the turn goes to the opponent.
If neither player has valid moves the game is over.
If all discs are placed or there is no more valid placements the game is over.
Strategy:
Don't be fooled about how seemingly badly the AI plays in the beginning.
It's playing 10 moves ahead and choosing the move that's optimal later as opposed to good now. The board state in a game of Othello can flip quickly, literally.
Aim to place your discs in the corners! Corners can never be flipped and are therefore very valuable.
The edges are likewise more valuable than the rest of the playing field because they're statistically more difficult to outflank.
Othello is as much about playing your opponents discs as much as your own.
Make sure to not give it easy flips.

## License

Copyright (c) 2024 Patrik Bergsten

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Contact

- Email: pbergsten@me.com
- Website: [patrikbergsten.com](https://www.patrikbergsten.com)
- Itch.io: [bergsten.itch.io](https://bergsten.itch.io)
- Twitter: [@patrik_bergsten](https://twitter.com/patrik_bergsten)
- GitHub: [CaretSoftware](https://github.com/CaretSoftware)
