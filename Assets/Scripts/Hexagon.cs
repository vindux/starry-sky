using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hexagon : MonoBehaviour {
	private const float Minx = -8.5f;
	private const float MaxX = 8.5f;
	private const float MinY = -4.5f;
	private const float MaxY = 4.5f;
	public float moveSpeed = 0.35f;
	private Vector3 _targetPosition;

	private void Start() {
		if (moveSpeed == 0.0f) return;
		_targetPosition = GetRandomPosition();
	}

	private void Update() {
		if ( moveSpeed == 0.0f) return;
		transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

		if (transform.position == _targetPosition) {
			_targetPosition = GetRandomPosition();
		}
	}

	private Vector3 GetRandomPosition() {
		var randomX = Random.Range(Minx, MaxX);
		var randomY = Random.Range(MinY, MaxY);
		return new Vector3(randomX, randomY, transform.position.z);
	}
}