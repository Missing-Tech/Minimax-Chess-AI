using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Piece
{
    private bool _isFirstMove;
    
    
    protected override void SetDirections()
    {
        _isFirstMove = true;
        
        base.SetDirections();
        availableDirections = new Directions[]
        {
            Directions.North,
        };
    }

    protected override void EndTurn()
    {
        base.EndTurn();
        _isFirstMove = false;
    }

    public override void FindValidMoves(bool highlightCells)
    {
        int teamMultiplier;
        teamMultiplier = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)) ? -1 : 1;

        if (!cell.board.cellGrid[cell.cellPos.x, cell.cellPos.y + (1 * teamMultiplier)].CheckForAnyPiece())
        {
            if (_isFirstMove && IsInRange(cell.cellPos + (Vector2Int.up * teamMultiplier * 2)) 
                && !cell.board.cellGrid[cell.cellPos.x, cell.cellPos.y + (2 * teamMultiplier)].CheckForAnyPiece())
            {
                AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier * 2), highlightCells);
            }
            if (IsInRange(cell.cellPos + (Vector2Int.up * teamMultiplier)))
            {
                AddAvailableCell(cell.cellPos + (Vector2Int.up * teamMultiplier), highlightCells);
            }
        }

        CheckForEnemy(Directions.NorthEast, 1 * teamMultiplier, highlightCells);
        CheckForEnemy(Directions.NorthWest, 1 * teamMultiplier, highlightCells);
    }

    void AddAvailableCell(Vector2Int pos, bool highlightCells)
    {
        if (IsInRange(pos))
        {
            Cell newCell = cell.board.cellGrid[pos.x, pos.y];
            //White is 0, black is 1
            int blackOrWhite = IsWhite(pieceColor) ? 0 : 1;
            King king = BoardManager.Instance.kings[blackOrWhite];
            if (king.inCheck)
            {
                if (!GameManager.Instance.validCheckCells.Contains(newCell))
                {
                    return;
                }
            }
            availableCells.Add(newCell);
            if (highlightCells)
            {
                newCell.SetOutline(true);
            }
        }
    }

    public override bool CanTakePiece(Cell cell)
    {
        if (!_isFirstMove)
        {
            if (availableCells.Contains(cell) && availableCells.IndexOf(cell) != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckForEnemy(Directions direction, int multiplier, bool highlightCells)
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