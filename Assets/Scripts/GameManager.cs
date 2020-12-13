using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    //Stores reference to UI elements
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;
    public bool gameWon;
    public List<Cell> validCheckCells;
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
        //Property to check if it's white's turn
        get => isWhiteTurn;
        set
        {
            isWhiteTurn = value;
            if (!isWhiteTurn)
            {
                bm.EndTurn();
            }
        }
    }

    public void UpdateScoreText()
    {
        //Sets the UI text
        scoreText.text = $"({bm.ai.movePos.x},{bm.ai.movePos.y}) : {bm.ai.score}";
    }

    void Start()
    {
        //Initialises the board at the start of the game
        board.InitBoard();
    }

    private void Update()
    {
        //If the game is over
        if (gameWon)
        {
            //And enter is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //Reset the board
                bm.ResetBoard();
            }
        }
    }

    public void Win(bool isWhite)
    {
        //Sets the UI if a player wins
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
