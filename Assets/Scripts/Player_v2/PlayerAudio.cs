using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip groundedSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip dashSound;

	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
	}

	public void PlayGroundSound()
	{
        audioSource.clip = groundedSound;
        audioSource.Play();
	}

    public void PlayJumpSound()
	{
        audioSource.clip = jumpSound;
        audioSource.Play();
	}

    public void PlayDashSound()
	{
        audioSource.clip = dashSound;
        audioSource.Play();
	}
}
