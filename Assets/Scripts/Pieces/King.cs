using static Colours;

public class King : Piece
{
    bool isWhite;
    public bool inCheck;

    public bool IsCheckmate => CheckIfCheckmate();

    private void Start()
    {
        isWhite = IsWhite(pieceColor);
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

    bool CheckIfCheckmate()
    {
        inCheck = false;
        if (!CheckIfCheck())
        {
            return false;
        }
        //Currently in check
        inCheck = true;
        //Makes a board state one move ahead
        BoardState nextState = new BoardState(1, FindObjectOfType<Board>().cellGrid, 1);
        if (nextState.whiteCheck && IsWhite(pieceColor)|| nextState.blackCheck && !IsWhite(pieceColor))
        {
            return true;
        }
        return false;
    }

    bool CheckIfCheck()
    {
        //Looks at the current board state to see if the king is in check
        BoardState currentState = new BoardState(0,FindObjectOfType<Board>().cellGrid, 1);
        return currentState.whiteCheck || currentState.blackCheck;
    }

    void OnDisable()
    {
        GameManager.Instance.Win(pieceColor.Equals(ColourValue(ColourNames.White)));
    }

    public override void FindValidMoves(bool highlightCells)
    {
        base.FindValidMoves(highlightCells);
        Cell[] _availableCells = availableCells.ToArray();
    }
}
