using System.Collections.Generic;
using static Colours.ColourNames;

/// <summary>
/// This class is used for future board states so that the AI can predict the best move for the future
/// Creates children classes for itself
/// </summary>
public class BoardState
{
    private Cell[,] _cellGrid = new Cell[8, 8];
    public Cell[,] CellGrid => _cellGrid;
    private BoardState _parentState;
    public BoardState ParentState => _parentState;
    public List<BoardState> childrenStates;
    public Piece pieceToMove;
    public Cell cellToMove;

    public List<BoardState> ChildrenStates
    {
        get
        {
            if (childrenStates == null || childrenStates.Count == 0)
            {
                //If there isn't any children when it's referenced then create some
                //More performant than creating every single necessary child at once
                //Only called when necessary
                return CreateChildrenStates();
            }

            return childrenStates;
        }
    }

    public int _depth, _maxDepth;

    /// <summary>
    /// Constructor for future board states
    /// </summary>
    /// <param name="maxDepth">Limiting depth to stop the search from going on forever</param>
    /// <param name="depth">The depth of this board state (how many moves ahead)</param>
    /// <param name="parentState">Stores the board state that this is a child of</param>
    /// <param name="cellGrid">Stores a local representation of the board</param>
    /// <param name="pieceToMove">Stores the piece object to move(local to this board state)</param>
    /// <param name="cellToMove">Stores the cell to move to(local to this board state)</param>
    private BoardState(int maxDepth, int depth, BoardState parentState,
        Cell[,] cellGrid, Piece pieceToMove, Cell cellToMove)
    {
        _maxDepth = maxDepth;
        _depth = depth;
        _cellGrid = cellGrid;
        this.pieceToMove = pieceToMove;
        this.cellToMove = cellToMove;
        _parentState = parentState;
    }

    /// <summary>
    /// Constructor for the current board state 
    /// </summary>
    /// <param name="maxDepth">Limiting depth to stop the search from going on forever</param>
    /// <param name="cellGrid">Stores a local representation of the board</param>
    /// <param name="depth">The depth of this board state (how many moves ahead)</param>
    public BoardState(int maxDepth, Cell[,] cellGrid, int depth)
    {
        _maxDepth = maxDepth;
        _depth = depth;
        _parentState = this;
        pieceToMove = null;
        cellToMove = null;
        _cellGrid = cellGrid;
    }

    /// <summary>
    /// Loops through every piece and creates a board state for every possible move
    /// </summary>
    /// <returns>Returns a list of future board states from this board state</returns>
    public List<BoardState> CreateChildrenStates()
    {
        List<BoardState> localChildrenStates = new List<BoardState>();

        foreach (var cell in _cellGrid)
        {
            Piece piece = cell.currentPiece;
            if (piece != null)
            {
                bool isAITurn = _depth % 2 != 0; //Returns false if it's the AI's turn
                                                 //AI turns are odd depth values
                bool canMoveThePiece = (piece.IsWhite(piece.PieceColor) && !isAITurn) ||
                                       (!piece.IsWhite(piece.PieceColor) && isAITurn);
                if (!canMoveThePiece) //If it's on the AI's turn
                {
                    piece.FindValidMoves(false);
                    Cell[] availableCells = piece.availableCells.ToArray();
                    //Check every cell the piece can move to
                    foreach (var availableCell in availableCells)
                    {
                        //Create a local cell grid
                        Cell[,] newCellGrid = new Cell[8,8];
                        newCellGrid = _cellGrid;

                        //Create a new board state as a child
                        BoardState childBoardState = new BoardState(_maxDepth, _depth - 1, _parentState,
                            newCellGrid, piece, availableCell);
                        localChildrenStates.Add(childBoardState);
                    }
                    piece.ClearCells();
                }
            }
        }

        return localChildrenStates;
    }
}