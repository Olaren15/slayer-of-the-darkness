using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
	private EventSystem eventSystem;
	private GameObject resumeButton;

	private void Start()
	{
		eventSystem = GetComponentInParent<EventSystem>();
		resumeButton = transform.Find("Resume Button").gameObject;

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
		transform.localScale = new Vector3(1, 1, 1);
		eventSystem.SetSelectedGameObject(resumeButton);
		
		GameManager.Pause();
	}

	public void Hide()
	{
		transform.localScale = new Vector3(0, 0, 0);
		GameManager.Resume();
	}

	public void Quit()
	{
		GameManager.Quit();
	}
}
