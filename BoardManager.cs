using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using static Colours.ColourNames;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;

    public static BoardManager Instance { get { return _instance; } }
    
    public GameObject checkText;
    
    public Board board;
    public GameObject pieceObject;

    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    public MinimaxAI ai;

    [Tooltip("0 = Rook, 1 = Knight, 2 = Bishop, 3 = Queen, 4 = King, 5 = Pawn")]
    public Sprite[] pieceSprites;

    private int[] _royalRow = {0, 1, 2, 3, 4, 2, 1, 0};

    public List<Cell> validCheckCells;

    public List<King> kings;

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
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    public void SpawnPieces()
    {
        //white pieces
        SpawnRow(true, 1, true);
        SpawnRow(false, 0, true);
        //black pieces
        SpawnRow(true, 6, false);
        SpawnRow(false, 7, false);
    }

    void SpawnRow(bool isPawnRow, int cellColumn, bool isWhitePiece)
    {
        for (int x = 0; x < 8; x++)
        {
            bool isKing;
            
            //Gets the cell where to spawn the piece
            //More performant than multiple calls to the array
            var spawnedPieceCell = board.cellGrid[x, cellColumn];
            GameObject spawnedPiece = Instantiate(pieceObject, spawnedPieceCell.transform);
            spawnedPiece.transform.position = spawnedPieceCell.GetWorldPos();
            spawnedPiece.transform.SetParent(transform);

            //Checks what piece type it should be
            var pieceType = isPawnRow ? typeof(Pawn) : pieceConverter[_royalRow[x]];
            spawnedPiece.AddComponent(pieceType);

            isKing = pieceType == typeof(King);

            //Stores the piece component since it's called multiple times
            var pieceComponent = spawnedPiece.GetComponent<Piece>();
            pieceComponent.PieceSprite = isPawnRow ? pieceSprites[5] : pieceSprites[_royalRow[x]];
            spawnedPieceCell.SetPiece(pieceComponent);

            if (isWhitePiece)
            {
                whitePieces.Add(spawnedPiece.GetComponent<Piece>());
                pieceComponent.Init(spawnedPieceCell, Colours.ColourValue(White));
            }
            else
            {
                blackPieces.Add(spawnedPiece.GetComponent<Piece>());
                pieceComponent.Init(spawnedPieceCell, Colours.ColourValue(Black));
            }

            if (isKing)
            {
                kings.Add(spawnedPiece.GetComponent<King>());
            }
        }
    }

    public void EndTurn()
    {
        validCheckCells.Clear();
        GameManager gm = GameManager.Instance;
        foreach (var king in kings)
        {
            if (king.inCheck)
                checkText.SetActive(true);
            else
                checkText.SetActive(false);
            
            if (king.IsCheckmate)
            {
                bool _isWhite = king.IsWhite(king.PieceColor);
                gm.Win(_isWhite);
            }
        }

        gm.UpdateScoreText();
        
        if (!gm.IsWhiteTurn)
        {
            ai.DoTurn();
        }
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