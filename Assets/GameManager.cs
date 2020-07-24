using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board board;
    
    // Start is called before the first frame update
    void Start()
    {
        board.InitBoard();
    }

}
