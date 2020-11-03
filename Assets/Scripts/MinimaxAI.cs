using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static Colours.ColourNames;

public class MinimaxAI : MonoBehaviour
{
    private BoardManager _bm; //Local reference to the BoardManager object
    private BoardState _bestPossibleMove; //The final move the AI decides to do
    private int searchDepth = 7; //Increases search time exponentially

    private void Start()
    {
        _bm = BoardManager.Instance; //Gets the instance and stores it locally, saves calling the instance multiple times
    }

    /// <summary>
    /// Called when the AI does it's turn
    /// Calls a recursive search through possible moves
    /// Then moves the appropriate piece
    /// </summary>
    public void DoTurn()
    {
        var currentBoardPosition = new BoardState(searchDepth,_bm.board.cellGrid,searchDepth);

        //Calls a recursive depth search on a tree of possible board states
        //The 'alpha' and 'beta' values are used for alpha-beta pruning which optimises the search
        Minimax(searchDepth, currentBoardPosition, false, 
            -Mathf.Infinity,Mathf.Infinity);

        //Local reference to the cell grid
        Cell[,] cellGrid = new Cell[8,8];
        cellGrid = _bm.board.cellGrid;
        
        if (_bestPossibleMove != null)
        {
            //Finds the piece in the instantiated piece array
            Piece pieceToMove = _bm.blackPieces.Find(x => x == _bestPossibleMove.pieceToMove);
            //Finds the cell where to move the piece to on the board
            Cell cellToMove = cellGrid[_bestPossibleMove.cellToMove.cellPos.x, _bestPossibleMove.cellToMove.cellPos.y];

            //If there is a piece that can be taken
            if (cellToMove.CheckIfOtherTeam(pieceToMove.PieceColor))
            {
                //Remove the piece from the board
                cellToMove.currentPiece.gameObject.SetActive(false);
            }

            //Move the piece to the cell
            pieceToMove.Place(cellToMove);
        }

        //Resets the move for the next turn
        _bestPossibleMove = null;
    }

    /// <summary>
    /// Recursive search to find the best move
    /// </summary>
    /// <param name="depth">Number of moves it's looking ahead</param>
    /// <param name="boardState">The future board state</param>
    /// <param name="isMaximisingPlayer">Is it white</param>
    /// <param name="alpha">Alpha value</param>
    /// <param name="beta">Beta value</param>
    /// <returns></returns>
    private float Minimax(int depth, BoardState boardState, bool isMaximisingPlayer, float alpha, float beta)
    {
        //White is maximising, black is minimising
        if (depth == 1)
        {
            //Gives the board state the piece and cell to move
            _bestPossibleMove = boardState.ParentState;
            _bestPossibleMove.pieceToMove = boardState.pieceToMove;
            _bestPossibleMove.cellToMove = boardState.cellToMove;
            //Returns the static evaluation of the board state
            return CalculateStaticEvaluation(boardState.CellGrid, boardState);
        }
        
        if (isMaximisingPlayer)
        {
            float maxEval = -Mathf.Infinity;
            foreach (var nextMove in boardState.ChildrenStates)
            {
                //Recursively calls the function to the layer above in the tree
                    float eval = Minimax(depth - 1, nextMove, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    //Alpha beta pruning
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }
            return maxEval;
        }
        else
        {
            float minEval = Mathf.Infinity;
            foreach (var nextMove in boardState.ChildrenStates)
                {
                    //Recursively calls the function to the layer above in the tree
                    float eval = Minimax(depth - 1, nextMove, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    //Alpha beta pruning
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }

            return minEval;
        }
    }
    
    /// <summary>
    /// Calculates a score for the given grid
    /// </summary>
    /// <param name="cellGrid">Local array to store the board</param>
    /// <param name="boardState">The board state to calculate the score of</param>
    /// <returns></returns>
    private int CalculateStaticEvaluation(Cell[,] cellGrid, BoardState boardState)
    {
        //Local score variable to modify and then return
        int score = 0;
        //Loops through every cell on the board
        foreach (var cell in cellGrid)
        {
            if (cell.currentPiece != null)
            {
                //If there is an active piece
                if (cell.currentPiece.gameObject.activeSelf)
                {
                    if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(White)))
                    {
                        //Add the score of this piece
                        score += BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                    else if (cell.currentPiece.PieceColor.Equals(Colours.ColourValue(Black)))
                    {
                        //Minus the score of this piece
                        score -= BoardManager.Instance.pieceEvaluation[cell.currentPiece.GetType()];
                    }
                }
            }
        }

        //Joins together the white and black piece list
        var allPieces = _bm.whitePieces.Concat(_bm.blackPieces);
        //Finds the piece object
        Piece pieceToMove = allPieces.ToList().Find(x => x == boardState.pieceToMove);
        //Finds the cell
        Cell cellToMove = cellGrid[boardState.cellToMove.cellPos.x, boardState.cellToMove.cellPos.y];

        //If there's a piece that it can take
        if (cellToMove.CheckIfOtherTeam(pieceToMove.PieceColor))
        {
            //Adjust the score accordingly
            if (cellToMove.currentPiece.PieceColor.Equals(Colours.ColourValue(White)))
            {
                score -= BoardManager.Instance.pieceEvaluation[cellToMove.currentPiece.GetType()];
            }
            else if (cellToMove.currentPiece.PieceColor.Equals(Colours.ColourValue(Black)))
            {
                score += BoardManager.Instance.pieceEvaluation[cellToMove.currentPiece.GetType()];
            }
        }
        
        return score;
    }
}