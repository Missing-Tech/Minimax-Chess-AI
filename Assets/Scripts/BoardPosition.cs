using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro.EditorUtilities;
using UnityEngine;

//Custom class to store a possible move and the piece it's derived from
public class BoardPosition
{
    public Cell[,] cellGrid = new Cell[8,8];
    public bool isWhiteMove;
    public List<BoardPosition> nextMoves;
    public int staticEvaluation;
    public BoardManager bm;
    public Cell pieceCurrentCell;
    public Cell futureCell;
    private bool nextMovesCalculated = false;

    public List<BoardPosition> NextMoves
    {
        get
        {
            if (nextMovesCalculated)
            {
                return nextMoves;
            }
            nextMoves = CalculateNextMoves();
            nextMovesCalculated = true;
            return nextMoves;
        }
    }

    public int StaticEvaluation
    {
        get
        {
            staticEvaluation = CalculateScore();
            return staticEvaluation;
        }
    }

    public bool isGameOver = false;

    //Constructor for future positions
    private BoardPosition(Piece piece, Cell cellToMoveTo, bool isGameOver)
    {
        cellGrid = Board.Instance.cellGrid;
        isWhiteMove = GameManager.Instance.IsWhiteTurn;
        this.isGameOver = isGameOver;
        
        bm = BoardManager.Instance;
        
        /*Manually change piece variable (because it's a hypothetical cell position and
        the piece's shouldn't move on the game screen)*/
        pieceCurrentCell = cellGrid[piece.cell.cellPos.x, piece.cell.cellPos.y];
        pieceCurrentCell.currentPiece = null;
        futureCell = cellGrid[cellToMoveTo.cellPos.x, cellToMoveTo.cellPos.y];
        futureCell.currentPiece = piece;
    }

    //Constructor for current position
    public BoardPosition()
    {
        cellGrid = Board.Instance.cellGrid;
        isWhiteMove = GameManager.Instance.IsWhiteTurn;
        isGameOver = GameManager.Instance.gameWon;
        
        bm = BoardManager.Instance;
    }

    private List<BoardPosition> CalculateNextMoves()
    {
        List<BoardPosition> moves = new List<BoardPosition>();

        foreach (var cell in cellGrid)
        {
            Piece piece = cell.currentPiece;
            
            if (piece != null)
            {
                piece.FindValidMoves(false);
                List<Cell> pieceMoves = piece.availableCells;

                bool checkmate = false;

                foreach (var move in pieceMoves)
                {
                    if (move.currentPiece != null)
                    {
                        if (move.currentPiece.GetType() == typeof(King))
                        {
                            King king = (King) move.currentPiece;
                            checkmate = king.isCheckmate;
                        }
                    }

                    moves.Add(new BoardPosition(piece,move,checkmate));
                }
            }
        }
        return moves;
    }

    int CalculateScore()
    {
        int score = 0;

        foreach (var cell in cellGrid)
        {
            if (cell.currentPiece != null)
            {
                if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White)))
                {
                    score += bm.pieceEvaluation[cell.currentPiece.GetType()];
                }
                else
                {
                    score -= bm.pieceEvaluation[cell.currentPiece.GetType()];
                }
            }
        }
        return score;
    }
    
}
