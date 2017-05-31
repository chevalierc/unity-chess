using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for the grahics of the board. creating the board, moving pieces, send events to chessManager

public class BoardManager : MonoBehaviour {
    public int width;
    public int height;
    public bool isPlayerTurn;
    public Position selectedTile;
    public Position[] possibleMovePositions;

    public Board tiles;
    public Board pieces;
    public ChessManager chessManager;

    public GameObject tile;
    public GameObject[] PlayerStartingPieces;
    public GameObject[] AiStartingPieces;

    private float tileWidth;
    private Vector3 boardTopLeft;
    private Vector3 screenCenter;

    // Use this for initialization
    void Start() {
        isPlayerTurn = true;
        getScreenInfo();
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
                } else if (!pieces.positionIsFree(clickPosition) && !pieces.get(clickPosition).GetComponent<Piece>().isAI) {
                    resetTiles();
                    selectTile(clickPosition);
                    getOptions(clickPosition);
                    setOptions();
                } else if (new ArrayList(possibleMovePositions).Contains(clickPosition)) {
                    movePiece(selectedTile, clickPosition, false);
                    doAiTurn();
                    resetTiles();
                }
                
            }
        }

    }

    private void getOptions(Position startPosition) {
        GameObject pieceToMove = pieces.get(startPosition);
        possibleMovePositions = pieceToMove.GetComponent<Piece>().getMovesDestinationFromLocationOnBoard(startPosition, pieces);
    }

    //piece gui

    public void movePiece(Position startPosition, Position endPosition, bool isAi) {
        if (pieces.get(endPosition)) {
            Destroy(pieces.get(endPosition));
        }
        GameObject pieceToMove = pieces.get(startPosition);
        Vector3 endVector = new Vector3(endPosition.x * tileWidth, -endPosition.y * tileWidth, 0f);
        endVector += boardTopLeft;
        pieceToMove.GetComponent<Piece>().move(endVector);
        pieceToMove.GetComponent<Piece>().position = new Position(endPosition);
        pieces.move(startPosition, endPosition);
    }

    //tile gui

    private void setOptions() {
        Debug.Log(possibleMovePositions.Length);
        for(var i = 0; i < possibleMovePositions.Length; i++) {
            Debug.Log( possibleMovePositions[i].x + "," + possibleMovePositions[i].y );
            tiles.get( possibleMovePositions[i] ).GetComponent<Tile>().setAsOption();
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

    private void getScreenInfo() {
        tileWidth = (float) tile.GetComponent<Renderer>().bounds.size.x;
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        boardTopLeft = screenCenter + new Vector3(-this.width / 2 * tileWidth, (float)((this.height / 2) -.5) * tileWidth);
    }

    private void initPieces() {
        pieces = new Board(width, height);
        List<GameObject> piecesToPlace;

        //loop through player and ai
        bool isAi = true;
        for(int p = 0; p < 2; p++) {
            if(p == 1) {
                isAi = false;
            }

            if (isAi) {
                piecesToPlace = new List<GameObject>(AiStartingPieces);
            } else {
                piecesToPlace = new List<GameObject>(PlayerStartingPieces);
            }

            //sort by the value of the pieces
            piecesToPlace.Sort(
                (x, y) => {
                    int result = y.GetComponent<Piece>().value.CompareTo(x.GetComponent<Piece>().value);
                    return result != 0 ? result : y.name.CompareTo(x.name);
                });

            //instantiate each piece
            for (var i = 0; i < piecesToPlace.Count; i++) {
                Position newPosition = indexToPosition(i, width, height, isAi);
                Vector3 location = new Vector3(newPosition.x * tileWidth, - newPosition.y * tileWidth, 0f);
                location += boardTopLeft;
                GameObject newPiece = Instantiate(piecesToPlace[i], location, Quaternion.identity) as GameObject;
                newPiece.GetComponent<Piece>().setAsAi(isAi);
                newPiece.GetComponent<Piece>().startingPosition = new Position(newPosition);
                newPiece.GetComponent<Piece>().position = new Position(newPosition);
                Position position = new Position(newPosition);
                pieces.set(position, newPiece);
            }
        }
    }

    private Position indexToPosition(int i, int w, int h, bool isTop) {
        int x = 0;
        int y = Mathf.FloorToInt(i / w);
        int rowI = i % w;
        if (rowI % 2 == 0) {
            x = w / 2 + (rowI / 2);
        } else {
            x = w / 2 - (rowI / 2) - 1;
        }
        if (isTop) {
            y = h - y - 1;
        }
        return new Position(x, y);
    }

    private void initBoardTiles() {
        tiles = new Board(width, height);
        
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

                Vector3 location = new Vector3(x * tileWidth, -y * tileWidth, 0f);
                location += boardTopLeft;

                GameObject instance = Instantiate(newTile, location, Quaternion.identity) as GameObject;
                tiles.set(new Position(x, y), instance);
            }
        }
    }
	
}
