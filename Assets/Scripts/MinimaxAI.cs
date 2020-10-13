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
    private bool searchComplete = false;
    private int searchDepth = 5; //Increases search time exponentially

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        var currentBoardPosition = new BoardState(searchDepth,_bm.board.cellGrid);

        float test = Minimax(searchDepth, currentBoardPosition, false, 
            -Mathf.Infinity,Mathf.Infinity);
        Debug.Log(test);

        Cell[,] cellGrid = _bm.board.cellGrid;

        bool hasMoved = false;
        
        Debug.Log(_bestPossibleMove == null);
        if (_bestPossibleMove != null)
        {
            cellGrid = _bestPossibleMove.CellGrid;
            foreach (var cell in cellGrid)
            {
                cell.Refresh();
            }
        }
        GameManager.Instance.IsWhiteTurn = true;
        
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardState boardState, bool isMaximisingPlayer, float alpha, float beta)
    {
        if (depth == 0)
        {
            _bestPossibleMove = boardState.FindFirstMove();
            searchComplete = true;
            return boardState.StaticEvaluation;
        }
        if (isMaximisingPlayer)
        {
            float maxEval = -Mathf.Infinity;
            if (boardState.childrenStates != null)
                foreach (var nextMove in boardState.childrenStates)
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
            if (boardState.childrenStates != null)
                foreach (var nextMove in boardState.childrenStates)
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
}