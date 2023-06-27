using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heaxong : MonoBehaviour {
	public Transform mesh;
	public float updateLerpDelta = 15.0f;
	

	// Start is called before the first frame update
	private void Start() {
	}

	// Update is called once per frame
	private void Update() {
		mesh.position = Vector3.Lerp(mesh.position, transform.position, Time.deltaTime * updateLerpDelta);
	}
}