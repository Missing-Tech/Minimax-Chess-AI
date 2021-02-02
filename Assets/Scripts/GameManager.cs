using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;
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
            if (!isWhiteTurn)
            {
                bm.EndTurn();
            }
        }
    }

    public void UpdateScoreText(float score = -10101)
    {
        //If the value of score hasn't changed
        if (score == -10101)
        {
            score = bm.ai.score;
        }
        scoreText.text = $"({bm.ai.movePos.x},{bm.ai.movePos.y}) : {score}";
    }

    void Start()
    {
        board.InitBoard();
    }

    private void Update()
    {
        if (gameWon)
        {
            //Resets the board if the user presses enter after the game has been won
            if (Input.GetKeyDown(KeyCode.Return))
            {
                bm.ResetBoard();
                UpdateScoreText(0);
                bm.canMove = true;
            }
        }
    }

    public void Win(bool isWhite)
    {
        winText.gameObject.SetActive(true);
        winText.text = isWhite ? "BLACK WINS" : "WHITE WINS";
        bm.canMove = false;
        gameWon = true;
    }

    public void Reset()
    {
        winText.gameObject.SetActive(false);
        gameWon = false;
    }
    
}
