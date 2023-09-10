using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
	private AudioSource _audioSource;
	public float fadeDuration = 1.0f;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.volume = 0.0f; // Start with volume at 0
		PlayBackgroundMusic();
	}

	public void PlayBackgroundMusic()
	{
		StartCoroutine(FadeIn());
	}

	public void StopBackgroundMusic()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeIn()
	{
		// _audioSource.Play();
		var startVolume = _audioSource.volume;
		var timer = 0.0f;
		while (timer < fadeDuration)
		{
			_audioSource.volume = Mathf.Lerp(startVolume, 1.0f, timer / fadeDuration);
			timer += Time.deltaTime;
			yield return null;
		}
		_audioSource.volume = 1.0f;
	}

	private IEnumerator FadeOut()
	{
		var startVolume = _audioSource.volume;
		var timer = 0.0f;
		while (timer < fadeDuration)
		{
			_audioSource.volume = Mathf.Lerp(startVolume, 0.0f, timer / fadeDuration);
			timer += Time.deltaTime;
			yield return null;
		}
		_audioSource.volume = 0.0f;
		// _audioSource.Stop();
	}
}