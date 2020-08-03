using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
   public Image cellImage;
   public Board board;
   public Vector2Int cellPos;
   [HideInInspector]
   public RectTransform rectTransform;
   [HideInInspector] 
   public Piece currentPiece;
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

   public void SetPiece(Piece newPiece)
   {
      if (currentPiece != null && !currentPiece.PieceColor.Equals(newPiece.PieceColor))
      {
         currentPiece.gameObject.SetActive(false);
         RemovePiece();
      }
      currentPiece = newPiece;
   }

   public void RemovePiece()
   {
      currentPiece = null;
   }

   public bool CheckIfValid(Color32 pieceColour)
   {
      if (currentPiece == null)
      {
         return true;
      }
      if (currentPiece.PieceColor.Equals(pieceColour))
      {
         return false;
      }
      return true;
   }
   
   public Vector2 GetWorldPos()
   {
      return transform.position;
   }
   
}
