using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
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
        CheckForNearbyMoves();
        if (availableCells.Count <= 1)
        {
            bool isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
            GameManager.Instance.Win(isWhite);
        }
    }

    private void OnDisable()
    {
        bool isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
        GameManager.Instance.Win(isWhite);
    }

    void CheckForNearbyMoves()
    {
        //pieceThreatening.FindValidMoves(false);
        foreach (var direction in availableDirections)
        {
            Vector2 cellPos = cell.cellPos + convertDirectionToVector2[direction];
            if (IsInRange(cellPos))
            {
                Cell cellToCheck = cell.board.cellGrid[(int) cellPos.x, (int) cellPos.y];
                if (!cellToCheck.CheckForAnyPiece())
                {
                    availableCells.Add(cellToCheck);
                }
            }
        }
        //pieceThreatening.ClearCells();
    }
}
