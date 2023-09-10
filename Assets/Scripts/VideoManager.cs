using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {
	private VideoPlayer _superNovaVideo;
	public bool IsVideoPlaying { get; private set; }

	public float videoFadeSpeed = 0.5f;
	public int videoTimeoutSeconds = 10;

	private void Start() {
		_superNovaVideo = GetComponent<VideoPlayer>();
		_superNovaVideo.targetCameraAlpha = 0f;
	}
	
	public IEnumerator PlaySuperNovaVideo(BackgroundMusicManager backgroundMusicManager) {
		var videoEndTime = (float)_superNovaVideo.length - 4.0f;
		IsVideoPlaying = true;

		StartCoroutine(FadeIn());
		backgroundMusicManager.StopBackgroundMusic();
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(() => _superNovaVideo.time >= videoEndTime);

		backgroundMusicManager.PlayBackgroundMusic();
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(videoTimeoutSeconds);
		Debug.Log("Finished Super Nova video.");

		IsVideoPlaying = false;
	}

	public void PlayVideo() {
		StartCoroutine(FadeIn());
	}

	public void StopVideo() {
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeIn()
	{
		var fadeStartValue = 0f;
		var fadeEndValue = 1f;
		
		Debug.Log("Play Video");
		_superNovaVideo.targetCameraAlpha = 0f;
		_superNovaVideo.Play();
		
		while (fadeStartValue <= fadeEndValue) {
			fadeStartValue += Time.deltaTime * videoFadeSpeed;
			_superNovaVideo.targetCameraAlpha = fadeStartValue;

			yield return null;
		}
	}

	private IEnumerator FadeOut()
	{
		var fadeStartValue = 1f;
		var fadeEndValue = 0f;
		
		Debug.Log("Stop Video");
		while (fadeStartValue >= fadeEndValue) {
			fadeStartValue -= Time.deltaTime * videoFadeSpeed;
			_superNovaVideo.targetCameraAlpha = fadeStartValue;

			yield return null;
		}

		_superNovaVideo.targetCameraAlpha = 0f;
		_superNovaVideo.Stop();
	}
}