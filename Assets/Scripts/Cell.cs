using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
   //Stores necessary attributes for the cell
   public Image cellImage;
   public Board board;
   public Vector2Int cellPos;
   [HideInInspector]
   public RectTransform rectTransform;
   [HideInInspector] 
   public Piece currentPiece;
   private GameObject _outline;
   
   public void Init(Board board, Vector2Int cellPos)
   {
      //Constructor that assigns the cell's variables
      this.board = board;
      this.cellPos = cellPos;

      _outline = transform.GetChild(0).gameObject;
      _outline.SetActive(false);
      
      rectTransform = GetComponent<RectTransform>();
   }

   public void SetOutline(bool active)
   {
      //Sets the outline
      _outline.SetActive(active);
   }

   public void SetPiece(Piece newPiece)
   {
      //If there is a piece currently and it's on the other team
      if (currentPiece != null && !currentPiece.PieceColor.Equals(newPiece.PieceColor))
      {
         //"Takes" the piece
         currentPiece.gameObject.SetActive(false);
         RemovePiece();
      }
      //Sets the new piece as the cell's current piece
      currentPiece = newPiece;
   }

   public void RemovePiece()
   {
      //Removes the piece
      currentPiece = null;
   }

   //Checks if there's another piece of the same colour or no piece on the cell
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

   //Checks for any piece
   public bool CheckForAnyPiece()
   {
      if (currentPiece != null)
      {
         return true;
      }

      return false;
   }
   
   //Checks for a piece of the opposing colour
   public bool CheckIfOtherTeam(Color32 pieceColour)
   {
      if (currentPiece == null || currentPiece.PieceColor.Equals(pieceColour))
      {
         return false;
      }
      return true;
   }
   
<<<<<<< Updated upstream
   public bool CheckIfKing()
   {
      if (currentPiece != null && currentPiece.GetComponent<Piece>().GetType() == typeof(King))
      {
         return true;
      }
      return false;
   }
   
=======
   //Gets the global co-ords of the cell object
>>>>>>> Stashed changes
   public Vector2 GetWorldPos()
   {
      return transform.position;
   }
   
}
