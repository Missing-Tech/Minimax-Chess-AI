public class Knight : Piece
{
    //Knight class
    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 1;
        canJumpOverPieces = true;
        availableDirections = new Directions[]
        {
            Directions.NorthEastL,
            Directions.EastNorthL,
            Directions.EastSouthL,
            Directions.SouthEastL,
            Directions.SouthWestL,
            Directions.WestSouthL,
            Directions.WestNorthL,
            Directions.NorthWestL, 
        };
    }
}
