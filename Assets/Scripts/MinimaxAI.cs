using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class MinimaxAI : MonoBehaviour
{
    private List<Cell> _possibleMoves;
    private BoardManager _bm;

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
    }

    public void DoTurn()
    {
        _possibleMoves = FindPossibleMoves();
    }
    
    List<Cell> FindPossibleMoves()
    {
        List<Cell> possibleMoves = new List<Cell>();
        foreach (var piece in _bm.blackPieces)
        {
            piece.FindValidMoves(false);
            possibleMoves.Concat(piece.availableCells);
        }
        return possibleMoves;
    }
    
}
