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

    public void move(Move move) {
        this.move(move.start, move.end);
    }

    public void move(Position currentPosition, Position newPosition) {
        GameObject currentPiece = get(currentPosition);
        set(newPosition, currentPiece);
        set(currentPosition, null);
    }

    public GameObject get (Position pos) {
        if (pos == null) return null; 
        int i = pos.y * this.width + pos.x;
        return (GameObject)this.boardObjects[i];
    }

    public void set(Position pos, GameObject piece) {
        int i = pos.y * this.width + pos.x;
        this.boardObjects[i] = piece;
    }

    public Board getBoardAfterMove(Move move) {
        Board board = new Board(this);
        board.move(move);
        return board;
    }

}
    