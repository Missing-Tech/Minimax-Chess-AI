using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Piece : EventTrigger
{
    protected Sprite pieceSprite;
    
    public virtual void Init()
    {
        //todo
    }

    public Sprite PieceSprite
    {
        get => pieceSprite;
        set
        {
            pieceSprite = value;
            ChangeSprite();
        }
    }

    protected void ChangeSprite()
    {
        GetComponent<Image>().sprite = pieceSprite;
    }
}
