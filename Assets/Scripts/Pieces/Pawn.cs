using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool isFirstMove;
    
    
    protected override void SetDirections()
    {
        isFirstMove = true;
        
        base.SetDirections();
        availableDirections = new Directions[]
        {
            Directions.North,
        };
    }

    protected override void Place()
    {
        base.Place();
    }

    protected override void EndTurn()
    {
        base.EndTurn();
        isFirstMove = false;
    }

    public override void FindValidMoves()
    {
        ClearThreatenedPieces();
        
        int teamMultiplier;
        teamMultiplier = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)) ? -1 : 1;

        if (!cell.board.cellGrid[cell.cellPos.x, cell.cellPos.y + (1 * teamMultiplier)].CheckForAnyPiece())
        {
            if (isFirstMove)
            {
                AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier * 2));
            }
            AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier));
        }

        CheckForEnemy(Directions.NorthEast, 1 * teamMultiplier);
        CheckForEnemy(Directions.NorthWest, 1 * teamMultiplier);
    }

    void AddAvailableCell(Vector2Int pos)
    {
        if (pos.x < 8 && pos.y < 8 &&
            pos.x >= 0 && pos.y >= 0)
        {
            Cell newCell = cell.board.cellGrid[pos.x, pos.y];
            availableCells.Add(newCell);
            newCell.SetOutline(true);
        }
    }
    
    void CheckForEnemy(Directions direction, int multiplier)
    {
        Vector2 checkPos = cell.cellPos + (convertDirectionToVector2[direction] * multiplier);
        if (checkPos.x < 8 && checkPos.y < 8 &&
            checkPos.x >= 0 && checkPos.y >= 0)
        {
            Cell checkCell = cell.board.cellGrid[(int) checkPos.x, (int) checkPos.y];
            if (checkCell.CheckIfOtherTeam(pieceColor))
            {
                availableCells.Add(checkCell);
                checkCell.SetOutline(true);
            }
        }
    }
}