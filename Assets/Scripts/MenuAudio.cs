using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
	[SerializeField] public AudioSource sfxPlayer;
	[SerializeField] public List<AudioClip> sfxList;

	public void PlaySound(int index)
	{
		sfxPlayer.clip = sfxList[index];
		sfxPlayer.Play();
	}
}
