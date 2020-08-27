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
    private int searchDepth = 2;

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
        float test = Minimax(searchDepth, currentBoardPosition, false, -Mathf.Infinity,Mathf.Infinity);
        Debug.Log(test);
        
        //Piece pieceToMove = _bestPossibleMove.pieceCurrentCell.currentPiece;
        //_bm.blackPieces.Find(x => pieceToMove).Place(_bestPossibleMove.futureCell);
        GameManager.Instance.IsWhiteTurn = true;
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardPosition boardPosition, bool isMaximisingPlayer, float alpha, float beta)
    {
        if (depth == 0 || boardPosition.isGameOver)
        {
            _bestPossibleMove = boardPosition;
            return boardPosition.staticEvaluation;
        }
        if (isMaximisingPlayer)
        {
            float maxEval = -Mathf.Infinity;
            foreach (var nextMove in boardPosition.nextMoves)
            {
                float eval = Minimax(depth - 1, nextMove, false, alpha, beta);
                maxEval = Math.Max(maxEval, eval);
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
                float eval = Minimax(depth - 1, nextMove, true, alpha, beta);
                minEval = Math.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }

            return minEval;
        }
    }
}