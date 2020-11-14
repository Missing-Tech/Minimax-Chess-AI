using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Colours;

//Defines the base class for all pieces - inheriting from event trigger
public abstract class Piece : EventTrigger
{
    public Cell originalCell;
    public Cell cellLastTurn;
    public Cell cell;
    
    protected Sprite pieceSprite;
    protected Color32 pieceColor;
    protected GameObject outline;
    protected bool canJumpOverPieces = false;
    public bool canJumpOverKing = false;
    
    public Sprite PieceSprite
    {
        get => pieceSprite;
        set
        {
            //Updates the sprite image when it's changed
            pieceSprite = value;
            ChangeSprite();
        }
    }

    public Color32 PieceColor => pieceColor;

    protected void ChangeSprite()
    {
        GetComponent<Image>().sprite = pieceSprite;
    }

    //Stores all possible directions in plaintext
    public enum Directions
    {
        //Cardinal direction movement (clockwise)
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,

        //Knight movement (clockwise)
        NorthEastL,
        EastNorthL,
        EastSouthL,
        SouthEastL,
        SouthWestL,
        WestSouthL,
        WestNorthL,
        NorthWestL,
    }

    //Converts the direction into a vector2
    protected Dictionary<Directions, Vector2> convertDirectionToVector2 = new Dictionary<Directions, Vector2>()
    {
        //Cardinal direction movement (clockwise)
        {Directions.North, Vector2.up},
        {Directions.NorthEast, new Vector2(1, 1)},
        {Directions.East, Vector2.right},
        {Directions.SouthEast, new Vector2(1, -1)},
        {Directions.South, Vector2.down},
        {Directions.SouthWest, new Vector2(-1, -1)},
        {Directions.West, Vector2.left},
        {Directions.NorthWest, new Vector2(-1, 1)},
        //Knight movement (clockwise)
        {Directions.NorthEastL, new Vector2(1, 2)},
        {Directions.EastNorthL, new Vector2(2, 1)},
        {Directions.EastSouthL, new Vector2(2, -1)},
        {Directions.SouthEastL, new Vector2(1, -2)},
        {Directions.SouthWestL, new Vector2(-1, -2)},
        {Directions.WestSouthL, new Vector2(-2, -1)},
        {Directions.WestNorthL, new Vector2(-2, 1)},
        {Directions.NorthWestL, new Vector2(-1, 2)},
    };

    //The radius of possible moves, e.g. a king has a radius of 1 but a queen has a radius of 8
    public int radius = 1;

    //Stores all possible directions a piece can move
    protected Directions[] availableDirections;

    //Stores the possible moves from the current position
    public List<Cell> availableCells = new List<Cell>();

    //Constructor to initialise the variables
    public void Init(Cell cell, Color32 pieceColour)
    {
        //Populates the possible directions array
        SetDirections();

        //Stores the outline and disables it
        outline = transform.GetChild(0).gameObject;
        outline.SetActive(false);

        this.cell = cell;
        cellLastTurn = cell;
        originalCell = cell;
        
        //Gives the piece a sprite and colour
        Image pieceImage = GetComponent<Image>();
        pieceSprite = pieceImage.sprite;
        pieceImage.color = pieceColour;
        pieceColor = pieceColour;
    }

    protected virtual void SetDirections()
    {
        //SetDirections - Handled by the piece
    }

    public virtual void FindValidMoves(bool highlightCells)
    {
        //Loops through all possible directions a piece can move
        foreach (var direction in availableDirections)
        {
            Vector2 newPos = cell.cellPos;
            for (int i = 1; i <= radius; i++)
            {
                //Flips the vector if the player is on the black side
                if (IsWhite(pieceColor))
                {
                    newPos += convertDirectionToVector2[direction];
                }
                else
                {
                    newPos -= convertDirectionToVector2[direction];
                }

                //Checks if the move is on the board
                if (IsInRange(newPos))
                {
                    //Stores the cell
                    Cell availableCell = cell.board.cellGrid[(int)newPos.x, (int)newPos.y];
                    //Checks if the piece is on the other team
                    if (availableCell.CheckIfValid(pieceColor))
                    {
                        bool isValid = true;
                        int blackOrWhite = IsWhite(pieceColor) ? 0 : 1;
                        King king = BoardManager.Instance.kings[blackOrWhite];
                        if (king.inCheck)
                        {
                            if (!GameManager.Instance.validCheckCells.Contains(availableCell))
                            {
                                isValid = false;
                            }
                        }
                        if (isValid)
                        {
                            if (highlightCells)
                            {
                                //Outlines the possible moves
                                availableCell.SetOutline(true);
                            }
                            //Stores all possible cells
                            availableCells.Add(availableCell);
                        }
                    }

                    //Used for checkmate edge case
                    if (availableCell.CheckIfOtherTeam(pieceColor) && !canJumpOverKing)
                    {
                        break;
                    }
                    
                    //If there's a piece there and the piece can't jump over it
                    if (availableCell.CheckForAnyPiece() && !canJumpOverPieces)
                    {
                        //Break out the loop
                        break;
                    }
                }
            }
        }
    }

    public bool IsWhite(Color32 colour)
    {
        return colour.Equals(ColourValue(ColourNames.White));
    }

    public void Place(Cell cellToMoveTo)
    {
        Move(cellToMoveTo);

        if (cell != cellLastTurn)
        {
            EndTurn();
            //FindValidMoves(false);
        }
    }

    public void Move(Cell cellToMoveTo)
    {
        cell.RemovePiece();
        cell = cellToMoveTo;
        cell.SetPiece(this);
        transform.position = cell.GetWorldPos();
    }

    protected virtual void EndTurn()
    {
        GameManager.Instance.IsWhiteTurn = !GameManager.Instance.IsWhiteTurn;
        cellLastTurn = cell;
    }

    public void ClearCells()
    {
        //Clears all highlighted cells
        foreach (var cell in availableCells)
            cell.SetOutline(false);
        availableCells.Clear();
    }

    protected bool IsTeamTurn()
    {
        if (GameManager.Instance.IsWhiteTurn && IsWhite(pieceColor))
        {
            return true;
        }
        return false;
    }

    protected bool IsInRange(Vector2 pos)
    {
        if (pos.x < 8 && pos.y < 8 &&
            pos.x >= 0 && pos.y >= 0)
        {
            return true;
        }
        return false;
    }
    
    //Used for pawn
    public virtual bool CanTakePiece(Cell cell)
    {
        return true;
    }
    
    #region Events
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(IsTeamTurn())
        {
            base.OnBeginDrag(eventData);
            //Add the current cell the piece is on so you can put the piece back down
            availableCells.Add(cell);
            int blackOrWhite = IsWhite(pieceColor) ? 0 : 1;
            King king = BoardManager.Instance.kings[blackOrWhite];
            if (!king.inCheck)
            {
                FindValidMoves(true);
            }
            outline.SetActive(true);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (IsTeamTurn())
        {
            base.OnDrag(eventData);
            transform.position = eventData.position;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (IsTeamTurn())
        {
            base.OnEndDrag(eventData);
            outline.SetActive(false);
            
            //Stores the cellPos locally
            Vector2Int cellBelowPos = cell.cellPos;

            //Go through all possible moves for the piece
            foreach (var availableCell in availableCells)
            {
                //If the mouse is on top of one of the cells
                if (RectTransformUtility.RectangleContainsScreenPoint(availableCell.rectTransform, Input.mousePosition))
                {
                    //Set the local variable to the cell the mouse is on
                    cellBelowPos = availableCell.cellPos;
                }
            }
            //Moves the piece to the selected cell
            Cell cellBelow = cell.board.cellGrid[cellBelowPos.x, cellBelowPos.y];
            
            Place(cellBelow);
            ClearCells();
        }
    }

    #endregion
}