﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.PlayerLoop;

//Custom class to store a possible move and the piece it's derived from
public class BoardPosition
{
    public Cell[,] cellGrid = new Cell[8,8];
    public bool isWhiteMove;
    public BoardPosition parentNode;
    public List<BoardPosition> childNodes;
    public bool finishedCalculating;
    public int staticEvaluation;
    public BoardManager bm;
    //public Cell pieceCurrentCell;
    //public Cell futureCell;

    public bool isGameOver = false;

    //Constructor for future positions
    private BoardPosition(Piece piece, Cell cellToMoveTo, bool isGameOver, int depth)
    {
        if (parentNode == null)
        {
            parentNode = this;
        }
        cellGrid = parentNode.cellGrid;
        isWhiteMove = GameManager.Instance.IsWhiteTurn;
        this.isGameOver = isGameOver;
        bm = BoardManager.Instance;

        if (depth > 0)
        {
            childNodes = CalculateNextMoves(depth);
            staticEvaluation = CalculateScore();
        }
        
        /*Manually change piece variable (because it's a hypothetical cell position and
        the piece's shouldn't move on the game screen)*/
        /*pieceCurrentCell = cellGrid[piece.cell.cellPos.x, piece.cell.cellPos.y];
        pieceCurrentCell.currentPiece = null;
        futureCell = cellGrid[cellToMoveTo.cellPos.x, cellToMoveTo.cellPos.y];
        futureCell.currentPiece = piece;*/
    }

    //Constructor for current position
    public BoardPosition(int depth)
    {
        cellGrid = Board.Instance.cellGrid;
        isWhiteMove = GameManager.Instance.IsWhiteTurn;
        isGameOver = GameManager.Instance.gameWon;
        bm = BoardManager.Instance;
        parentNode = this;
        
        if (depth > 0)
        {
            childNodes = CalculateNextMoves(depth);
            staticEvaluation = CalculateScore();
        }
    }

    private List<BoardPosition> CalculateNextMoves(int depth)
    {
        List<BoardPosition> moves = new List<BoardPosition>();

        foreach (var cell in cellGrid)
        {
            Piece piece = cell.currentPiece;
            
            if (piece != null)
            {
                piece.FindValidMoves(false);

                var availableCells = piece.availableCells;
                
                bool checkmate = false;

                foreach (var move in availableCells.ToArray())
                {
                    if (move.currentPiece != null)
                    {
                        if (move.currentPiece.GetType() == typeof(King))
                        {
                            King king = (King) move.currentPiece;
                            checkmate = king.isCheckmate;
                        }
                    }

                    BoardPosition nextMove = new BoardPosition(piece, move, checkmate, depth - 1);
                    nextMove.parentNode = parentNode;
                    moves.Add(nextMove);
                }
                //piece.ClearCells();
            }
        }
        finishedCalculating = true;
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
