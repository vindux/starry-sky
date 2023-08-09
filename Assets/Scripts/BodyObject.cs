using System.Collections.Generic;
using UnityEngine;

public class BodyObject {
	public ulong ID { get; }
	public GameObject GameObject { get; }
	public GameObject BodyPrefab { get; }

	public BodyObject(ulong id, GameObject gameObject, GameObject bodyPrefab) {
		ID = id;
		GameObject = gameObject;
		BodyPrefab = bodyPrefab;
		GameObject.AddComponent<Hexagon>();
		InstantiateChild();
	}
	
	public void InstantiateChild() {
		var jointObj = Object.Instantiate(BodyPrefab, GameObject.transform, false);
		jointObj.name = "Head";
	} 

	public bool IdMatches(ulong id) {
		return id == ID;
	}

	public int GetChildCount() {
		return GameObject.transform.childCount;
	}

	public IEnumerable<GameObject> GetAllChildren() {
		var childCount = GameObject.transform.childCount;
		List<GameObject> children = new();
		
		for (var i = 0; i < childCount; i++) {
			var childTransform = GameObject.transform.GetChild(i);
			var childObject = childTransform.gameObject;
			children.Add(childObject);
		}

		return children;
	}
}