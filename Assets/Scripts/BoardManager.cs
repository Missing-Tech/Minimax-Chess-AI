using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Colours.ColourNames;

public class BoardManager : MonoBehaviour
{
    //Singleton pattern
    private static BoardManager _instance;
    public static BoardManager Instance { get { return _instance; } }
    
    //Stores local reference to the board
    public Board board;
    //Stores local reference to the piece prefab
    private GameObject pieceObject;

    //Stores a public list of all pieces separated by colour
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    //Local reference to the AI class
    public MinimaxAI ai;

    //Array to store all the different piece sprites with a tooltip for ease-of-use
    [Tooltip("0 = Rook, 1 = Knight, 2 = Bishop, 3 = Queen, 4 = King, 5 = Pawn")]
    public Sprite[] pieceSprites;

    //Array to store which pieces are where on the royal row, using same notation as tooltip above
    private readonly int[] _royalRow = {0, 1, 2, 3, 4, 2, 1, 0};
    
    //References to both kings
    public List<King> kings;

    //Dictionary to convet between an int and a piece type
    private Dictionary<int, Type> pieceConverter = new Dictionary<int, Type>()
    {
        {0, typeof(Rook)},
        {1, typeof(Knight)},
        {2, typeof(Bishop)},
        {3, typeof(Queen)},
        {4, typeof(King)},
    };

    //Dictionary to convert a piece type into a score, used for static evaluation
    //Values from chess.com
    public Dictionary<Type, int> pieceEvaluation = new Dictionary<Type, int>()
    {
        {typeof(Pawn),1},
        {typeof(Rook),5},
        {typeof(Knight),3},
        {typeof(Bishop),3},
        {typeof(Queen),9},
        {typeof(King),1000}, //King is a very high number so AI priorities above all other pieces
    };
    
    private void Awake()
    {
        //Singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    public void SpawnPieces()
    {
        //Spawns all the white pieces
        SpawnRow(true, 1, true);
        SpawnRow(false, 0, true);
        //Spawns all the black pieces
        SpawnRow(true, 6, false);
        SpawnRow(false, 7, false);
    }

    //Subroutine to create all piece objects with the correct components
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

            //Adds it to the piece array depending on the piece colour
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

            //Adds it to the king array if it's a piece of the same type
            if (isKing)
            {
                kings.Add(spawnedPiece.GetComponent<King>());
            }
        }
    }

    //Ran at the end of every turn
    public void EndTurn()
    {
        GameManager gm = GameManager.Instance;
        foreach (var king in kings)
        {
            //Checks if the king is in checkmate at the end of every turn
            if (king.IsCheckmate)
            {
<<<<<<< Updated upstream
                bool _isWhite = king.IsWhite(king.PieceColor);
                Debug.Log("checkmate");
                gm.Win(_isWhite);
=======
                //Wins the game if the king is in checkmate
                bool isWhite = king.PieceColor.Equals(Colours.ColourValue(White));
                GameManager.Instance.Win(isWhite);
>>>>>>> Stashed changes
            }
        }

        gm.validCheckCells = null;
        gm.UpdateScoreText();
        
<<<<<<< Updated upstream
        if (!gm.IsWhiteTurn)
=======
        //Updates the UI
        GameManager.Instance.UpdateScoreText();
        
        //If it's not the player's turn then it will make the AI do a single move
        if (!GameManager.Instance.IsWhiteTurn)
>>>>>>> Stashed changes
        {
            ai.DoTurn();
        }
    }

<<<<<<< Updated upstream
=======
    //Resets the board to all the original positions
>>>>>>> Stashed changes
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