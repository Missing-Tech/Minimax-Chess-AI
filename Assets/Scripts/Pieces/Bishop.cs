public class Bishop : Piece
{
    //Bishop class
    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 8;
        availableDirections = new Directions[]
        {
            Directions.NorthEast,
            Directions.SouthEast,
            Directions.SouthWest,
            Directions.NorthWest
        };
    }
}
