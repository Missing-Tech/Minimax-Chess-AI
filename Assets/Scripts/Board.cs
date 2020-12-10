using UnityEngine;
using static Colours.ColourNames;

public class Board : MonoBehaviour
{
    private static Board _instance;

    public static Board Instance { get { return _instance; } }
    
    public GameObject cellPrefab;
    
    public Cell[,] cellGrid = new Cell[8,8];

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void InitBoard()
    {
        //Makes the grid visible to the player
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                //Instantiates a cell
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.GetComponent<RectTransform>().anchoredPosition = new Vector2((x*100)+50,(y*100)+50);

                //Adds the cell to the grid
                cellGrid[x, y] = cell.GetComponent<Cell>();
                cellGrid[x,y].Init(this, new Vector2Int(x,y));
            }
        }

        ColourBoard();
    }

    void ColourBoard()
    {
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 8; y++)
            {
                int offsetX = x;

                //Offsets the grid every 2nd row to make a checkerboard pattern
                if (y % 2 == 0)
                    offsetX += 1;

                cellGrid[offsetX, y].cellImage.color = Colours.ColourValue(PaleBrown);
            }
        }
        FindObjectOfType<BoardManager>().SpawnPieces();
    }
    
}
