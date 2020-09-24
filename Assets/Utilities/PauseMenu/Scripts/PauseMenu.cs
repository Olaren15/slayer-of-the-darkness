using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused;
    public GameObject pauseMenuUI;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Enable();
        controls.Interface.Pause.performed += context => TogglePauseMenu();
    }

    private void TogglePauseMenu()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        paused = true;
    }
}
