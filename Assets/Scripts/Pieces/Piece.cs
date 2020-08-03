using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public abstract class Piece : EventTrigger
{
    protected Sprite pieceSprite;
    protected Cell cell;
    protected Color32 pieceColor;
    protected GameObject outline;
    
    public enum Directions
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    private Dictionary<Directions, Vector2> convertDirectionToVector2 = new Dictionary<Directions, Vector2>()
    {
        {Directions.North, Vector2.up},
        {Directions.NorthEast, new Vector2(1,1)},
        {Directions.East, Vector2.right},
        {Directions.SouthEast, new Vector2(1,-1)},
        {Directions.South, Vector2.down},
        {Directions.SouthWest, new Vector2(-1,-1)},
        {Directions.West, Vector2.left},
        {Directions.NorthWest, new Vector2(-1,1)},
    };
    
    protected int radius = 1;

    protected Directions[] availableDirections = new Directions[]
        {Directions.North,Directions.NorthEast,Directions.NorthWest};
    public List<Cell> availableCells = new List<Cell>();
    
    public virtual void Init(Cell cell, Color32 pieceColour)
    {
        outline = transform.GetChild(0).gameObject;
        outline.SetActive(false);
        this.cell = cell;
        Image pieceImage = GetComponent<Image>();
        pieceSprite = pieceImage.sprite;
        pieceImage.color = pieceColour;
        pieceColor = pieceColour;
    }

    public virtual void FindValidMoves()
    {
        foreach (var direction in availableDirections)
        {
            Vector2 newPos = cell.cellPos;
            for (int i = 1; i <= radius; i++)
            {
                if (pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.Black)))
                {
                    newPos -= convertDirectionToVector2[direction];
                }
                else
                {
                    newPos += convertDirectionToVector2[direction];
                }

                if (newPos.x < 8 && newPos.y < 8 &&
                newPos.x >= 0 && newPos.y >= 0)
                {
                    Cell availableCell = cell.board.cellGrid[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y)];
                    if (availableCell.CheckIfValid(pieceColor))
                    {
                        availableCell.SetOutline(true);
                        availableCells.Add(availableCell);
                    }
                    availableCells.Add(cell);
                }
            }
        }
    }

    void Place()
    {
        Vector2Int cellBelowPos = Vector2Int.zero;

        foreach (var availableCell in availableCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(availableCell.rectTransform,Input.mousePosition))
            {
                cellBelowPos = availableCell.cellPos;
            }
        }
        Cell cellBelow = cell.board.cellGrid[cellBelowPos.x, cellBelowPos.y];
        cell.RemovePiece();
        cell = cellBelow;
        cell.SetPiece(this);
        transform.position = cell.GetWorldPos();
    }
    
    void ClearCells()
    {
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

    public Color32 PieceColor => pieceColor;

    protected void ChangeSprite()
    {
        GetComponent<Image>().sprite = pieceSprite;
    }
    
    //Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        FindValidMoves();
        outline.SetActive(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position = eventData.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        outline.SetActive(false);
        Place();
        ClearCells();
    }
}
