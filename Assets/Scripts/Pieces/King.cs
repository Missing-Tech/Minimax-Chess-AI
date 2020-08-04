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
        if (availableCells.Count == 0)
        {
            bool isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
            GameManager.Instance.Win(isWhite);
        }
    }

    public override void FindValidMoves()
    {
        base.FindValidMoves();
        BeingThreatened();
    }

    private void OnDisable()
    {
        bool isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
        GameManager.Instance.Win(isWhite);
    }

    void CheckForNearbyMoves()
    {
        foreach (var direction in availableDirections)
        {
            Vector2 posToCheck = cell.cellPos + convertDirectionToVector2[direction];
            if (posToCheck.x < 8 && posToCheck.y < 8 &&
                posToCheck.x >= 0 && posToCheck.y >= 0)
            {
                Cell cellToCheck = cell.board.cellGrid[(int) posToCheck.x, (int) posToCheck.y];
                if (!cellToCheck.CheckForAnyPiece())
                {
                    availableCells.Add(cellToCheck);
                }
            }
        }
    }
}
