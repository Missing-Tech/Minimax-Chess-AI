using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static Colours.ColourNames;

public class BoardState
{
    private Cell[,] _cellGrid = new Cell[8, 8];

    public Cell[,] CellGrid => _cellGrid;

    private BoardState _parentState;

    public BoardState ParentState => _parentState;

    public List<BoardState> childrenStates;

    public Piece pieceToMove;
    public Cell cellToMove;

    public List<BoardState> ChildrenStates
    {
        get
        {
            if (childrenStates == null || childrenStates.Count == 0)
            {
                return CreateChildrenStates();
            }

            return childrenStates;
        }
    }

    public int _depth, _maxDepth;

    private BoardState(int maxDepth, int depth, BoardState parentState,
        Cell[,] cellGrid, Piece pieceToMove, Cell cellToMove)
    {
        _maxDepth = maxDepth;
        _depth = depth;
        _cellGrid = cellGrid;
        this.pieceToMove = pieceToMove;
        this.cellToMove = cellToMove;
        _parentState = parentState;
        
        /*if (depth == 1)
        {
            _parentState.pieceToMove = pieceToMove;
            _parentState.cellToMove = cellToMove;
        }*/
    }

    public BoardState(int maxDepth, Cell[,] cellGrid, int depth)
    {
        _maxDepth = maxDepth;
        _depth = depth;
        _parentState = this;
        pieceToMove = null;
        cellToMove = null;
        _cellGrid = cellGrid;
    }

    public List<BoardState> CreateChildrenStates()
    {
        List<BoardState> localChildrenStates = new List<BoardState>();

        foreach (var cell in _cellGrid)
        {
            Piece piece = cell.currentPiece;
            if (piece != null)
            {
                bool isAITurn = _depth % 2 != 0; //Returns false if it's the AI's turn
                                                 //AI turns are odd depth values
                bool canMoveThePiece = (piece.PieceColor.Equals(Colours.ColourValue(White)) && !isAITurn) ||
                                       (piece.PieceColor.Equals(Colours.ColourValue(Black)) && isAITurn);
                if (!canMoveThePiece)
                {
                    piece.FindValidMoves(false);
                    Cell[] availableCells = piece.availableCells.ToArray();
                    foreach (var availableCell in availableCells)
                    {
                        Cell[,] newCellGrid = new Cell[8,8];
                        newCellGrid = _cellGrid;
                        /*Cell newCellPos = newCellGrid[availableCell.cellPos.x, availableCell.cellPos.y];

                        if (newCellPos.CheckIfOtherTeam(piece.PieceColor))
                        {
                            newCellPos.currentPiece.gameObject.SetActive(false);
                        }*/

                        BoardState childBoardState = new BoardState(_maxDepth, _depth - 1, _parentState,
                            newCellGrid, piece, availableCell);
                        localChildrenStates.Add(childBoardState);
                    }
                    piece.ClearCells();
                }
            }
        }

        return localChildrenStates;
    }
}