using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public abstract class Piece : EventTrigger
{
    protected Sprite pieceSprite;
    protected Cell cell;
    protected Color pieceColor;
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
    protected List<Cell> availableCells;
    
    public virtual void Init(Cell cell)
    {
        outline = transform.GetChild(0).gameObject;
        outline.SetActive(false);
        this.cell = cell;
        pieceSprite = GetComponent<Image>().sprite;
        pieceColor = GetComponent<Image>().color;
    }

    public virtual void FindValidMoves()
    {
        foreach (var direction in availableDirections)
        {
            for (int i = 0; i < radius; i++)
            {
                Vector2 _cellPos = cell.cellPos;
                Vector2 newPos = _cellPos + convertDirectionToVector2[direction];
                
                if (pieceColor == Colours.ColourValue(Colours.ColourNames.LightBlue))
                {
                    newPos *= -1;
                }
                
                Cell availableCell = cell.board.cellGrid[(int)newPos.x,(int)newPos.y];
                availableCell.SetOutline(true);
                //availableCells.Add(availableCell);
            }
        }
    }

    void ClearCells()
    {
        foreach (var cell in availableCells)
        {
            cell.SetOutline(false);
            //availableCells.Remove(cell);
        }
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
        //ClearCells();
        outline.SetActive(false);
    }
}
