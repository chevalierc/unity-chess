using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    override
    public Move[] getMovesFromLocationOnBoard(Position position, Board board) {
        ArrayList moves = new ArrayList();

        for (var d = 0; d < 8; d++) {
            int dx = 0;
            int dy = 0;
            if (d == 0) {
                dx = 2;
                dy = -1;
            } else if (d == 1) {
                dx = 2;
                dy = 1;
            } else if (d == 2) {
                dx = -2;
                dy = -1;
            } else if (d == 3) {
                dx = -2;
                dy = 1;
            } else if (d == 4) {
                dx = 1;
                dy = 2;
            } else if (d == 5) {
                dx = -1;
                dy = 2;
            } else if (d == 6) {
                dx = -1;
                dy = -2;
            } else if (d == 7) {
                dx = 1;
                dy = -2;
            }
            Position newPosition = new Position(position);
            newPosition.x += dx;
            newPosition.y += dy;
            if (board.positionIsFree(newPosition)) {
                moves.Add(new Move(position, newPosition));
            } else {
                if ((this.isAI && board.positionIsPlayer(newPosition)) || (!this.isAI && board.positionIsAI(newPosition))) {
                    moves.Add(new Move(position, newPosition));
                }
            }
        }


        return (Move[])moves.ToArray(typeof(Move)); //BOOM!
    }

}
