using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for the grahics of the board. creating the board, moving pieces, send events to chessManager

public class BoardManager : MonoBehaviour {
    public int width;
    public int height;
    public bool isPlayerTurn;
    public Position selectedTile;
    public Position[] poisitionTiles;

    public Board tiles;
    public Board pieces;
    public ChessManager chessManager;

    public GameObject tile;
    public GameObject pawn;

    private float tileWidth;

    // Use this for initialization
    void Start() {
        isPlayerTurn = true;
        initBoardTiles();
        initPieces();
    }

    public void setChessManager(ChessManager manager) {
        chessManager = manager;
    }

    //game managment

    IEnumerator simulateEnemyThinkingThenMovePiece() {
        yield return new WaitForSeconds(1f);
        decideAndMoveEnemyPiece();
    }

    private void decideAndMoveEnemyPiece() {
        Move bestMoveForAi = ChessAI.getBestMove(pieces);
        Debug.Log(bestMoveForAi.start.x + "," + bestMoveForAi.start.y + "to" + bestMoveForAi.end.x + "," + bestMoveForAi.end.y);
        movePiece(bestMoveForAi.start, bestMoveForAi.end, true);
    }


    private void doAiTurn() {
        StartCoroutine(simulateEnemyThinkingThenMovePiece());
    }

    //events

    public void onClick(int x, int y) {
        Position clickPosition = new Position(x, y);

        if (isPlayerTurn) {
            if (selectedTile == null) {
                if (!pieces.positionIsFree(clickPosition) && !pieces.get(clickPosition).GetComponent<Piece>().isAI) {
                    selectTile(clickPosition);
                    getOptions(clickPosition);
                    setOptions();
                }
            } else {
                if (clickPosition == selectedTile) {
                    resetTiles();
                } else {
                    if (new ArrayList(poisitionTiles).Contains(clickPosition)) {
                        movePiece(selectedTile, clickPosition, false);
                        doAiTurn();
                        resetTiles();
                    }
                }
            }
        }

    }

    private void getOptions(Position startPosition) {
        GameObject pieceToMove = pieces.get(startPosition);
        poisitionTiles = pieceToMove.GetComponent<Piece>().getMovesDestinationFromLocationOnBoard(startPosition, pieces);
    }

    //piece gui

    public void movePiece(Position startPosition, Position endPosition, bool isAi) {
        Debug.Log(startPosition.x + "," + startPosition.y + "to" + endPosition.x + "," + endPosition.y);
        if (pieces.get(endPosition)) {
            Destroy(pieces.get(endPosition));
        }

        GameObject pieceToMove = pieces.get(startPosition);
        pieceToMove.GetComponent<Piece>().move(endPosition);
        pieces.move(startPosition, endPosition);
    }

    //tile gui

    private void setOptions() {
        for(var i = 0; i < poisitionTiles.Length; i++) {
            Debug.Log(poisitionTiles[i].x + "," + poisitionTiles[i].y);
            tiles.get(poisitionTiles[i]).GetComponent<Tile>().setAsOption();
        }
    }
    
    private void resetTiles() {
        for(var i = 0; i < tiles.boardObjects.Length; i++) {
            tiles.asArray()[i].GetComponent<Tile>().setUnSelected();
        }
        selectedTile = null;
    }

    private void selectTile(Position tilePosition) {
        selectedTile = tilePosition;
        tiles.get(tilePosition).GetComponent<Tile>().setSelected();
    }

    //init functions

    private void initPieces() {
        pieces = new Board(width, height);
        for(var i = 0; i < width; i++) {
            GameObject newPiece = Instantiate(pawn, new Vector3(i * tileWidth, 0 * tileWidth, 0f), Quaternion.identity) as GameObject;
            newPiece.GetComponent<Piece>().setAsAi(false);
            Position position = new Position(i, 0);
            pieces.set(position, newPiece);
        }
        for (var i = 0; i < width; i++) {
            GameObject newPiece = Instantiate(pawn, new Vector3(i * tileWidth, (height-1) * tileWidth, 0f), Quaternion.identity) as GameObject;
            newPiece.GetComponent<Piece>().setAsAi(true);
            Position position = new Position(i, height-1);
            pieces.set(position, newPiece);
        }
    }

    private void initBoardTiles() {
        tiles = new Board(width, height);
        tileWidth = (float) tile.GetComponent<Renderer>().bounds.size.x;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject newTile = tile;
                if ( x % 2 == 0) {
                    if( y % 2 == 0) {
                        newTile.GetComponent<Tile>().isLight = true;
                    } else {
                        newTile.GetComponent<Tile>().isLight = false;
                    }
                }else{
                    if (y % 2 == 0) {
                        newTile.GetComponent<Tile>().isLight = false;
                    } else {
                        newTile.GetComponent<Tile>().isLight = true;
                    }
                }
                newTile.GetComponent<Tile>().x = x;
                newTile.GetComponent<Tile>().y = y;
                newTile.GetComponent<Tile>().boardManager = this;

                GameObject instance = Instantiate(newTile, new Vector3(x*tileWidth, y*tileWidth, 0f), Quaternion.identity) as GameObject;
                tiles.set(new Position(x, y), instance);
            }
        }
    }
	
}
