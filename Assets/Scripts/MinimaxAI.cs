using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static Colours.ColourNames;

public class MinimaxAI : MonoBehaviour
{
    private BoardManager _bm;
    private BoardState _bestPossibleMove;
    private int searchDepth = 4; //Increases search time exponentially

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        var currentBoardPosition = new BoardState(searchDepth,_bm.board.cellGrid,searchDepth);

        //Calls a recursive depth search on a tree of possible board states
        //The 'alpha' and 'beta' values are used for alpha-beta pruning which optimises the search
        Minimax(searchDepth, currentBoardPosition, false, 
            -Mathf.Infinity,Mathf.Infinity);

        //Local reference to the cell grid
        Cell[,] cellGrid = new Cell[8,8];
        cellGrid = _bm.board.cellGrid;
        
        if (_bestPossibleMove != null)
        {
            Piece pieceToMove = _bm.blackPieces.Find(x => x == _bestPossibleMove.pieceToMove);
            Cell cellToMove = cellGrid[_bestPossibleMove.cellToMove.cellPos.x, _bestPossibleMove.cellToMove.cellPos.y];
            pieceToMove.Place(cellToMove);
            Debug.Log(cellToMove.cellPos);
        }

        _bestPossibleMove = null;
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardState boardState, bool isMaximisingPlayer, float alpha, float beta)
    {
        if (depth == 0)
        {
            _bestPossibleMove = boardState.ParentState;
            return CalculateStaticEvaluation(boardState.CellGrid);
        }
        
        if (isMaximisingPlayer)
        {
            float maxEval = -Mathf.Infinity;
            foreach (var nextMove in boardState.ChildrenStates)
                {
                    //Recursively calls the function to the layer above in the tree
                    float eval = Minimax(depth - 1, nextMove, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    //Alpha beta pruning
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }
            return maxEval;
        }
        else
        {
            float minEval = Mathf.Infinity;
            foreach (var nextMove in boardState.ChildrenStates)
                {
                    //Recursively calls the function to the layer above in the tree
                    float eval = Minimax(depth - 1, nextMove, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    //Alpha beta pruning
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }

            return minEval;
        }
    }
    
    private int CalculateStaticEvaluation(Cell[,] cellGrid)
    {
        int score = 0;
        foreach (var cell in cellGrid)
        {
            if (cell.currentPiece != null)
            {
                if (cell.currentPiece.gameObject.activeSelf)
                {
                    if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(White)))
                    {
                        score += BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                    else if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(Black)))
                    {
                        score -= BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                }
            }
        }

        return score;
    }
}