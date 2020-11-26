using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            UpdatePauseMenu();
            isPaused = value;
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
