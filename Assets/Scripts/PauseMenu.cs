using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;

    private void Start()
    {
        IsPaused = false;
    }
    
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            isPaused = value;
            UpdatePauseMenu();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
        }
    }

    private void UpdatePauseMenu()
    {
        pauseMenu.SetActive(IsPaused);
    }

    public void Resume()
    {
        IsPaused = !IsPaused;
    }
    
}
