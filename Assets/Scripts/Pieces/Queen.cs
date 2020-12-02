public class Queen : Piece
{
    //Queen Class
    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 8;
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
}
