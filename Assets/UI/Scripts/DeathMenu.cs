using UnityEngine;
using UnityEngine.EventSystems;

public class DeathMenu : MonoBehaviour
{
	public GameObject defaultSelection;

	private EventSystem eventSystem;
	private CanvasGroup canvasGroup;

	private void Start()
	{
		eventSystem = GetComponentInParent<EventSystem>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Show()
	{
		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
		eventSystem.SetSelectedGameObject(defaultSelection);
	}

	//private void Restart()
	//{
	//	GameManager.Restart();
	//	Destroy(GameObject.Find("Player"));
	//}
}