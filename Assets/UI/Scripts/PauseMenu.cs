using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
	public GameObject defaultSelection;

	private EventSystem eventSystem;
	private CanvasGroup canvasGroup;

	private void Start()
	{
		eventSystem = GetComponentInParent<EventSystem>();
		canvasGroup = GetComponent<CanvasGroup>();

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
		eventSystem.SetSelectedGameObject(defaultSelection);
		GameManager.Pause();
	}

	public void Hide()
	{
		canvasGroup.alpha = 0.0f;
		eventSystem.SetSelectedGameObject(null);
		GameManager.Resume();
	}

	public void Quit()
	{
		GameManager.Quit();
	}
}
