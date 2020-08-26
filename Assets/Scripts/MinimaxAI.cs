using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static Colours.ColourNames;

public class MinimaxAI : MonoBehaviour
{
    private BoardManager _bm;

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        BoardPosition currentBoardPosition = new BoardPosition();
        float maxScore = Minimax(3, currentBoardPosition, false);
        Debug.Log(maxScore);
    }

    //White is maximising, black is minimising
    private float Minimax(int depth, BoardPosition boardPosition, bool isMaximisingPlayer)
    {
        if (depth == 0 || boardPosition.isGameOver)
        {
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
