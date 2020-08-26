using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public TextMeshProUGUI winText;
    public bool gameWon;
    private BoardManager bm;
    private bool isWhiteTurn = true;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        bm = FindObjectOfType<BoardManager>();
    }

    public Board board;

    public bool IsWhiteTurn
    {
        get => isWhiteTurn;
        set
        {
            isWhiteTurn = value;
            bm.EndTurn();
        }
    }

    void Start()
    {
        board.InitBoard();
    }

    private void Update()
    {
        if (gameWon)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                bm.ResetBoard();
            }
        }
    }

    public void Win(bool isWhite)
    {
        winText.gameObject.SetActive(true);
        winText.text = isWhite ? "BLACK WINS" : "WHITE WINS";
        gameWon = true;
    }

    public void Reset()
    {
        winText.gameObject.SetActive(false);
        gameWon = false;
    }
    
}
