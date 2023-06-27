using UnityEngine;

public class BodyObject {
	public ulong ID { get; }
	public GameObject GameObject { get; }

	public BodyObject(ulong id, GameObject gameObject) {
		ID = id;
		GameObject = gameObject;
	}

	public bool IdMatches(ulong id) {
		return id == ID;
	}
}