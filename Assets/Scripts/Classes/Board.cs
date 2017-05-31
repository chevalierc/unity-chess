using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simply a 2d cointainer

public class Board {
    public GameObject[] boardObjects;
    public int height;
    public int width;

    public void print() {
        for (int y = 0; y < height; y++) {
            string line = y + " ";
            for (int x = 0; x < width; x++) {
                if(this.get(new Position(x, y))) {
                    line += "x";
                }else {
                    line += " ";
                }
            }
            Debug.Log(line);
        }
    }

    public Board(int w, int h) {
        width = w;
        height = h;
        boardObjects = new GameObject[w*h];
        for(int i = 0; i < w*h; i++) {
            boardObjects[i] = null;
        }
    }

    public Board(Board oldBoard) {
        width = oldBoard.width;
        height = oldBoard.height;
        boardObjects = new GameObject[width * height];
        boardObjects = (GameObject[]) oldBoard.boardObjects.Clone();
    }

    public bool positionExists(Position position) {
        return position.x >= 0 && position.x < this.width && position.y >= 0 && position.y < this.height;
    }

    public bool positionIsFree(Position position) {
        if (positionExists(position)) {
            return this.get(position) == null;
        }
        return false;
    }

    public bool positionIsPlayer(Position position) {
        if (positionExists(position)) {
            if (this.get(position) != null) {
                return !this.get(position).GetComponent<Piece>().isAI;
            }
        }
        return false;
    }

    public bool isSomeoneInCheck(bool checkForAiInCheck) {
        //check if ai or player is in check
        Debug.Log("IsSomeoneInCheck?");
        return false;
        Move[] moves = this.getPossibleMovesFor(!checkForAiInCheck);
        Debug.Log(moves.Length);
        for(int i = 0; i < moves.Length; i++) {
            Position destination = moves[i].end;
            if (!this.positionIsFree(destination)) {
                if( (checkForAiInCheck && this.positionIsPlayer(destination)) || (!checkForAiInCheck && this.positionIsAI(destination)) ){
                    Debug.Log(this.get(destination).name);
                    if( this.get(destination).name == "King") {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Move[] getPossibleMovesFor( bool isAiTurn) {
        Board currentBoard = this;
        List<Move> possibleMoves = new List<Move>();
        for (var x = 0; x < currentBoard.width; x++) {
            for (var y = 0; y < currentBoard.height; y++) {
                Position position = new Position(x, y);
                if (currentBoard.get(position)) {
                    Piece currentPiece = currentBoard.get(position).GetComponent<Piece>();
                    if ( (currentPiece.isAI && isAiTurn) || (!currentPiece.isAI && !isAiTurn) ) {
                        possibleMoves.AddRange(currentPiece.getMovesFromLocationOnBoard(position, currentBoard));
                    }
                }
            }
        }

        return possibleMoves.ToArray();
    }

    public int getBoardScore() {
        //returns sum of players pieces values - ai piece values. Higher score is better for player
        Board currentBoard = this;
        int score = 0;
        Position boardCenter = new Position(currentBoard.width / 2, currentBoard.height / 2);
        GameObject[] board = currentBoard.asArray();
        for (var i = 0; i < board.Length; i++) {
            if (board[i]) {
                Piece piece = board[i].GetComponent<Piece>();
                if (piece.isAI) {
                    score -= piece.value * 100;
                    score += board[i].GetComponent<Piece>().position.manhattanDistanceTo(boardCenter);
                } else if (!piece.isAI) {
                    score += piece.value * 100;
                    score -= board[i].GetComponent<Piece>().position.manhattanDistanceTo(boardCenter);
                }
            }
        }
        return score;
    }

    public bool positionIsAI(Position position) {
        if (positionExists(position)) {
            if(this.get(position) != null) {
                return this.get(position).GetComponent<Piece>().isAI;
            }
        }
        return false;
    }

    public GameObject[] asArray() {
        return boardObjects;
    }

    public Board move(Move move) {
        return this.move(move.start, move.end);
    }

    public Board move(Position currentPosition, Position newPosition) {
        GameObject currentPiece = get(currentPosition);
        set(newPosition, currentPiece);
        set(currentPosition, null);
        return this;
    }

    public GameObject get (Position pos) {
        if (pos == null) return null; 
        int i = pos.y * this.width + pos.x;
        return (GameObject)this.boardObjects[i];
    }

    public Board set(Position pos, GameObject piece) {
        int i = pos.y * this.width + pos.x;
        this.boardObjects[i] = piece;
        return this;
    }

    public Board getBoardAfterMove(Move move) {
        Board board = new Board(this);
        board.move(move);
        return board;
    }

}
    