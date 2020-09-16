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
    private BoardPosition _bestPossibleMove;
    private int searchDepth = 3; //Increases search time exponentially

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        var currentBoardPosition = new BoardPosition(searchDepth);
        //Waits until the board has calculated possible moves
        while (!currentBoardPosition.finishedCalculating)
        {
            Thread.Sleep(50);
        }
        float test = Minimax(searchDepth, currentBoardPosition, false, 
            -Mathf.Infinity,Mathf.Infinity);
        Debug.Log(test);

        Cell[,] cellGrid = _bm.board.cellGrid;
        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Cell cell = cellGrid[x,y];
                Cell newCell = _bestPossibleMove.cellGrid[cell.cellPos.x, cell.cellPos.y];
                cell.currentPiece = newCell.currentPiece;
                cell.Refresh();
            }
            
        }
        GameManager.Instance.IsWhiteTurn = true;
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardPosition boardPosition, bool isMaximisingPlayer, float alpha, float beta)
    {
        if (depth == 0 || boardPosition.isGameOver)
        {
            _bestPossibleMove = boardPosition.firstBoardPos;
            return boardPosition.staticEvaluation;
        }
        if (isMaximisingPlayer)
        {
            float maxEval = -Mathf.Infinity;
            foreach (var nextMove in boardPosition.nextMoves)
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
            foreach (var nextMove in boardPosition.nextMoves)
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