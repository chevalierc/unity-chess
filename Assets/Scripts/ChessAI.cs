using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessAI  {

    private class Node {
        public Move move;
        public int value;

        public Node(Move move, int value) {
            this.move = move;
            this.value = value;
        }
    }
	
    public static Move getBestMove(Board currentBoard) {
        int maxDepth = 2;
        int alpha = int.MaxValue;
        int beta = int.MinValue;

        Board board = new Board(currentBoard);

        Node bestNode = value(board, true, alpha, beta, 0, maxDepth);

        return bestNode.move;
    }

    private static Node value(Board currentBoard, bool aiTurn, int alpha, int beta, int curDepth, int maxDepth) {
        if (aiTurn) {
            curDepth++;
        }

        if(curDepth == maxDepth) {
            return new Node(null, getBoardScore(currentBoard));
        }

        return minMax(currentBoard, aiTurn, alpha, beta, curDepth, maxDepth);
    }

    private static Node minMax(Board currentBoard, bool isAiTurn, int alpha, int beta, int curDepth, int maxDepth) {
        Node minOrMaxNode = new Node(null, 0);

        Move[] possibleMoves = getpossibleMovesFor(currentBoard, isAiTurn);
        Debug.Log(possibleMoves.Length);
        for(var i = 0; i < possibleMoves.Length; i++) {
            Move move = possibleMoves[i];
            Board newBoard = currentBoard.getBoardAfterMove(move);

            int boardValue = value(newBoard, !isAiTurn, alpha, beta, curDepth, maxDepth).value;

            //set min max node
            if (minOrMaxNode.move == null) {
                minOrMaxNode = new Node(move, boardValue);
            } else {
                //player turn. Maximize score
                if (!isAiTurn) {
                    if(boardValue > minOrMaxNode.value) {
                        minOrMaxNode = new Node(move, boardValue);
                    }
                //ai turn. Minimize score
                }else {
                    if (boardValue < minOrMaxNode.value) {
                        minOrMaxNode = new Node(move, boardValue);
                    }
                }
            }

            //set alpha/beta
            
            /*
            if(isAiTurn) {
                if(boardValue > beta) {
                    return new Node(move, boardValue);
                }
                if(alpha < boardValue) {
                    alpha = boardValue;
                }
            }else {
                if (boardValue < alpha) {
                    return new Node(move, boardValue);
                }
                if (beta > boardValue) {
                    beta = boardValue;
                }
            }
            */
            
            
        }

        return minOrMaxNode;
    }

    public static Move[] getpossibleMovesFor(Board currentBoard, bool isAiTurn) {
        ArrayList possibleMoves = new ArrayList();
        for (var x = 0; x < currentBoard.width; x++) {
            for (var y = 0; y < currentBoard.height; y++) {
                Position position = new Position(x, y);
                if (currentBoard.get(position)) {
                    Piece currentPiece = currentBoard.get(position).GetComponent<Piece>();
                    if (currentPiece.isAI && isAiTurn) {
                        possibleMoves.AddRange(currentPiece.getMovesFromLocationOnBoard(position, currentBoard));
                    }else if (!currentPiece.isAI && !isAiTurn) {
                        possibleMoves.AddRange(currentPiece.getMovesFromLocationOnBoard(position, currentBoard));
                    }
                }
            }
        }

        return (Move[]) possibleMoves.ToArray(typeof(Move));
    }

    public static int getBoardScore(Board currentBoard) {
        //returns sum of players pieces values - ai piece values. Higher score is better for player
        int score = 0;
        GameObject[] board = currentBoard.asArray();
        for (var i = 0; i < board.Length; i++) {
            if (board[i]) {
                Piece piece = board[i].GetComponent<Piece>();
                if (piece.isAI) {
                    score -= piece.value;
                } else if (!piece.isAI) {
                    score += piece.value;
                }
            }
        }
        return score;
    }



}
