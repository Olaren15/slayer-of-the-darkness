using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool paused;

	public Controls controls;
	private PauseMenu pauseMenu;
	
	private void Awake()
	{
		// need to use FindObjectsOfTypeAll to find the menu because it is inactive on start
		pauseMenu = Resources.FindObjectsOfTypeAll<PauseMenu>().First();
		
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
	
	public void Pause()
	{
		pauseMenu.Show();
		Time.timeScale = 0.0f;
		paused = true;
	}

	public void Resume()
	{
		pauseMenu.Hide();
		Time.timeScale = 1.0f;
		paused = false;
	}

	public void Quit()
	{
		if (Application.isEditor)
		{
			EditorApplication.isPlaying = false;
		}
		else
		{
			Application.Quit();
		}
	}
}
