using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
  public static bool Paused => Time.timeScale == 0.0f;
  public static Controls controls;

  private bool hideCursor;

  private void Awake()
  {
    controls = new Controls();
    controls.Enable();
  }

  private void Update()
  {
    if (Gamepad.current?.wasUpdatedThisFrame == true)

    {
      hideCursor = true;
    }

    if (Keyboard.current?.wasUpdatedThisFrame == true || Mouse.current?.wasUpdatedThisFrame == true)
    {
      hideCursor = false;
    }

    Cursor.visible = !hideCursor;
  }

  public static void Pause()
  {
    Time.timeScale = 0.0f;
    controls.Player.Disable();
  }

  public static void Resume()
  {
    Time.timeScale = 1.0f;
    controls.Player.Enable();
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
