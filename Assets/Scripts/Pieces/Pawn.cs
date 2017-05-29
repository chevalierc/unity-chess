using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    override
    public Move[] getMovesFromLocationOnBoard(Position position, Board board) {
        ArrayList moves = new ArrayList();
        //player
        if (!this.isAI) {
            Position newPosition = new Position(position.x, position.y + 1);
            if (board.positionIsFree(newPosition)) {
                moves.Add(new Move(position, newPosition));
                //double jump
                newPosition = new Position(position.x, position.y + 2);
                if (board.positionIsFree(newPosition) && position.y == 0) {
                    moves.Add(new Move(position, newPosition));
                }
            }
            //attackleft
            newPosition = new Position(position.x - 1, position.y + 1);
            if (board.positionIsAI(newPosition)) {
                moves.Add(new Move(position, newPosition));
            }
            //attackright
            newPosition = new Position(position.x + 1, position.y + 1);
            if (board.positionIsAI(newPosition)) {
                moves.Add(new Move(position, newPosition));
            }
            //ai
        } else if (position.y >= 0) {
            //one ahead
            Position newPosition = new Position(position.x, position.y - 1);
            if (board.positionIsFree(newPosition)) {
                moves.Add(new Move(position, newPosition));
                //double jump
                newPosition = new Position(position.x, position.y - 2);
                if (board.positionIsFree(newPosition) && position.y == board.height - 1) {
                    moves.Add(new Move(position, newPosition));
                }
            }
            //attackleft
            newPosition = new Position(position.x - 1, position.y - 1);
            if (board.positionIsPlayer(newPosition)) {
                moves.Add(new Move(position, newPosition));
            }
            //attackright
            newPosition = new Position(position.x + 1, position.y - 1);
            if (board.positionIsPlayer(newPosition)) {
                moves.Add(new Move(position, newPosition));
            }
        }
        return (Move[]) moves.ToArray(typeof(Move)); //BOOM!
    }
}
