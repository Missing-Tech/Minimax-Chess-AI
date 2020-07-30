using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
   public Image cellImage;
   private Vector2Int cellPos;
   private Board board;
   private RectTransform rectTransform;
   
   public void Init(Board board, Vector2Int cellPos)
   {
      //Constructor that assigns the cell's variables
      this.board = board;
      this.cellPos = cellPos;

      rectTransform = GetComponent<RectTransform>();
   }

   public Vector2 GetWorldPos()
   {
      return transform.position;
   }
   
}
