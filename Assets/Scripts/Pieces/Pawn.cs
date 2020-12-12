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
        //Separate logic for pawn to find moves
        int teamMultiplier;
        teamMultiplier = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)) ? -1 : 1;

        //Checks position in front of the pawn for another piece
        if (!cell.board.cellGrid[cell.cellPos.x, cell.cellPos.y + (1 * teamMultiplier)].CheckForAnyPiece())
        {
            //If it is the first turn, check two places in front
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

        //Checks the diagonals
        CheckForEnemy(Directions.NorthEast, 1 * teamMultiplier, highlightCells);
        CheckForEnemy(Directions.NorthWest, 1 * teamMultiplier, highlightCells);
    }

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

    //Checks if the pawn can take a piece
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

<<<<<<< Updated upstream
    public void CheckForEnemy(Directions direction, int multiplier, bool highlightCells)
=======
    //Checks for an enemy on the cell
    void CheckForEnemy(Directions direction, int multiplier, bool highlightCells)
>>>>>>> Stashed changes
    {
        Vector2 checkPos = cell.cellPos + (convertDirectionToVector2[direction] * multiplier);
        //If it's on the board
        if (IsInRange(checkPos))
        {
            Cell checkCell = cell.board.cellGrid[(int) checkPos.x, (int) checkPos.y];
            if (checkCell.CheckIfOtherTeam(pieceColor))
            {
                //Adds it to available cells if there's an enemy there
                availableCells.Add(checkCell);
                if (highlightCells)
                {
                    checkCell.SetOutline(true);
                }
            }
        }
    }
}