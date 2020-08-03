using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    protected override void SetDirections()
    {
        base.SetDirections();
        radius = 8;
        availableDirections = new Directions[]
        {   
            Directions.North,
            Directions.East,
            Directions.South,
            Directions.West,
        };
    }
}
