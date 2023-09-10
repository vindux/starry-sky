using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Joint = Windows.Kinect.Joint;
using Random = UnityEngine.Random;

public class BodyView : MonoBehaviour {
	// public float scaleFactorAll = 10;
	public float ceilingSizeX = 5;
	public float ceilingSizeY = 2.8125f;
	public float minDistance = 1.00f;

	private const float CamSizeX = 17.77568f;
	private const float CamSizeY = 5.0f;
	private TextMeshPro _textMeshPro;
	private bool isVideoStarting = false;
	private bool isVideoPlaying = false;

	public BodyManager bodyManager;
	public GameObject clusterPrefab;
	public GameObject bodyPrefab;
	public VideoPlayer superNovaVideo;
	public float videoFadeSpeed = 0.5f;
	public int videoTimeoutSeconds = 10;

	// private readonly Dictionary<ulong, GameObject> _bodies = new();
	private readonly List<BodyObject> _bodies = new();

	private readonly List<JointType> _joints = new() {
		JointType.Head
	};

	private bool _isBodyManagerNull;

	private void Start() {
		_isBodyManagerNull = bodyManager == null;
		_textMeshPro = GetComponentInChildren<TextMeshPro>();
		superNovaVideo.gameObject.SetActive(false);
	}

	private void Update() {
		if (_isBodyManagerNull) return;

		var data = bodyManager.GetData();
		if (data == null) {
			// return;
		}

		if (data != null) {
			var trackedIds = (data.Where(body => body != null)
				.Where(body => body.IsTracked)
				.Select(body => body.TrackingId)).ToList();
			var knownIds = _bodies.Select(body => body.ID).ToList();

			// delete untracked bodies
			foreach (var trackingId in knownIds.Where(trackingId => !trackedIds.Contains(trackingId))) {
				foreach (var body in _bodies.ToList().Where(body => body.IdMatches(trackingId))) {
					Destroy(body.GameObject);
					_bodies.Remove(body);
				}
			}

			knownIds = _bodies.Select(body => body.ID).ToList();
			foreach (var body in data) {
				if (body == null) continue;
				if (!body.IsTracked) continue;
				if (!knownIds.Contains(body.TrackingId)) {
					_bodies.Add(CreateBodyObject(body.TrackingId));
				}

				var bodyObject = _bodies.FirstOrDefault(bodyObject => bodyObject.IdMatches(body.TrackingId));
				RefreshBodyObject(body, bodyObject);
			}
		}

		CheckKeyInput();
		VisualizeClusters();
	}

	private void CheckKeyInput() {
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		var num = (ulong)Random.Range(10000000, 99999999);
		_bodies.Add(CreateBodyObject(num));
	}

	private void VisualizeClusters() {
		var clusters = BodyComparer.GetCloseGroups(_bodies, minDistance, clusterPrefab);
		// Debug.Log("Cluster size = " + clusters.Count);
		if (_textMeshPro != null) {
			var text = "\nBodies = " + _bodies.Count + "\nClusters = " + clusters.Count;
			_textMeshPro.text = text;
		}

		for (var index = 0; index < clusters.Count; index++) {
			// Debug.Log("cluster [" + index + "] size = " + cluster.Size());
			var cluster = clusters[index];
			var bodies = cluster.ClusterMembers;

			if (bodies.Count >= 5 && !isVideoPlaying) {
				Debug.Log("Playing video");
				StartCoroutine(PlaySuperNovaVideo());
				
				return;
			}

			foreach (var body1 in bodies) {
				foreach (var body2 in bodies.Where(body => body1 != body)) {
					var start = body1.GameObject.transform.position;
					var end = body2.GameObject.transform.position;
					Debug.DrawLine(start, end, Color.red, Time.deltaTime, false);
				}
			}
		}
	}

	private IEnumerator PlaySuperNovaVideo() {
		isVideoPlaying = true;
		superNovaVideo.gameObject.SetActive(true);
		// superNovaVideo.Play();

		StartCoroutine(FadeVideoPlayerAlpha(superNovaVideo, FadeDirection.In, videoFadeSpeed));
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(() => !superNovaVideo.isPlaying);

		StartCoroutine(FadeVideoPlayerAlpha(superNovaVideo, FadeDirection.Out, videoFadeSpeed));
		yield return new WaitForSeconds(videoTimeoutSeconds);
		Debug.Log("Finished Super Nova video.");
		
		isVideoPlaying = false;
	}

	private IEnumerator FadeVideoPlayerAlpha(VideoPlayer video, FadeDirection direction, float speed) {
		if (video.renderMode is not (VideoRenderMode.CameraNearPlane
		    or VideoRenderMode.CameraFarPlane
		    or VideoRenderMode.RenderTexture)) yield break;
	
		RawImage rawImage = null;
		if (video.renderMode == VideoRenderMode.RenderTexture) {
			video.gameObject.TryGetComponent(out rawImage);
			if (!rawImage) {
				Debug.LogWarning($"No RawImage on the VideoPlayer GameObject found. -> ({video.gameObject.name})");
			}
		}

		var alpha = direction == FadeDirection.Out ? 1f : 0f;
		var fadeEndValue = direction == FadeDirection.Out ? 0f : 1f;

		if (direction == FadeDirection.Out) {
			Debug.Log("Stop Video");
			while (alpha >= fadeEndValue) {
				alpha -= Time.deltaTime * speed;
				if (rawImage) {
					rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
				}
				else {
					video.targetCameraAlpha = alpha;
				}

				yield return null;
			}

			video.Stop();
			if (rawImage) rawImage.enabled = false;
		}
		else {
			Debug.Log("Play Video");

			//Make sure alpha is 0
			if (rawImage) {
				rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
			}
			else {
				video.targetCameraAlpha = alpha;
			}

			//Enable the RawImage and start the player
			if (rawImage) {
				rawImage.enabled = true;
			}

			video.Play();

			//Delay - to make sure the Image has the correct Texture
			yield return new WaitForSeconds(0.1f);

			while (alpha <= fadeEndValue) {
				alpha += Time.deltaTime * speed;
				if (rawImage) {
					rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
				}
				else {
					video.targetCameraAlpha = alpha;
				}

				yield return null;
			}
		}
	}


	private BodyObject CreateBodyObject(ulong id, float x = 0, float y = 0) {
		var body = new GameObject("body=" + id) {
			transform = {
				position = new Vector3(x, y, 0)
			}
		};

		return new BodyObject(id, body, bodyPrefab);
	}

	private void RefreshBodyObject(Body body, BodyObject bodyObject) {
		var bodyObjectGameObject = bodyObject.GameObject;
		foreach (var joint in _joints) {
			var sourceJoint = body.Joints[joint];
			var targetPosition = Map3DTo2D(sourceJoint);
			targetPosition.z = 0;

			var jointObj = bodyObjectGameObject.transform.Find(joint.ToString());
			jointObj.position = targetPosition;
		}
	}

	private Vector3 Map3DTo2D(Joint joint) {
		// x == joint.z
		// y == joint.x
		// z == 0

		var xPos = -8.88784f + (joint.Position.Z / ceilingSizeX * CamSizeX);
		var yPos = joint.Position.X / ceilingSizeY * CamSizeY;
		const float zPos = 0f;

		return new Vector3(xPos, yPos, zPos);
	}
}