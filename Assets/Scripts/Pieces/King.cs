using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class King : Piece
{
    bool isWhite;
    public bool isCheckmate;

    private void Start()
    {
        isWhite = pieceColor.Equals(Colours.ColourValue(Colours.ColourNames.White));
    }

    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 1;
        availableDirections = new Directions[]
        {   
            Directions.North, 
            Directions.NorthEast, 
            Directions.East,
            Directions.SouthEast,
            Directions.South,
            Directions.SouthWest,
            Directions.West,
            Directions.NorthWest
        };
    }
    
    protected override void BeingThreatened()
    {
        base.BeingThreatened();
        Debug.Log(IsThreatened);
        CheckForNearbyMoves();
        if (isCheckmate)
        {
            GameManager.Instance.Win(isWhite);
        }
    }

    void CheckForNearbyMoves()
    {
        foreach (var direction in availableDirections)
        {
            Vector2 cellPos = cell.cellPos + convertDirectionToVector2[direction];
            if (IsInRange(cellPos))
            {
                Cell cellToCheck = cell.board.cellGrid[(int) cellPos.x, (int) cellPos.y];
                if (cellToCheck.CheckForAnyPiece())
                {
                    //Store all pieces locally
                    List<Piece> enemyPieces = !isWhite
                        ? FindObjectOfType<BoardManager>().whitePieces
                        : FindObjectOfType<BoardManager>().blackPieces;
                    
                    /*List<Piece> friendlyPieces = isWhite
                        ? FindObjectOfType<BoardManager>().whitePieces
                        : FindObjectOfType<BoardManager>().blackPieces;

                    friendlyPieces.Remove(this);*/

                    bool isSafeMove = true;

                    //Check if any enemy can move to the cell
                    foreach (var piece in enemyPieces)
                    {
                        piece.FindValidMoves(false);
                        if (!piece.availableCells.Contains(cellToCheck))
                        {
                            isSafeMove = false;
                            break;
                        }
                        //piece.ClearCells();
                    }

                    if (isSafeMove)
                    {
                        availableCells.Add(cellToCheck);
                        isCheckmate = false;
                    }
                    else
                    {
                        isCheckmate = true;
                    }
                    
                    //Check if a friendly piece can block the enemy
                    /*foreach (var piece in friendlyPieces)
                    {
                        pieceThreatening.GetComponent<Piece>().FindValidMoves(false);
                        List<Cell> threatenedPieceMoves = pieceThreatening.GetComponent<Piece>().availableCells;
                        
                        piece.FindValidMoves(false);
                        foreach (var move in threatenedPieceMoves)
                        {
                            if (piece.availableCells.Contains(move))
                            {
                                Debug.Log("qwoop");
                                isCheckmate = false;
                                //break;
                            }
                        }
                    }*/
                        
                }
            }
        }
        //pieceThreatening.ClearCells();
    }
}
