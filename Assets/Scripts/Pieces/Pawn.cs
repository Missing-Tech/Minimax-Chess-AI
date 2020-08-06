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

    public override void FindValidMoves(bool highlightCells)
    {
        int teamMultiplier;
        teamMultiplier = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)) ? -1 : 1;

        if (!cell.board.cellGrid[cell.cellPos.x, cell.cellPos.y + (1 * teamMultiplier)].CheckForAnyPiece())
        {
            if (isFirstMove)
            {
                AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier * 2), highlightCells);
            }
            AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier), highlightCells);
        }

        CheckForEnemy(Directions.NorthEast, 1 * teamMultiplier, highlightCells);
        CheckForEnemy(Directions.NorthWest, 1 * teamMultiplier, highlightCells);
    }

    //todo cleanup
    
    void AddAvailableCell(Vector2Int pos, bool highlightCells)
    {
        if (IsInRange(pos))
        {
            Cell newCell = cell.board.cellGrid[pos.x, pos.y];
            availableCells.Add(newCell);
            if (highlightCells)
            {
                newCell.SetOutline(true);
            }
        }
    }
    
    void CheckForEnemy(Directions direction, int multiplier, bool highlightCells)
    {
        Vector2 checkPos = cell.cellPos + (convertDirectionToVector2[direction] * multiplier);
        if (IsInRange(checkPos))
        {
            Cell checkCell = cell.board.cellGrid[(int) checkPos.x, (int) checkPos.y];
            if (checkCell.CheckIfOtherTeam(pieceColor))
            {
                availableCells.Add(checkCell);
                if (highlightCells)
                {
                    checkCell.SetOutline(true);
                }
            }
        }
    }
}