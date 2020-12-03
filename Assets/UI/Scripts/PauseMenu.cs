using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject defaultSelection;

	private EventSystem eventSystem;
	private CanvasGroup canvasGroup;

	private MusicPlayer music;

	private void Start()
	{
		eventSystem = GetComponentInParent<EventSystem>();
		canvasGroup = GetComponent<CanvasGroup>();
		music = FindObjectOfType<MusicPlayer>();

		GameManager.controls.Interface.Pause.performed += context => TogglePauseMenu();
	}

	private void TogglePauseMenu()
	{
		if (GameManager.Paused)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	private void Show()
	{
		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
		eventSystem.SetSelectedGameObject(defaultSelection);
		GameManager.Pause();
	}

	public void Hide()
	{
		canvasGroup.alpha = 0.0f;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
		eventSystem.SetSelectedGameObject(null);
		GameManager.Resume();
	}

	public void Restart()
	{
		GameManager.Restart();
		Destroy(GameObject.Find("Player"));
	}

	public void Quit()
	{
		GameManager.Quit();
		Destroy(GameObject.Find("Player"));
		Destroy(GameObject.Find("Main Music"));
	}

	public void GoBackToMainMenu()
	{
		GameManager.Restart();
		SceneManager.LoadScene(0);
		if (music != null)
		{
			music.RestartMusicPlayer();
		}
		Destroy(GameObject.Find("Player"));
		Destroy(GameObject.Find("Main Music"));
	}
}