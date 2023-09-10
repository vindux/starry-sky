using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BodyComparer : MonoBehaviour {
	public static List<Cluster> GetCloseGroups(List<BodyObject> bodies, float minDistance, GameObject clusterPrefab) {
		var closeGroups = new List<Cluster>();

		var clusterObjects = FindObjectsOfType<GameObject>()
			.Where(go => go.name.Contains("Cluster"))
			.ToArray();
		foreach (var cluster in clusterObjects) {
			Destroy(cluster);
		}

		foreach (var body1 in bodies) {
			var existingCluster = closeGroups.FirstOrDefault(cluster => cluster.IsMember(body1.ID));
			Cluster currentCluster;

			if (existingCluster == null) {
				currentCluster = new Cluster(body1);
				closeGroups.Add(currentCluster);
			}
			else {
				currentCluster = existingCluster;
			}

			foreach (var body2 in bodies.Where(body2 => body1 != body2)) {
				if (Vector3.Distance(body1.GameObject.transform.position, body2.GameObject.transform.position) >
				    minDistance) continue;

				var existingCluster2 = closeGroups.FirstOrDefault(cluster => cluster.IsMember(body2.ID));
				if (existingCluster2 != null && currentCluster != existingCluster2) {
					foreach (var member in existingCluster2.ClusterMembers) {
						currentCluster.AddClusterMember(member);
					}

					closeGroups.Remove(existingCluster2);
				}

				if (!currentCluster.IsMember(body2.ID)) {
					currentCluster.AddClusterMember(body2);
				}
			}
		}

		foreach (var cluster in closeGroups) {
			if (cluster.Size() == 1) {
				foreach (var member in cluster.ClusterMembers) {
					foreach (var child in member.GetAllChildren()) {
						child.SetActive(true);
					}
				}
			}
			else {
				cluster.AddClusterObject(clusterPrefab);
			}
		}

		var removed = closeGroups.RemoveAll(cluster => cluster.Size() == 1);
		return closeGroups;
	}
}