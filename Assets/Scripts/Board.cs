using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Colours.ColourNames;

public class Board : MonoBehaviour
{

    public GameObject cellPrefab;
    
    public Cell[,] cellGrid = new Cell[8,8];
    
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

                if (y % 2 == 0)
                    offsetX += 1;

                cellGrid[offsetX, y].cellImage.color = Colours.ColourValue(PaleBrown);
            }
        }
        FindObjectOfType<BoardManager>().SpawnPieces();
    }
    
}
