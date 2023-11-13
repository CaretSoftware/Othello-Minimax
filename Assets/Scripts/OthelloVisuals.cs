using System;
using System.Text;
using System.Collections;
using UnityEngine;
using TMPro;

public class OthelloVisuals : MonoBehaviour {
    private const int Width = 8;
    private const int Blank = 0;
    private const int White = 1;
    private const int Black = -1;
    private const int LastBlack = -2;
    private const int LastWhite = 2;
    private ulong prevWhiteBoard = 0UL;
    private ulong prevBlackBoard = 0UL;
    
    [SerializeField]
    private GameObject greenSquare;
    [SerializeField]
    private GameObject whiteSquare;
    [SerializeField]
    private GameObject blackSquare;
    [SerializeField]
    private GameObject lastBlackSquare;
    [SerializeField]
    private GameObject lastWhiteSquare;
    private GameObject[] board = new GameObject[64];
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private TextMeshProUGUI scoreTextWhite;
    [SerializeField]
    private TextMeshProUGUI scoreTextBlack;

    private readonly Othello othello = new Othello();

    void Start() {
        InitBoard();
        othello.Message += Message;
        othello.ScoreUpdate += UpdateScore;
        othello.UpdateVisuals += UpdateBoard;
    }

    private void OnDestroy() {
        othello.Message -= Message;
        othello.ScoreUpdate -= UpdateScore;
        othello.UpdateVisuals -= UpdateBoard;
    }

    private void Message(String message) {
        print(message);
    }

    private void InitBoard() {
        ulong blackBoard = othello.GetBlacksBoard();
        ulong whiteBoard = othello.GetWhitesBoard();
        prevBlackBoard = blackBoard;
        prevWhiteBoard = whiteBoard;

        for (int coord = 0; coord < 64; coord++) {

            if ((blackBoard & (1UL << coord)) != 0)
                InstantiateTile(coord, Black);
            else if ((whiteBoard & (1UL << coord)) != 0)
                InstantiateTile(coord, White);
            else
                InstantiateTile(coord, Blank);
        }
    }

    private void InstantiateTile(int coord, int color) {
        if (board[coord] != null) {
            Destroy(board[coord]);
        }

        GameObject tile = null;
        switch (color) {
            case (Blank):
                tile = greenSquare;
                break;
            case (Black):
                tile = blackSquare;
                break;
            case (White):
                tile = whiteSquare;
                break;
            case (LastBlack):
                tile = lastBlackSquare;
                break;
            case (LastWhite):
                tile = lastWhiteSquare;
                break;
            default:
                break;
        }

        if (tile != null) {
            Square s = Instantiate(tile, transform).GetComponent<Square>();
            s.Init(coord);
            s.CoordDelegate += ClickedCoord;
            s.transform.position = TwoDimensionalCoord(coord);
            board[coord] = s.gameObject;
        }
    }

    private void ClickedCoord(int coord) {
        if (othello.Move(coord)) {
            IEnumerator turn = Turn();
            StartCoroutine(turn);
        }
    }

    private IEnumerator Turn() {
        yield return new WaitForSeconds(.5f);
        othello.Minimax();
    }

    private void UpdateBoard() {
        ulong whiteBoard = othello.GetWhitesBoard();
        ulong blackBoard = othello.GetBlacksBoard();
        int lastPlacedCoord = othello.LastPlacedCoord();

        if (lastPlacedCoord < 0) return;

        for (int coord = 0; coord < 64; coord++) {
            if (coord == lastPlacedCoord) continue;

            if (!((prevBlackBoard & (1UL << coord)) != 0) && (blackBoard & (1UL << coord)) != 0)
                InstantiateTile(coord, Black);
            if (!((prevWhiteBoard & (1UL << coord)) != 0) && (whiteBoard & (1UL << coord)) != 0)
                InstantiateTile(coord, White);
        }
        if ((blackBoard & (1UL << lastPlacedCoord)) != 0) {
            InstantiateTile(lastPlacedCoord, LastWhite);
        }
        if ((whiteBoard & (1UL << lastPlacedCoord)) != 0) {
            InstantiateTile(lastPlacedCoord, LastBlack);
        }

        prevBlackBoard = blackBoard;
        prevWhiteBoard = whiteBoard;
        turnText.text = othello.GetTurn() == 1 ? "WHITE" : "BLACK";
        turnText.color = othello.GetTurn() == 1 ? Color.white : Color.black;
    }

    private void UpdateScore(int whiteScore, int blackScore) {
        scoreTextWhite.text = String.Format("{0}", whiteScore);
        scoreTextBlack.text = String.Format("{0}", blackScore);

    }

    private Vector2 TwoDimensionalCoord(int coord) {
        return new Vector2(coord % Width, coord / Width);
    }
}
