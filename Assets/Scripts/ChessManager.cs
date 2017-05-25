using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for gameplay managment internally and controlling everything else

public class ChessManager: MonoBehaviour {
    public BoardManager boardManager;
    public bool isPlayerTurn = true;
    public int turnCount;


    private void Awake() {
        boardManager = GetComponent<BoardManager>();
        boardManager.setChessManager(this);
    }

    public void endPlayerTurn() {
        isPlayerTurn = false;
        Board pieces = boardManager.pieces;
        Move bestMoveForAi = ChessAI.getBestMove(pieces);
        Debug.Log(bestMoveForAi.start.x + "," + bestMoveForAi.start.y);
        //boardManager.movePiece(bestMoveForAi.start, bestMoveForAi.end);
        isPlayerTurn = true;
    }

}
