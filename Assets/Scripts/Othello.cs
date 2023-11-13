using System;
using System.Diagnostics;

public class Othello {
    public event Action<int, int> ScoreUpdate;
    public event Action<string> Message;
    public event Action UpdateVisuals;

    private const int MinimaxDepth = 10;
    private const int MinimaxMaxMilliseconds = 1000;

    private const int BoardWidth = 8;
    private const int NumDiscsInBoard = 64;
    private const int White = 1;
    private const int Black = -1;
    private const int Blank = 0;
    private const int GameOverDiscPoints = 3000000;
    private const int EndGameDiscPoints = 1000;
    private const int LegalMovesScore = 100;
    private const int EndGameDiscLimit = 14;
    
    private readonly int[] scoreBoard = {
        1000, -100,  150,  100,  100,  150, -100, 1000,
        -100, -200,   20,   20,   20,   20, -200, -100,
         150,   20,   15,   15,   15,   15,   20,  150,
         100,   20,   15,   10,   10,   15,   20,  100,
         100,   20,   15,   10,   10,   15,   20,  100,
         150,   20,   15,   15,   15,   15,   20,  150,
        -100, -200,   20,   20,   20,   20, -200, -100,
        1000, -100,  150,  100,  100,  150, -100, 1000
    };
    private readonly int[,] _numSquaresToEdge = new int[64, 8] {
        { 7, 0, 0, 7, 0, 0, 7, 0 },
        { 7, 0, 1, 6, 1, 0, 6, 0 },
        { 7, 0, 2, 5, 2, 0, 5, 0 },
        { 7, 0, 3, 4, 3, 0, 4, 0 },
        { 7, 0, 4, 3, 4, 0, 3, 0 },
        { 7, 0, 5, 2, 5, 0, 2, 0 },
        { 7, 0, 6, 1, 6, 0, 1, 0 },
        { 7, 0, 7, 0, 7, 0, 0, 0 },
        { 6, 1, 0, 7, 0, 1, 6, 0 },
        { 6, 1, 1, 6, 1, 1, 6, 1 },
        { 6, 1, 2, 5, 2, 1, 5, 1 },
        { 6, 1, 3, 4, 3, 1, 4, 1 },
        { 6, 1, 4, 3, 4, 1, 3, 1 },
        { 6, 1, 5, 2, 5, 1, 2, 1 },
        { 6, 1, 6, 1, 6, 1, 1, 1 },
        { 6, 1, 7, 0, 6, 0, 0, 1 },
        { 5, 2, 0, 7, 0, 2, 5, 0 },
        { 5, 2, 1, 6, 1, 2, 5, 1 },
        { 5, 2, 2, 5, 2, 2, 5, 2 },
        { 5, 2, 3, 4, 3, 2, 4, 2 },
        { 5, 2, 4, 3, 4, 2, 3, 2 },
        { 5, 2, 5, 2, 5, 2, 2, 2 },
        { 5, 2, 6, 1, 5, 1, 1, 2 },
        { 5, 2, 7, 0, 5, 0, 0, 2 },
        { 4, 3, 0, 7, 0, 3, 4, 0 },
        { 4, 3, 1, 6, 1, 3, 4, 1 },
        { 4, 3, 2, 5, 2, 3, 4, 2 },
        { 4, 3, 3, 4, 3, 3, 4, 3 },
        { 4, 3, 4, 3, 4, 3, 3, 3 },
        { 4, 3, 5, 2, 4, 2, 2, 3 },
        { 4, 3, 6, 1, 4, 1, 1, 3 },
        { 4, 3, 7, 0, 4, 0, 0, 3 },
        { 3, 4, 0, 7, 0, 4, 3, 0 },
        { 3, 4, 1, 6, 1, 4, 3, 1 },
        { 3, 4, 2, 5, 2, 4, 3, 2 },
        { 3, 4, 3, 4, 3, 4, 3, 3 },
        { 3, 4, 4, 3, 3, 3, 3, 4 },
        { 3, 4, 5, 2, 3, 2, 2, 4 },
        { 3, 4, 6, 1, 3, 1, 1, 4 },
        { 3, 4, 7, 0, 3, 0, 0, 4 },
        { 2, 5, 0, 7, 0, 5, 2, 0 },
        { 2, 5, 1, 6, 1, 5, 2, 1 },
        { 2, 5, 2, 5, 2, 5, 2, 2 },
        { 2, 5, 3, 4, 2, 4, 2, 3 },
        { 2, 5, 4, 3, 2, 3, 2, 4 },
        { 2, 5, 5, 2, 2, 2, 2, 5 },
        { 2, 5, 6, 1, 2, 1, 1, 5 },
        { 2, 5, 7, 0, 2, 0, 0, 5 },
        { 1, 6, 0, 7, 0, 6, 1, 0 },
        { 1, 6, 1, 6, 1, 6, 1, 1 },
        { 1, 6, 2, 5, 1, 5, 1, 2 },
        { 1, 6, 3, 4, 1, 4, 1, 3 },
        { 1, 6, 4, 3, 1, 3, 1, 4 },
        { 1, 6, 5, 2, 1, 2, 1, 5 },
        { 1, 6, 6, 1, 1, 1, 1, 6 },
        { 1, 6, 7, 0, 1, 0, 0, 6 },
        { 0, 7, 0, 7, 0, 7, 0, 0 },
        { 0, 7, 1, 6, 0, 6, 0, 1 },
        { 0, 7, 2, 5, 0, 5, 0, 2 },
        { 0, 7, 3, 4, 0, 4, 0, 3 },
        { 0, 7, 4, 3, 0, 3, 0, 4 },
        { 0, 7, 5, 2, 0, 2, 0, 5 },
        { 0, 7, 6, 1, 0, 1, 0, 6 },
        { 0, 7, 7, 0, 0, 0, 0, 7 }};
    private readonly int[] _directions = {
        8, -8, -1, 1, 7, -7, 9, -9
    };

    private ulong _whitesBoard = (1UL << (3 + 4 * BoardWidth))
        | (1UL << (4 + 3 * BoardWidth));
    private ulong _blacksBoard = (1UL << (3 + 3 * BoardWidth))
            | (1UL << (4 + 4 * BoardWidth));
    private ulong _occupiedSquares;
    private ulong _potentialSquares;
    private int _discsLeft = NumDiscsInBoard - 4;
    private int _lastPlacedCoord = -1;
    private int _whitePoints = 2;
    private int _blackPoints = 2;
    private int _turn = White;
    private bool _noMovesPrevTurn = false;

    private int _numChecks;
    private int _numPrunesMax;
    private int _numPrunesMin;

    public Othello() {

        int[] startingTiles = { 3 + 4 * BoardWidth, 4 + 3 * BoardWidth, 3 + 3 * BoardWidth, 4 + 4 * BoardWidth };
        for (int i = 0; i < 4; i++) {
            PotentialValidSquares(ref _potentialSquares, ref _occupiedSquares, startingTiles[i]);
        }
    }

    public bool Move(int coord) {

        if (ValidMove(coord, White, _blacksBoard, _whitesBoard)) {
            PlaceDisc(coord, White, ref _blacksBoard, ref _whitesBoard);
            PotentialValidSquares(ref _potentialSquares, ref _occupiedSquares, coord);
            _lastPlacedCoord = coord;
            _discsLeft--;
            ChangeTurn();
            CalculateScore();
            ScoreUpdate?.Invoke(_whitePoints, _blackPoints);
            UpdateVisuals?.Invoke();
            return true;
        } else if (HasToSkip(White, _blacksBoard, _whitesBoard) && !_noMovesPrevTurn) {
            _noMovesPrevTurn = true;
            ChangeTurn();
            UpdateVisuals?.Invoke();
            ScoreUpdate?.Invoke(_whitePoints, _blackPoints);
            Minimax();
            Message?.Invoke("Player Forced To Skip!");
            ChangeTurn();
            CalculateScore();
            ScoreUpdate?.Invoke(_whitePoints, _blackPoints);
            UpdateVisuals?.Invoke();
        } else if (_noMovesPrevTurn) {
            Message?.Invoke("STALEMATE " + (_blackPoints > _whitePoints ? "AI Win!" : "Human Win!"));
        }
        return false;
    }

    /** <summary>
     * Minimax algorithm with alpha-beta-pruning and iterative deepening.
     * </summary>
    */
    public void Minimax() {

        MaxHeap maxHeap = new MaxHeap(NumDiscsInBoard);
        MaxHeap nextHeap;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        _numChecks = 0;
        _numPrunesMax = 0;
        _numPrunesMin = 0;
        int bestMove = -1;
        int maxEval = int.MinValue;

        int numValidSquares = 0;
        for (int i = 0; i < NumDiscsInBoard; i++) {
            if (IsNeighbouringTile(_potentialSquares, i)) {
                HeapNode heapNode = new HeapNode(i, i);
                maxHeap.Add(heapNode);
                numValidSquares++;
            }
        }

        int iterativeDepth = 1;
        for ( ; iterativeDepth <= MinimaxDepth && stopwatch.ElapsedMilliseconds < MinimaxMaxMilliseconds; iterativeDepth++) {

            bestMove = -1;
            maxEval = int.MinValue;
            nextHeap = new MaxHeap(numValidSquares);

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            while (!maxHeap.IsEmpty()) {
                HeapNode nextCoord = maxHeap.Pop();
                int coord = nextCoord.value;

                if (IsNeighbouringTile(_potentialSquares, coord) && ValidMove(coord, Black, _blacksBoard, _whitesBoard)) {

                    ulong tempBlackBoard = _blacksBoard;
                    ulong tempWhiteBoard = _whitesBoard;
                    PlaceDisc(coord, Black, ref tempBlackBoard, ref tempWhiteBoard);

                    ulong newPotentialSquares = _potentialSquares;
                    ulong newOccupiedSquares = _occupiedSquares;
                    PotentialValidSquares(ref newPotentialSquares, ref newOccupiedSquares, coord);

                    int eval = Minimax(iterativeDepth, false, alpha, beta, _discsLeft, tempBlackBoard, tempWhiteBoard, _noMovesPrevTurn, newPotentialSquares, newOccupiedSquares);

                    HeapNode heapNode = new HeapNode(eval, coord);
                    nextHeap.Add(heapNode);

                    alpha = Math.Max(alpha, eval);
                    if (eval > maxEval) {
                        maxEval = eval;
                        bestMove = coord;
                    }
                }
            }

            numValidSquares = nextHeap.Size();
            maxHeap = nextHeap;
        }

        if (bestMove > -1) {
            PlaceDisc(bestMove, Black, ref _blacksBoard, ref _whitesBoard);

            PotentialValidSquares(ref _potentialSquares, ref _occupiedSquares, bestMove);

            _discsLeft--;
            _lastPlacedCoord = bestMove;
        } else {
            Message?.Invoke("NO MOVES");
            _lastPlacedCoord = -1;
        }

        CalculateScore();
        ChangeTurn();
        UpdateVisuals?.Invoke();
        ScoreUpdate?.Invoke(_whitePoints, _blackPoints);

        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;

        string message = String.Format("board iterations: {0:n0} | time: {1:00}.{2:00} | depth: {3} | prunes: {4:n0} | best score: {5:n0} | tiles left: {6}",
                _numChecks, ts.Seconds, ts.Milliseconds, iterativeDepth - 1, _numPrunesMin + _numPrunesMax, maxEval, _discsLeft);
        Message?.Invoke(message);
    }

    /** <summary>
     * <para>
     * This method determines the optimal move for the AI player.
     * It alternates between playing all possible moves as the maximizing,
     * and minimizing player from the resulting board state. 
     * Maximizing choosing the most favourable move for <paramref name="Black"/>
     * and minimizing choosing the least favourable for <paramref name="Black"/>.
     * </para>
     * <para>
     * When maximum depth, or game over state has been reached; the boardstate
     * is evaluated for heuristic score for black player. <see cref="Evaluation"/>
     * </para>
     * </summary>
     * <param name="depth">the current moves ahead minimax has left to play.
     * </param>
     * <param name="maximizingPlayer">boolean to keep track of if the method is 
     * choosing the greatest or the lowest valued move.</param>
     * <param name="alpha">the highest valued move so far.</param>
     * <param name="beta">the lowest valued move so far.</param>
     * <param name="discsLeft">how many discs are left to play.</param>
     * <param name="blackBoard">the one-dimensional 64-bit bitrepresentation
     * of positions of the black discs on the board.</param>
     * <param name="whiteBoard">the one-dimensional 64-bit bitrepresentation
     * of positions of the white discs on the board.</param>
     * <param name="haspassed">if previous Minimax had to skip.</param>
     * <param name="neighbours">neighbouring squares to occupied squares 
     * that are potential placements.</param>
     * <param name="occupied">the tiles occupied by discs. Used as mask 
     * for neighbours.</param>
     * <returns>A score of the max or min evaluated boardstate when maximum 
     * depth har been reached or when tiles have run out.</returns>
    */
    private int Minimax(int depth, bool maximizingPlayer, int alpha, int beta, int discsLeft, 
        ulong blackBoard, ulong whiteBoard, bool hasPassed, ulong neighbours, ulong occupied) {

        _numChecks++;

        if (depth <= 0 || discsLeft <= 0) {
            return Evaluation(discsLeft, blackBoard, whiteBoard, false);
        }

        if (maximizingPlayer) {

            int maxEval = int.MinValue;
            bool forcedToPass = true;
            for (int coord = 0; coord < NumDiscsInBoard; coord++) {
                if (IsNeighbouringTile(_potentialSquares, coord) && 
                    ValidMove(coord, Black, blackBoard, whiteBoard)) {

                    forcedToPass = false;

                    ulong tempBlackBoard = blackBoard;
                    ulong tempWhiteBoard = whiteBoard;
                    PlaceDisc(coord, Black, ref tempBlackBoard, ref tempWhiteBoard);

                    ulong newNeighbours = neighbours;
                    ulong newOccupied = occupied;
                    PotentialValidSquares(ref newNeighbours, ref newOccupied, coord);

                    int eval = Minimax(depth - 1, false, alpha, beta, discsLeft - 1, 
                        tempBlackBoard, tempWhiteBoard, false, newNeighbours, newOccupied);

                    maxEval = Math.Max(maxEval, eval);
                    if (eval >= beta) {
                        _numPrunesMax++;
                        break;
                    }
                    alpha = Math.Max(alpha, eval);
                }
            }
            if (forcedToPass) {
                if (hasPassed) {
                    return Evaluation(discsLeft, blackBoard, whiteBoard, true);
                }

                maxEval = Minimax(depth - 1, false, alpha, beta, discsLeft, 
                    blackBoard, whiteBoard, true, neighbours, occupied);
            }

            return maxEval;
        } else {

            int minEval = int.MaxValue;
            bool forcedToPass = true;
            for (int coord = 0; coord < NumDiscsInBoard; coord++) {
                if (IsNeighbouringTile(_potentialSquares, coord) && 
                    ValidMove(coord, White, blackBoard, whiteBoard)) {

                    forcedToPass = false;

                    ulong tempBlackBoard = blackBoard;
                    ulong tempWhiteBoard = whiteBoard;
                    PlaceDisc(coord, White, ref tempBlackBoard, ref tempWhiteBoard);

                    ulong newNeighbours = neighbours;
                    ulong newOccupied = occupied;
                    PotentialValidSquares(ref newNeighbours, ref newOccupied, coord);

                    int eval = Minimax(depth - 1, true, alpha, beta, discsLeft - 1, 
                        tempBlackBoard, tempWhiteBoard, false, newNeighbours, newOccupied);

                    minEval = Math.Min(minEval, eval);
                    if (eval <= alpha) {
                        _numPrunesMin++;
                        break;
                    }
                    beta = Math.Min(beta, eval);
                }
            }
            if (forcedToPass) {
                if (hasPassed) {
                    return Evaluation(discsLeft, blackBoard, whiteBoard, true);
                }

                minEval = Minimax(depth - 1, true, alpha, beta, discsLeft, 
                    blackBoard, whiteBoard, true, neighbours, occupied);
            }

            return minEval;
        }
    }

    /** <summary>
    * This method evaluates the boardstate for the <paramref name="Black"/> player.
    * Evaluation takes into account the current score, how many legal moves
    * there are for <paramref name="Black"/> player, and placement of tiles on stable positions.
    * Discbalance is evaluated more highly only by the end as to prioritize 
    * placement of stable disc early in the game.
    * </summary>
    * <param name="tilesLeft">the amount of tiles left currently.</param>
    * <param name="blackBoard">the one-dimensional 64-bit bitrepresentation
    * of positions of the black discs on the board.</param>
    * <param name="whiteBoard">the one-dimensional 64-bit bitrepresentation
    * of positions of the white discs on the board.</param>
    * <param name="stalemate">if there is a stalemate triggering a game over.</param>
    * <returns>an evaluation of the boardstate according to set heuristiks.</returns>
    */
    private int Evaluation(int tilesLeft, ulong blackBoard, ulong whiteBoard, bool stalemate) {

        int discsBalance = 0;
        int legalMovesCount = 0;
        int placementBonus = 0;

        for (int coord = 0; coord < NumDiscsInBoard; coord++) {
            if ((blackBoard & (1UL << coord)) != 0) {
                discsBalance++;
                placementBonus += scoreBoard[coord];
            } else if ((whiteBoard & (1UL << coord)) != 0) {
                discsBalance--;
                placementBonus -= scoreBoard[coord];
            }
            if (ValidMove(coord, Black, blackBoard, whiteBoard)) {
                legalMovesCount += LegalMovesScore;
            }
            if (ValidMove(coord, White, blackBoard, whiteBoard)) {
                legalMovesCount -= LegalMovesScore;
            }
        }

        if (tilesLeft <= 0 || stalemate) {
            return discsBalance * GameOverDiscPoints;
        }

        if (tilesLeft < EndGameDiscLimit) {
            discsBalance *= EndGameDiscPoints;
        }

        return discsBalance + legalMovesCount + placementBonus;
    }

    private bool IsNeighbouringTile(ulong potentialSquares, int coord) {
        return (potentialSquares & (1UL << coord)) != 0;
    }

    private bool HasToSkip(int sidesTurn, ulong blackBoard, ulong whiteBoard) {

        for (int coord = 0; coord < NumDiscsInBoard; coord++) {
            if (ValidMove(coord, sidesTurn, blackBoard, whiteBoard))
                return false;
        }
        return true;
    }

    private void PotentialValidSquares(ref ulong neighbours, ref ulong occupied, int coord) {

        for (int direction = 0; direction < _directions.Length; direction++) {
            if (_numSquaresToEdge[coord, direction] > 0) {
                neighbours |= 1UL << (coord + _directions[direction]);
            }
        }
        occupied |= 1UL << coord;
        neighbours &= ~occupied;
    }

    private bool ValidMove(int coord, int sidesTurn, ulong blackBoard, ulong whiteBoard) {

        if ((blackBoard & (1UL << coord)) != 0 ||
            (whiteBoard & (1UL << coord)) != 0) // Occupied space
            return false;

        ulong opponent = sidesTurn == Black ? whiteBoard : blackBoard;
        ulong proponent = sidesTurn == White ? whiteBoard : blackBoard;

        for (int dirIndex = 0; dirIndex < _directions.Length; dirIndex++) {
            int square = coord + _directions[dirIndex];

            for (int n = 0; n < _numSquaresToEdge[coord, dirIndex] - 1; n++) {

                if (!IsDiscBelongingTo(square, opponent)) {
                    break;
                }
                if (IsDiscBelongingTo(square + _directions[dirIndex], proponent)) {
                    _noMovesPrevTurn = false;
                    return true;
                }
                square += _directions[dirIndex];
            }
        }
        return false;
    }

    private void PlaceDisc(int coord, int sidesTurn, ref ulong blackBoard, ref ulong whiteBoard) {

        ulong opponent = sidesTurn == Black ? whiteBoard : blackBoard;
        ulong proponent = sidesTurn == White ? whiteBoard : blackBoard;
        ulong tilesToFlip = 0UL;

        for (int dirIndex = 0; dirIndex < 8; dirIndex++) {
            int square = coord + _directions[dirIndex];
            ulong tilesToFlipInDir = 0UL;
            for (int n = 0; n < _numSquaresToEdge[coord, dirIndex] - 1; n++) {

                if (!IsDiscBelongingTo(square, opponent)) {
                    break;
                }
                tilesToFlipInDir |= (1UL << square);
                if (IsDiscBelongingTo(square + _directions[dirIndex], proponent)) {
                    tilesToFlip |= tilesToFlipInDir;
                    break;
                }
                square += _directions[dirIndex];
            }
        }

        blackBoard ^= tilesToFlip;
        whiteBoard ^= tilesToFlip;

        if (sidesTurn == Black)
            blackBoard |= (1UL << coord);
        else
            whiteBoard |= (1UL << coord);
    }

    private void CalculateScore() {

        _blackPoints = 0;
        _whitePoints = 0;
        for (int coord = 0; coord < NumDiscsInBoard; coord++) {

            int color = 0;
            if ((_blacksBoard & (1UL << coord)) != 0) color = Black;
            else if ((_whitesBoard & (1UL << coord)) != 0) color = White;

            switch (color) {
                case Black:
                    _blackPoints++;
                    break;
                case White:
                    _whitePoints++;
                    break;
                case Blank:
                    break;
            }
        }
    }

    private bool IsDiscBelongingTo(int coord, ulong playerBoard) => (playerBoard & (1UL << coord)) != 0;

    private void ChangeTurn() => _turn *= -1;

    public int GetTurn() => _turn;

    public ulong GetWhitesBoard() => _whitesBoard;

    public ulong GetBlacksBoard() => _blacksBoard;

    public int LastPlacedCoord() => _lastPlacedCoord;
}
