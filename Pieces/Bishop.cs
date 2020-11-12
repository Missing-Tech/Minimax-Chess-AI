using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
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
