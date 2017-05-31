using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {

    override
    public Move[] getMovesFromLocationOnBoard(Position position, Board board) {
        List<Move> moves = new List<Move>();

        for (var d = 0; d < 4; d++) {
            int dx = 0;
            int dy = 0;
            if (d == 0) {
                dx = 0;
                dy = 1;
            } else if (d == 1) {
                dx = 0;
                dy = -1;
            } else if (d == 2) {
                dx = -1;
                dy = 0;
            } else if (d == 3) {
                dx = 1;
                dy = 0;
            }
            Position newPosition = new Position(position);
            do {
                newPosition = new Position(newPosition);
                newPosition.x += dx;
                newPosition.y += dy;
                if (board.positionIsFree(newPosition)) {
                    Move currentMove = new Move(position, newPosition);
                    Board boardAfterMove = board.getBoardAfterMove(currentMove);
                    if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                        moves.Add(currentMove);
                    }
                } else {
                    if ((this.isAI && board.positionIsPlayer(newPosition)) || (!this.isAI && board.positionIsAI(newPosition))) {
                        Move currentMove = new Move(position, newPosition);
                        Board boardAfterMove = board.getBoardAfterMove(currentMove);
                        if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                            moves.Add(currentMove);
                        }
                    }
                    break;
                }
            } while (true);
        }


        return moves.ToArray();
    }

}
