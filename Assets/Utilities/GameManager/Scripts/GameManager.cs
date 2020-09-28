using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static bool Paused => Time.timeScale == 0.0f;
	public static Controls controls;
	
	private void Awake()
	{
		controls = new Controls();
		controls.Enable();
	}
	
	public static void Pause()
	{
		Time.timeScale = 0.0f;
	}

	public static void Resume()
	{
		Time.timeScale = 1.0f;
	}

	public static void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}
}
