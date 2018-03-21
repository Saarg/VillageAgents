using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

	[SerializeField]
	List<VillagerAgent> agents = new List<VillagerAgent>();

	void OnTriggerEnter(Collider col) {
		VillagerAgent agent = col.GetComponentInParent<VillagerAgent>();
		if (agent != null) {
			

			agents.Add(agent);
		}
	}

	void OnTriggerExit(Collider col) {
		VillagerAgent agent = col.GetComponentInParent<VillagerAgent>();
		if (agent != null) {
			agents.Remove(agent);
		}
	}
}
