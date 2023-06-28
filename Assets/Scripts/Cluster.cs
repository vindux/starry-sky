using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cluster {
	public List<BodyObject> ClusterMembers { get; } = new();

	public Cluster(BodyObject bodyObject) {
		ClusterMembers.Add(bodyObject);
	}

	public void AddClusterMember(BodyObject bodyObject) {
		ClusterMembers.Add(bodyObject);
	}

	public void RemoveClusterMember(BodyObject bodyObject) {
		ClusterMembers.Remove(bodyObject);
	}

	// public void SpawnClusterObject(ulong id, GameObject newObjectPrefab) {
	// 	var clusterCenter = Vector3.zero;
	// 	foreach (var member in ClusterMembers) {
	// 		clusterCenter += member.GameObject.transform.position;
	// 		// Destroy the individual cluster member GameObjects
	// 		Destroy(member.GameObject);
	// 	}
	// 	clusterCenter /= ClusterMembers.Count;
	// 	
	// 	var newObject = Instantiate(newObjectPrefab, clusterCenter, Quaternion.identity);
	//
	// 	ClusterMembers.Clear();
	// 	ClusterMembers.Add(new BodyObject(id, newObject));
	// }


	public bool IsMember(ulong id) {
		return ClusterMembers.Any(clusterMember => clusterMember.IdMatches(id));
	}

	public int Size() {
		return ClusterMembers.Count;
	}
}