using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Colours;
using static Colours.ColourNames;

public class King : Piece
{
    bool isWhite;
    public bool isCheckmate;
    public bool inCheck;
    int noOfDangerCells = 0;
    public bool IsCheckmate
    {
        get
        {
            return CheckIfCheckmate();
        }
    }

    private void Start()
    {
        isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
    }

    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 1;
        availableDirections = new Directions[]
        {   
            Directions.North, 
            Directions.NorthEast, 
            Directions.East,
            Directions.SouthEast,
            Directions.South,
            Directions.SouthWest,
            Directions.West,
            Directions.NorthWest
        };
    }

    bool CheckIfCheckmate()
    {
        bool _isCheckmate = false;

        if (CheckEveryDirection(cell))
        {
            //Is currently in danger
            inCheck = true;
            FindValidMoves(false);
            if (availableCells.Count == 0)
            {
                _isCheckmate = true;
            }
            
            //Checks if the cells it can move into are in danger
            foreach (var availableCell in availableCells)
            {
                if(CheckEveryDirection(availableCell))
                {
                    noOfDangerCells++;
                }
            }

            //If all of the cells it can move into are dangerous then it's in checkmate
            if (noOfDangerCells == availableCells.Count)
            {
                _isCheckmate = true;
            }

            if (CanPiecesHelp() && noOfDangerCells == 0)
            {
                _isCheckmate = false;
            }

            ClearCells();
        }
        else
        {
            inCheck = false; 
        }
        
        return _isCheckmate;
    }

    bool CanPiecesHelp()
    {
        List<Cell> validCheckCells = GameManager.Instance.validCheckCells;
        bool _isWhite = pieceColor.Equals(Colours.ColourValue(White));
        List<Piece> pieces = _isWhite ? BoardManager.Instance.whitePieces : BoardManager.Instance.blackPieces;
        foreach (var piece in pieces)
        {
            piece.FindValidMoves(false);
            foreach (var cell in validCheckCells)
            {
                if (piece.availableCells.Contains(cell))
                {
                    noOfDangerCells--;
                    return true;
                }
            }
        }
        return false;
    }

    //Returns true if the cell is in danger
    bool CheckEveryDirection(Cell cellToCheck)
    {
        foreach (Directions direction in (Directions[]) Enum.GetValues(typeof(Directions)))
        {
            bool isKnightDirection = direction.ToString().Contains("L");
            Vector2Int checkCellPos = cellToCheck.cellPos;
            for (int i = 1; i <= 8; i++)
            {
                if (isKnightDirection && i > 1)
                {
                    break;
                }
                //Flips the vector if the player is on the black side
                if (pieceColor.Equals(ColourValue(ColourNames.Black)))
                {
                    checkCellPos -= Vector2Int.RoundToInt(convertDirectionToVector2[direction]);
                }
                else
                {
                    checkCellPos += Vector2Int.RoundToInt(convertDirectionToVector2[direction]);
                }
                Cell checkCell = null;
                if (IsInRange(checkCellPos))
                {
                    checkCell = BoardManager.Instance.board.cellGrid[checkCellPos.x, checkCellPos.y];
                }
                if (checkCell != null)
                {
                    if (checkCell.CheckIfOtherTeam(pieceColor))
                    {
                        Piece piece = checkCell.currentPiece;
                        if (i > piece.radius)
                        {
                            break;
                        }
                        //Temporarily lets the piece jump over other pieces so that the king
                        //Stops an edge case where the king can move away from a piece but still be in check
                        bool couldJumpOverPieces = piece.canJumpOverKing;
                        piece.canJumpOverKing = true;
                        
                        piece.FindValidMoves(false);
                        if (piece.availableCells.Contains(cellToCheck))
                        {
                            if (cellToCheck == cell)
                            {
                                GameManager.Instance.validCheckCells.Add(checkCell);
                                checkCell.SetOutline(true);
                            }
                            piece.canJumpOverKing = couldJumpOverPieces;
                            return true;
                        }
                        piece.canJumpOverKing = couldJumpOverPieces;
                    }
                }
            }
        }
        return false;
    }

    public override void FindValidMoves(bool highlightCells)
    {
        base.FindValidMoves(highlightCells);
        Cell[] _availableCells = availableCells.ToArray();
        foreach (var availableCell in _availableCells)
        {
            if (CheckEveryDirection(availableCell))
            {
                if (highlightCells)
                {
                    availableCell.SetOutline(false);
                }
                availableCells.Remove(availableCell);
            }
        }
    }
}
