using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class BoardState
{
    private Cell[,] _cellGrid = new Cell[8,8];

    public Cell[,] CellGrid => _cellGrid;

    private BoardState _parentState;
    public List<BoardState> childrenStates;
    private int _depth, _maxDepth;

    private BoardState(int maxDepth,int depth, BoardState parentState, 
        Cell[,] cellGrid, Piece pieceToMove, Cell cellToMove)
    {
        _maxDepth = maxDepth;
        _depth = depth;
        _parentState = parentState;
        _cellGrid = cellGrid;
        cellGrid[pieceToMove.cell.cellPos.x,pieceToMove.cell.cellPos.y].currentPiece.Place(cellToMove);
        childrenStates = new List<BoardState>();
        if (_depth <= _maxDepth)
        {
            childrenStates = CreateChildrenStates();
        }
    }

    public BoardState(int maxDepth, Cell[,] cellGrid)
    {
        _maxDepth = maxDepth;
        _depth = maxDepth;
        _parentState = this;
        _cellGrid = cellGrid;
        childrenStates = new List<BoardState>();
        if (_depth <= _maxDepth)
        {
            childrenStates = CreateChildrenStates();
        }
    }
    
    public BoardState FindFirstMove()
    {
        BoardState parentState = this;
        while (parentState._depth != 1)
        {
            parentState = _parentState.FindFirstMove();
        }
        return parentState;
    }
    
    public List<BoardState> CreateChildrenStates()
    {
        List<BoardState> localChildrenStates = new List<BoardState>();
        if (_depth < _maxDepth)
        {
            foreach (var cell in _cellGrid)
            {
                if (cell.currentPiece != null)
                {
                    Piece piece = cell.currentPiece;
                    piece.FindValidMoves(false);
                    foreach (var availableCell in piece.availableCells)
                    {
                        localChildrenStates.Add(new BoardState(_maxDepth,_depth+1,this,
                            _cellGrid,piece,availableCell));
                    }
                    piece.ClearCells();
                }
            }
        }
        return localChildrenStates;
    }

    public int StaticEvaluation
    {
        get
        {
            return CalculateStaticEvaluation();
        }
    }

    private int CalculateStaticEvaluation()
    {
        int score = 0;
        foreach (var cell in _cellGrid)
        {
            if (cell.currentPiece != null)
            {
                if (cell.currentPiece.gameObject.activeSelf)
                {
                    if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White)))
                    {
                        score += BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                    else
                    {
                        score -= BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                }
            }
        }
        return score;
    }
    
}
