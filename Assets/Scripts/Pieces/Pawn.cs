using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 2;
        availableDirections = new Directions[]
        {
            Directions.North,
        };
    }

    protected override void Place()
    {
        base.Place();
        radius = 1;
    }

    public override void FindValidMoves()
    {
        base.FindValidMoves();

        //Checks the diagonals for an enemy piece (Bulky)
        Directions[] diagonals = new[] {Directions.NorthEast, Directions.NorthWest};
        foreach (var direction in diagonals)
        {
            Vector2 newPos = cell.cellPos;
            //Flips the vector if the player is on the black side
            if (pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)))
            {
                newPos -= convertDirectionToVector2[direction];
            }
            else
            {
                newPos += convertDirectionToVector2[direction];
            }

            //Checks if the move is on the board
            if (newPos.x < 8 && newPos.y < 8 &&
                newPos.x >= 0 && newPos.y >= 0)
            {
                //Stores the cell
                Cell availableCell = cell.board.cellGrid[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y)];
                //Checks if the piece is on the other team
                if (availableCell.CheckIfOtherTeam(pieceColor))
                {
                    //Marks the piece as threatened by another if one of the possible moves of this piece is on it
                    availableCell.currentPiece.isThreatened = true;
                    piecesThreatened.Add(availableCell.currentPiece);
                    //Outlines the possible moves
                    availableCell.SetOutline(true);
                    //Stores all possible cells
                    availableCells.Add(availableCell);
                }
            }
        }
    }
}