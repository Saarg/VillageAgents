using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerAgent : Agent {

	static List<VillagerAgent> agents = new List<VillagerAgent>();
	public static int AgentCount { get { return agents.Count; }}
	public static float AvgHappiness { get { 
		float avgHap = 0;

		foreach (VillagerAgent a in agents) {
			avgHap += a.happiness;
		}

		return avgHap / agents.Count;
	}}

	NavMeshAgent nma;

	[Header("Specific to Villagers")]
	public GameObject Villager;
	public GameObject Home;
	public GameObject Work;
	public GameObject Bar;

	[Range(0, 100f)]
	public float strength = 100f;
	[Range(0, 100f)]
	public float happiness = 50f;

	public enum AgentState {AtHome, AtWork, InTransit, AtBar}
	public AgentState curState;
	public AgentState nextState;

	public override void InitializeAgent() {
		nma = Villager.GetComponent<NavMeshAgent>();

		agents.Add(this);

		if (Home != null)
			Home.GetComponent<House>().AddOwner(this);

		maxStep = Random.Range(800, 1000);
	}

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

        state.Add((float) curState);
        state.Add((float) nextState);
        state.Add((float) strength);
        state.Add((float) happiness);

        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
		if (act[0] == 1f) {
			nextState = (AgentState) act[1];
		}

		if (curState != nextState) {
			switch (nextState) {
				case AgentState.AtHome:
					curState = AgentState.InTransit;
					nma.destination = Home.transform.position;
					break;
				case AgentState.AtWork:
					curState = AgentState.InTransit;
					nma.destination = Work.transform.position;
					break;
				case AgentState.AtBar:
					curState = AgentState.AtBar;
					nma.destination = Bar.transform.position;
					break;
				case AgentState.InTransit:
					break;
			}
		}
		
		switch (curState) {
			case AgentState.AtHome:
				strength++;
				break;
			case AgentState.AtWork:
				strength--;
				happiness--;
				break;
			case AgentState.AtBar:
				strength--;
				happiness++;
				break;
			case AgentState.InTransit:
				//enabled = false;
				break;
		}
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
        
    }

	public override void AgentOnDone()
	{
		agents.RemoveAll(item => item == null);
		agents.Remove(this);

		Home.GetComponent<House>().RemoveOwner(this);

		Destroy(gameObject);
	}
}
