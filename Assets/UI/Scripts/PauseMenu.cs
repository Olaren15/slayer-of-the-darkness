using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	private EventSystem eventSystem;
	private GameObject resumeButton;
	private GameManager gameManager;

	public void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		
		eventSystem = transform.parent.GetComponent<EventSystem>();
		resumeButton = transform.Find("Resume Button").gameObject;
		
		// register click events
		resumeButton.GetComponent<Button>().onClick.AddListener(gameManager.Resume);
		transform.Find("Quit Button").gameObject.GetComponent<Button>().onClick.AddListener(gameManager.Quit);
	}

	public void Show()
	{
		gameObject.SetActive(true);
		eventSystem.SetSelectedGameObject(resumeButton);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
