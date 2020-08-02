using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
   public Image cellImage;
   public Board board;
   public Vector2Int cellPos;
   private RectTransform rectTransform;
   private GameObject outline;
   
   public void Init(Board board, Vector2Int cellPos)
   {
      //Constructor that assigns the cell's variables
      this.board = board;
      this.cellPos = cellPos;

      outline = transform.GetChild(0).gameObject;
      outline.SetActive(false);
      
      rectTransform = GetComponent<RectTransform>();
   }

   public void SetOutline(bool active)
   {
      outline.SetActive(active);
   }
   
   public Vector2 GetWorldPos()
   {
      return transform.position;
   }
   
}
