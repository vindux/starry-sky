using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hexagon : MonoBehaviour {
	private const float Minx = -8.5f;
	private const float MaxX = 8.5f;
	private const float MinY = -4.5f;
	private const float MaxY = 4.5f;
	public float moveSpeed = 1.0f;
	private Vector3 _targetPosition;

	// Start is called before the first frame update
	private void Start() {
		var parent = transform.parent;
		if (parent == null || moveSpeed == 0.0f) return;
		_targetPosition = GetRandomPosition();
	}

	// Update is called once per frame
	private void Update() {
		var parent = transform.parent;
		if (parent == null || moveSpeed == 0.0f) return;
		parent.position = Vector3.MoveTowards(parent.position, _targetPosition, moveSpeed * Time.deltaTime);

		if (transform.parent.position == _targetPosition) {
			_targetPosition = GetRandomPosition();
		}
	}

	private Vector3 GetRandomPosition() {
		var randomX = Random.Range(Minx, MaxX);
		var randomY = Random.Range(MinY, MaxY);
		return new Vector3(randomX, randomY, transform.parent.position.z);
	}
}