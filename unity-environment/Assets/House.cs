using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

	public int capacity = 4;

	[SerializeField]
	List<VillagerAgent> houseOwners = new List<VillagerAgent>();
	
	List<VillagerAgent> agents = new List<VillagerAgent>();

	public bool IsFull { get { 
		agents.RemoveAll(item => item == null); 
		houseOwners.RemoveAll(item => item == null); 
		return capacity <= houseOwners.Count; 
	} }

	public bool AddOwner(VillagerAgent a) {
		if (capacity > houseOwners.Count) {
			houseOwners.Add(a);
			a.Home = gameObject;
			return true;
		} else {
			return false;
		}
	}

	public void RemoveOwner(VillagerAgent a) {
		houseOwners.Remove(a);
	}

	void OnTriggerEnter(Collider col) {
		VillagerAgent agent = col.GetComponentInParent<VillagerAgent>();
		if (agent != null) {
			agent.curState = VillagerAgent.AgentState.AtHome;
			//agent.enabled = true;

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
