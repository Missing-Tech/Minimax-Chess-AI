using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class King : Piece
{
    bool isWhite;
    public bool isCheckmate;

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
    
    protected override void BeingThreatened()
    {
        base.BeingThreatened();
        Debug.Log(IsThreatened);
        CheckForNearbyMoves();
        if (isCheckmate)
        {
            GameManager.Instance.Win(isWhite);
        }
    }

    void CheckForNearbyMoves()
    {
        foreach (var direction in availableDirections)
        {
            Vector2 cellPos = cell.cellPos + convertDirectionToVector2[direction];
            if (IsInRange(cellPos))
            {
                Cell cellToCheck = cell.board.cellGrid[(int) cellPos.x, (int) cellPos.y];
                if (cellToCheck.CheckForAnyPiece())
                {
                    //Store all pieces locally
                    List<Piece> enemyPieces = !isWhite
                        ? FindObjectOfType<BoardManager>().whitePieces
                        : FindObjectOfType<BoardManager>().blackPieces;

                    bool isSafeMove = true;

                    //Check if any enemy can move to the cell
                    foreach (var piece in enemyPieces)
                    {
                        piece.FindValidMoves(false);
                        if (!piece.availableCells.Contains(cellToCheck))
                        {
                            isSafeMove = false;
                            break;
                        }
                        //piece.ClearCells();
                    }

                    if (isSafeMove)
                    {
                        availableCells.Add(cellToCheck);
                        isCheckmate = false;
                    }
                    else
                    {
                        isCheckmate = true;
                    }

                }
            }
        }
        //pieceThreatening.ClearCells();
    }
}
