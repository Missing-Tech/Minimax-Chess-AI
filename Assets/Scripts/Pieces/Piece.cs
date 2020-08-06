﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Defines the base class for all pieces - inheriting from event trigger
public abstract class Piece : EventTrigger
{
    public Cell originalCell;
    protected Cell cellLastTurn;
    public Cell cell;
    
    protected Sprite pieceSprite;
    protected Color32 pieceColor;
    protected GameObject outline;
    protected bool canJumpOverPieces = false;
    private bool isThreatened;
    //public Piece pieceThreatening;

    protected List<Piece> piecesThreatened = new List<Piece>();
    

    //Stores all possible directions in plaintext
    protected enum Directions
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
    protected int radius = 1;

    //Stores all possible directions a piece can move
    protected Directions[] availableDirections;

    //Stores the possible moves from the current position
    public List<Cell> availableCells = new List<Cell>();

    //Constructor to initialise the variables
    public virtual void Init(Cell cell, Color32 pieceColour)
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

    protected virtual void BeingThreatened()
    {
        //Threatened - Handled by the piece
    }

    public virtual void FindValidMoves(bool highlightCells)
    {
        //ClearCells();
        //Loops through all possible directions a piece can move
        foreach (var direction in availableDirections)
        {
            Vector2 newPos = cell.cellPos;
            for (int i = 1; i <= radius; i++)
            {
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
                if (IsInRange(newPos))
                {
                    //Stores the cell
                    Cell availableCell = cell.board.cellGrid[(int)newPos.x, (int)newPos.y];
                    //Checks if the piece is on the other team
                    if (availableCell.CheckIfValid(pieceColor))
                    {
                        if (highlightCells)
                        {
                            //Outlines the possible moves
                            availableCell.SetOutline(true);
                        }
                        //Stores all possible cells
                        availableCells.Add(availableCell);
                    }

                    if (availableCell.CheckForAnyPiece())
                    {
                        //Marks the piece as threatened by another if one of the possible moves of this piece is on it
                        if (availableCell.CheckIfOtherTeam(pieceColor))
                        {
                            availableCell.currentPiece.IsThreatened = true;
                            //availableCell.currentPiece.pieceThreatening = this;
                            piecesThreatened.Add(availableCell.currentPiece);
                        }
                        if (!canJumpOverPieces)
                            break;
                    }

                    //Add the current cell the piece is on so you can put the piece back down
                    availableCells.Add(cell);
                }
            }
        }
    }

    protected virtual void Place()
    {
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
        cell.RemovePiece();
        cell = cellBelow;
        cell.SetPiece(this);
        transform.position = cell.GetWorldPos();

        if (cell != cellLastTurn)
        {
            EndTurn();
            //ClearThreatenedPieces();
        }
    }

    protected virtual void EndTurn()
    {
        GameManager.Instance.IsWhiteTurn = !GameManager.Instance.IsWhiteTurn;
        cellLastTurn = cell;
    }

    void ClearThreatenedPieces()
    {
        //Clears all threatened pieces
        foreach (var piece in piecesThreatened)
        {
            piece.IsThreatened = false;
            //piece.pieceThreatening = null;
        }
        piecesThreatened.Clear();
    }
    
    public void ClearCells()
    {
        //Clears all highlighted cells
        foreach (var cell in availableCells)
            cell.SetOutline(false);
        availableCells.Clear();
    }

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

    public bool IsThreatened
    {
        get => isThreatened;
        set
        {
            isThreatened = value;
            if (isThreatened)
            {
                BeingThreatened();
            }
        }
    }
    
    public Color32 PieceColor => pieceColor;

    protected void ChangeSprite()
    {
        GetComponent<Image>().sprite = pieceSprite;
    }

    #region Events

    protected bool IsTeamTurn()
    {
        if (GameManager.Instance.IsWhiteTurn && pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White)) || 
            !GameManager.Instance.IsWhiteTurn && pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)))
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
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(IsTeamTurn())
        {
            base.OnBeginDrag(eventData);
            FindValidMoves(true);
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
            Place();
            ClearCells();
        }
    }

    #endregion
}