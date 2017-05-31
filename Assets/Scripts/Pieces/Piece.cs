using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract class for all pieces

public abstract class Piece : MonoBehaviour {
    public Position position;
    public Position startingPosition;
    public Sprite aiSprite;
    public Sprite playerSprite;
    public float moveTime = 0.1f;
    public bool isAI = false;
    public int value = 1;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    public void setAsAi(bool asAI) {
        if (asAI) {
            this.isAI = true;
            spriteRenderer.sprite = aiSprite;
        }else {
            this.isAI = false;
            spriteRenderer.sprite = playerSprite;
        }
    }

    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void move(Vector3 end) {
        StartCoroutine(SmoothMovement(end));
    }

    public IEnumerator SmoothMovement(Vector3 end) {
        float inverseMoveTime = 1f / moveTime;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

    }

    public abstract Move[] getMovesFromLocationOnBoard(Position position, Board board);

    public Position[] getMovesDestinationFromLocationOnBoard(Position position, Board board) {
        Move[] moves = getMovesFromLocationOnBoard(position, board);
        Position[] positions = new Position[moves.Length];
        for (var i = 0; i < moves.Length; i++) {
            positions[i] = moves[i].end;
        }
        return positions;
    }

    public Move[] removeAnyMovesThatPutPlayerInCheck(Move[] moves, Board board) {
        List<Move> possibleMoves = new List<Move>();
        for(int i = 0; i < moves.Length; i++) {
            Move currentMove = moves[i];
            Board boardAfterMove = board.getBoardAfterMove(currentMove);
            if (!boardAfterMove.isSomeoneInCheck(this.isAI)){
                possibleMoves.Add(currentMove);
            }
        }
        return possibleMoves.ToArray();
    }



}
