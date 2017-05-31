using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    override
    public Move[] getMovesFromLocationOnBoard(Position position, Board board) {
        List<Move> moves = new List<Move>();
        
        int dy = 0;
        if (!this.isAI) {
            dy = 1;
        } else {
            dy = -1;
        }

        Debug.Log("yo");

        Position newPosition = new Position(position.x, position.y + dy);
        if (board.positionIsFree(newPosition)) {
            Move currentMove = new Move(position, newPosition);
            Board boardAfterMove = board.getBoardAfterMove(currentMove);
            if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                moves.Add(currentMove);
            }
            //double jump
            newPosition = new Position(position.x, position.y + dy*2);
            if (board.positionIsFree(newPosition) && position == startingPosition) {
                currentMove = new Move(position, newPosition);
                boardAfterMove = board.getBoardAfterMove(currentMove);
                if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                    moves.Add(currentMove);
                }
            }
        }
        //attackleft
        newPosition = new Position(position.x - 1, position.y + dy);
        if (board.positionIsAI(newPosition)) {
            Move currentMove = new Move(position, newPosition);
            Board boardAfterMove = board.getBoardAfterMove(currentMove);
            if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                moves.Add(currentMove);
            }
        }
        //attackright
        newPosition = new Position(position.x + 1, position.y + dy);
        if (board.positionIsAI(newPosition)) {
            Move currentMove = new Move(position, newPosition);
            Board boardAfterMove = board.getBoardAfterMove(currentMove);
            if (!boardAfterMove.isSomeoneInCheck(this.isAI)) {
                moves.Add(currentMove);
            }
        }

        return moves.ToArray();
    }
}
