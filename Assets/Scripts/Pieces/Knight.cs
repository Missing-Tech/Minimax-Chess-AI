using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
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
