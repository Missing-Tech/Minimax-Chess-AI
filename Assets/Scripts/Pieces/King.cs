using System;
using UnityEngine;
using static Colours;

public class King : Piece
{
    bool isWhite;
    public bool isCheckmate;
    public bool inCheck;

    public bool IsCheckmate
    {
        get
        {
            return CheckIfCheckmate();
        }
    }

    private void Start()
    {
        isWhite = pieceColor.Equals(ColourValue(ColourNames.White));
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

    //Check for checkmate (not finished)
    bool CheckIfCheckmate()
    {
        bool _isCheckmate = false;

        if (CheckEveryDirection(cell))
        {
            //If the king is under attack from another piece
            inCheck = true;
            FindValidMoves(false);
            
            //Checks if the cells it can move into are in danger
            int noOfDangerCells = 0;
            foreach (var availableCell in availableCells)
            {
                if(CheckEveryDirection(availableCell))
                {
                    noOfDangerCells++;
                }
            }

            //If all of the cells it can move into are dangerous then it's in checkmate
            if (noOfDangerCells == availableCells.Count || availableCells.Count == 0)
            {
                _isCheckmate = true;
            }
            
            ClearCells();
        }
        else
        {
            inCheck = false; 
        }
        
        return _isCheckmate;
    }

    //Returns true if the cell is in danger
    bool CheckEveryDirection(Cell cellToCheck)
    {
        //Checks every direction for an enemy piecec
        foreach (Directions direction in (Directions[]) Enum.GetValues(typeof(Directions)))
        {
            bool isKnightDirection = direction.ToString().Contains("L");
            Vector2Int checkCellPos = cellToCheck.cellPos;
            for (int i = 1; i <= 8; i++)
            {
                //Breaks out the loop if it's a knight after radius 1
                if (isKnightDirection && i > 1)
                {
                    break;
                }
                
                //Adds direction to the position to check
                checkCellPos += Vector2Int.RoundToInt(convertDirectionToVector2[direction]);
                
                Cell checkCell = null;
                //If the position is on the board
                if (IsInRange(checkCellPos))
                {
                    //Store the cell locally
                    checkCell = BoardManager.Instance.board.cellGrid[checkCellPos.x, checkCellPos.y];
                    
                    //If the cell contains a piece on the other team
                    if (checkCell.CheckIfOtherTeam(pieceColor))
                    {
                        Piece piece = checkCell.currentPiece;
                        bool isPawn = piece.GetComponent<Piece>().GetType() == typeof(Pawn);
                        //Immediately know that the piece can't attack the cell
                        if (i > piece.radius)
                        {
                            break;
                        }
                        //Temporarily lets the piece jump over other pieces so that the king
                        //Stops an edge case where the king can move away from a piece but still be in check
                        bool couldJumpOverPieces = piece.canJumpOverKing;
                        piece.canJumpOverKing = true;
                        
                        //Find all valid moves for the piece
                        piece.FindValidMoves(false);

                        //Fixes edge cases with pawns
                        if (isPawn)
                        {
                            CheckPawnPositions(piece);
                        }
                        
                        //See if one of the piece's moves is the cell
                        if (piece.availableCells.Contains(cellToCheck))
                        {
                            //If it's attacking the king piece
                            if (cellToCheck == cell)
                            {
                                //Make it a cell that can be attacked by other pieces
                                GameManager.Instance.validCheckCells.Add(piece.cell);
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

<<<<<<< Updated upstream
    void CheckPawnPositions(Piece piece)
    {
        Pawn pawn = piece.GetComponent<Pawn>();
        Vector2Int pawnCellPos = pawn.cell.cellPos;
        Cell[,] cellGrid = BoardManager.Instance.board.cellGrid;

        //All of the vectors necessary to check positions
        Vector2Int rUpDiag, lUpDiag, rDownDiag, lDownDiag,forward2, back2;
        rUpDiag = new Vector2Int(pawnCellPos.x + 1,pawnCellPos.y + 1);
        lUpDiag = new Vector2Int(pawnCellPos.x - 1, pawnCellPos.y + 1);
        rDownDiag = new Vector2Int(pawnCellPos.x + 1,pawnCellPos.y - 1);
        lDownDiag = new Vector2Int(pawnCellPos.x - 1, pawnCellPos.y - 1);
        forward2 = new Vector2Int(pawnCellPos.x,pawnCellPos.y + 2);
        back2 = new Vector2Int(pawnCellPos.x,pawnCellPos.y - 2);
        
        if (IsWhite(piece.PieceColor))
        {
            if (IsInRange(rUpDiag))
            {
                pawn.availableCells.Add(cellGrid[rUpDiag.x, rUpDiag.y]);
            }
            if (IsInRange(lUpDiag))
            {
                pawn.availableCells.Add(cellGrid[lUpDiag.x, lUpDiag.y]);
            }
            if (IsInRange(forward2))
            {
                pawn.availableCells.Remove(cellGrid[forward2.x,forward2.y - 1]);
                pawn.availableCells.Remove(cellGrid[forward2.x,forward2.y]);
            }
        }
        else
        {
            if (IsInRange(rDownDiag))
            {
                pawn.availableCells.Add(cellGrid[rDownDiag.x, rDownDiag.y]);
            }

            if (IsInRange(lDownDiag))
            {
                pawn.availableCells.Add(cellGrid[lDownDiag.x, lDownDiag.y]);

                if (IsInRange(back2))
                {
                    pawn.availableCells.Remove(cellGrid[back2.x, back2.y + 1]);
                    pawn.availableCells.Remove(cellGrid[back2.x, back2.y]);
                }
            }
        }
    }
    
    void OnDisable()
    {
        GameManager.Instance.Win(pieceColor.Equals(ColourValue(ColourNames.White)));
    }

=======
    //Looks for valid moves
>>>>>>> Stashed changes
    public override void FindValidMoves(bool highlightCells)
    {
        base.FindValidMoves(highlightCells);
        Cell[] _availableCells = availableCells.ToArray();
        foreach (var availableCell in _availableCells)
        {
            if (CheckEveryDirection(availableCell))
            {
                if (GameManager.Instance.validCheckCells.Contains(availableCell))
                {
                    availableCells.Remove(availableCell);
                    break;
                }
                if (highlightCells)
                {
                    availableCell.SetOutline(false);
                }
                availableCells.Remove(availableCell);
            }
        }
    }
}
