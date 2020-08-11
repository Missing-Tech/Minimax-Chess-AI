using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Piece piece;
    public Cell move;

    public Move(Piece piece, Cell move)
    {
        this.piece = piece;
        this.move = move;
    }
}
