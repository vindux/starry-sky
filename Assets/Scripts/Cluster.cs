using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cluster {
	public List<BodyObject> ClusterMembers { get; } = new();
	private Vector3 Center { get; set; }

	public Cluster(BodyObject bodyObject) {
		ClusterMembers.Add(bodyObject);
		UpdateCenter();
	}

	public void AddClusterMember(BodyObject bodyObject) {
		ClusterMembers.Add(bodyObject);
		UpdateCenter();
	}

	private void UpdateCenter() {
		var sum = ClusterMembers.Aggregate(Vector3.zero,
			(current, member) => current + member.GameObject.transform.position);

		Center = sum / ClusterMembers.Count;
	}

	public void AddClusterObject(GameObject newObjectPrefab) {
		foreach (var child in ClusterMembers.SelectMany(member => member.GetAllChildren())) {
			child.SetActive(false);
		}
		
		// > 5 cluster members = spawn video
		
		

		// Spawn new object at the center of the cluster
		var cluster = Object.Instantiate(newObjectPrefab, Center, Quaternion.identity);
		var num = (ulong)Random.Range(10000000, 99999999);
		
		cluster.name = "Cluster=" + ClusterMembers.Count + "_" + num;
	}

	public bool IsMember(ulong id) {
		return ClusterMembers.Any(clusterMember => clusterMember.IdMatches(id));
	}

	public int Size() {
		return ClusterMembers.Count;
	}
}