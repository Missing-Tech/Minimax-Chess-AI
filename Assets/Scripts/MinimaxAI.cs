using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class MinimaxAI : MonoBehaviour
{
    private List<Cell> _possibleMoves;
    private List<Piece> _blackPieces;
    private BoardManager _bm;

    private Dictionary<Piece, int> pieceEvaluation;
    
    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
        _blackPieces = _bm.blackPieces;
    }

    public void DoTurn()
    {
        _possibleMoves = FindPossibleMoves();
    }

    /*int MinimaxScore(Cell pos, int depth)
    {
        if (depth == 0)
        {
            
        }
    }*/
    
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
