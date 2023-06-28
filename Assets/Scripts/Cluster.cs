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

	public bool IsMember(ulong id) {
		return ClusterMembers.Any(clusterMember => clusterMember.IdMatches(id));
	}

	public int Size() {
		return ClusterMembers.Count;
	}
}