using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

	[SerializeField]
	float prodPerAgent = 1;

	[SerializeField]
	List<VillagerAgent> agents = new List<VillagerAgent>();

	public Color workerColor = Color.blue;

	public float GetProd() {
		agents.RemoveAll(item => item == null);

		return prodPerAgent * agents.Count;
	}

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
