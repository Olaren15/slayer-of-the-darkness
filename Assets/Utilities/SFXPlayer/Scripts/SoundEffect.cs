using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName = "ScriptableObjects/SoundEffect", order = 1)]
public class SoundEffect : ScriptableObject
{
	public AudioClip soundToPlay;
}
