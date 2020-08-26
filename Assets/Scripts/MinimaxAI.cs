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

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        BoardPosition currentBoardPosition = new BoardPosition();
        float test = Minimax(1, currentBoardPosition, false);
        Debug.Log(test);

        _bestPossibleMove.pieceCurrentCell.currentPiece.Place(_bestPossibleMove.futureCell);
        GameManager.Instance.IsWhiteTurn = true;
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardPosition boardPosition, bool isMaximisingPlayer)
    {
        Thread.Sleep(1000);
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
                float eval = Minimax(depth - 1, nextMove, false);
                maxEval = Math.Max(maxEval, eval);
            }

            return maxEval;
        }
        else
        {
            float minEval = Mathf.Infinity;
            foreach (var nextMove in boardPosition.nextMoves)
            {
                float eval = Minimax(depth - 1, nextMove, true);
                minEval = Math.Min(minEval, eval);
            }

            return minEval;
        }
    }
}