using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Kinect = Windows.Kinect;

public class BodyView : MonoBehaviour {
	// public float scaleFactorAll = 10;
	public float ceilingSizeX = 5;
	public float ceilingSizeY = 2.8125f;

	public float minDistance = 0.75f;


	private const float CamSizeX = 17.77568f;
	private const float CamSizeY = 5.0f;


	public BodyManager bodyManager;
	public GameObject jointObject;

	// private readonly Dictionary<ulong, GameObject> _bodies = new();
	private readonly List<BodyObject> _bodies = new();

	private readonly List<Kinect.JointType> _joints = new() {
		Kinect.JointType.Head
	};

	private bool _isbodyManagerNull;

	private void Start() {
		Debug.Log("starting bodyview");
		_isbodyManagerNull = bodyManager == null;
	}

	private void Update() {
		if (_isbodyManagerNull) return;

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
				foreach (var body in _bodies.Where(body => body.IdMatches(trackingId))) {
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

		if (Input.GetKeyDown(KeyCode.Space)) {
			var num = (ulong)Random.Range(100, 10000000);
			_bodies.Add(CreateBodyObject(num));
		}

		var clusters = BodyComparer.GetCloseGroups(_bodies);
		// Debug.Log("Cluster size = " + clusters.Count);

		for (var index = 0; index < clusters.Count; index++) {
			// Debug.Log("cluster [" + index + "] size = " + cluster.ClusterMembers.Count);
			var cluster = clusters[index];
			var bodies = cluster.ClusterMembers;

			foreach (var body1 in bodies) {
				foreach (var body2 in bodies.Where(body => body1 != body)) {
					var start = body1.GameObject.transform.position;
					var end = body2.GameObject.transform.position;

					// Debug.Log(start + "  TO  " + end);
					Debug.DrawLine(start, end, Color.red, Time.deltaTime, false);
				}
			}
		}
	}

	private BodyObject CreateBodyObject(ulong id) {
		var body = new GameObject("body=" + id);

		var jointObj = Instantiate(jointObject);
		foreach (var joint in _joints) {
			jointObj.name = joint.ToString();
			jointObj.transform.parent = body.transform;
		}

		return new BodyObject(id, body);
	}

	private void RefreshBodyObject(Kinect.Body body, BodyObject bodyObject) {
		var bodyObjectGameObject = bodyObject.GameObject;
		foreach (var joint in _joints) {
			var sourceJoint = body.Joints[joint];
			var targetPosition = Map3DTo2D(sourceJoint);
			targetPosition.z = 0;

			var jointObj = bodyObjectGameObject.transform.Find(joint.ToString());
			jointObj.position = targetPosition;
		}
	}

	private Vector3 Map3DTo2D(Kinect.Joint joint) {
		// x == joint.z
		// y == joint.x
		// z == 0

		var xPos = -8.88784f + (joint.Position.Z / ceilingSizeX * CamSizeX);
		var yPos = joint.Position.X / ceilingSizeY * CamSizeY;
		const float zPos = 0f;

		return new Vector3(xPos, yPos, zPos);
	}
}