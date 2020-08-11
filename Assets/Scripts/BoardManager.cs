﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using static Colours.ColourNames;

public class BoardManager : MonoBehaviour
{
    public Board board;
    public GameObject pieceObject;

    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    public MinimaxAI ai;

    [Tooltip("0 = Rook, 1 = Knight, 2 = Bishop, 3 = Queen, 4 = King, 5 = Pawn")]
    public Sprite[] pieceSprites;

    private int[] royalRow = {0, 1, 2, 3, 4, 2, 1, 0};
    

    private Dictionary<int, Type> pieceConverter = new Dictionary<int, Type>()
    {
        {0, typeof(Rook)},
        {1, typeof(Knight)},
        {2, typeof(Bishop)},
        {3, typeof(Queen)},
        {4, typeof(King)},
    };

    public Dictionary<Type, int> pieceEvaluation = new Dictionary<Type, int>() //Standard chess piece values
    {
        {typeof(Pawn),1},
        {typeof(Rook),5},
        {typeof(Knight),3},
        {typeof(Bishop),3},
        {typeof(Queen),9},
        {typeof(King),1000},
    };
    
    public void SpawnPieces()
    {
        //white pieces
        SpawnRow(true, 1, true);
        SpawnRow(false, 0, true);
        //black pieces
        SpawnRow(true, 6, false);
        SpawnRow(false, 7, false);
    }

    void SpawnRow(bool pawnRow, int cellColumn, bool whitePiece)
    {
        for (int x = 0; x < 8; x++)
        {
            //Gets the cell where to spawn the piece
            //More performant than multiple calls to the array
            var spawnedPieceCell = board.cellGrid[x, cellColumn];
            GameObject spawnedPiece = Instantiate(pieceObject, spawnedPieceCell.transform);
            spawnedPiece.transform.position = spawnedPieceCell.GetWorldPos();
            spawnedPiece.transform.parent = transform;

            //Checks what piece type it should be
            var pieceType = pawnRow ? typeof(Pawn) : pieceConverter[royalRow[x]];
            spawnedPiece.AddComponent(pieceType);

            //Stores the piece component since it's called multiple times
            var pieceComponent = spawnedPiece.GetComponent<Piece>();
            pieceComponent.PieceSprite = pawnRow ? pieceSprites[5] : pieceSprites[royalRow[x]];
            spawnedPieceCell.SetPiece(pieceComponent);

            if (whitePiece)
            {
                whitePieces.Add(spawnedPiece.GetComponent<Piece>());
                pieceComponent.Init(spawnedPieceCell, Colours.ColourValue(White));
            }
            else
            {
                blackPieces.Add(spawnedPiece.GetComponent<Piece>());
                pieceComponent.Init(spawnedPieceCell, Colours.ColourValue(Black));
            }
        }
    }

    public void EndTurn()
    {
        if (!GameManager.Instance.IsWhiteTurn)
        {
            ai.DoTurn();
        }
    }

    public int StaticEvaluation()
    {
        int score = 0;
        //Return the difference between value of white pieces vs black pieces
        foreach (var piece in whitePieces)
        {
            score += pieceEvaluation[piece.GetType()];
        }
        foreach (var piece in blackPieces)
        {
            score -= pieceEvaluation[piece.GetType()];
        }
        return score;
    }
    
    public void ResetBoard()
    {
        GameManager.Instance.Reset();
        var allPieces = whitePieces.Concat(blackPieces);
        foreach (var piece in allPieces)
        {
            piece.cell.RemovePiece();
            piece.cell = piece.originalCell;
            piece.gameObject.SetActive(true);
            piece.originalCell.SetPiece(piece);
            piece.transform.position = piece.cell.GetWorldPos();
        }
    }
}