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
    private Dictionary<Move, int> _possibleMoves = new Dictionary<Move, int>();
    private List<Piece> _blackPieces;
    private BoardManager _bm;

    private void Start()
    {
        _bm = FindObjectOfType<BoardManager>();
        _blackPieces = _bm.blackPieces;
    }

    public void DoTurn()
    {
        _possibleMoves = FindPossibleMoves();
        Move bestMove = _possibleMoves.OrderBy(x => x.Value).First().Key;
        Piece pieceToMove = bestMove.piece;
        pieceToMove.Place(bestMove.move);
        ClearMoves();
    }

    void ClearMoves()
    {
        foreach (var piece in _bm.blackPieces)
        {
            piece.availableCells.Clear();
        }
        _possibleMoves.Clear();
    }

    Dictionary<Move,int> FindPossibleMoves()
    {
        Dictionary<Move,int> possibleMoves = new Dictionary<Move, int>();
        foreach (var piece in _bm.blackPieces)
        {
            piece.FindValidMoves(false);
            foreach (var cell in piece.availableCells)
            {
                int score = _bm.StaticEvaluation();
                if (cell.CheckIfOtherTeam(Colours.ColourValue(Black)))
                {
                    score -= _bm.pieceEvaluation[cell.currentPiece.GetType()];
                }
                possibleMoves.Add(new Move(piece,cell), score);
            }
        }
        return possibleMoves;
    }
    
}
