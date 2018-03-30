using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentStates {Strength, Happiness, CurAction}
public enum AgentActions {Wander, Rest, Work, Fun}

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerAgent : Agent {

	static List<VillagerAgent> agents = new List<VillagerAgent>();
	public static int AgentCount { get { return agents.Count; }}
	public static float AvgHappiness { get { 
		if (agents.Count == 0) return 0;

		float avgHap = 0;

		foreach (VillagerAgent a in agents) {
			avgHap += a.happiness;
		}

		return avgHap / agents.Count;
	}}

	NavMeshAgent nma;

	public VillageAgent village;

	[Header("Specific to Villagers")]
	public GameObject Home;
	public GameObject Work;
	public GameObject Bar;

	[Range(0, 100f)]
	public float strength = 100f;
	[Range(0, 100f)]
	public float happiness = 50f;

	[SerializeField]
	AgentActions curAction;

	public override void InitializeAgent() {
		nma = GetComponent<NavMeshAgent>();

		agents.Add(this);

		if (Home != null)
			Home.GetComponent<House>().AddOwner(this);

		curAction = AgentActions.Wander;
	}

    public override void CollectObservations()
    {
		AddVectorObs((float) strength);
        AddVectorObs((float) happiness);
        AddVectorObs((float) curAction);
    }

    public override void AgentAction(float[] act, string textAction) {
		switch (curAction) {
			case AgentActions.Rest:
				if ((transform.position - nma.pathEndPosition).sqrMagnitude < 1f) {
					strength += Time.deltaTime;
				}
				break;
			case AgentActions.Work:
				if ((transform.position - nma.pathEndPosition).sqrMagnitude < 1f) {
					strength -= Time.deltaTime;
					happiness -= Time.deltaTime;
				}
				break;
			case AgentActions.Fun:
				if ((transform.position - nma.pathEndPosition).sqrMagnitude < 1f) {
					happiness += 1.5f * Time.deltaTime;
				}
				break;
			case AgentActions.Wander:
				strength -= Time.deltaTime;
				happiness -= 2f * Time.deltaTime;
				break;
		}

		if (act[0] > 0) {
			curAction = (AgentActions) act[0];

			switch (curAction) {
				case AgentActions.Rest:
					nma.SetDestination(Home.transform.position);

					Work = null;
					GetComponentInChildren<Renderer>().material.color = Color.red;
					break;
				case AgentActions.Work:
					if (village.jobOffers.Count <= 0 && Work == null) {
						curAction = AgentActions.Wander;
						GetComponentInChildren<Renderer>().material.color = Color.green;						
					} else {
						Work = Work == null ? village.jobOffers.Dequeue() : Work;
						nma.SetDestination(Work.transform.position);
						GetComponentInChildren<Renderer>().material.color = Work.GetComponent<Factory>().workerColor;						
					}
					break;
				case AgentActions.Fun:
					nma.SetDestination(Bar.transform.position);
					GetComponentInChildren<Renderer>().material.color = Color.yellow;												
					break;
				case AgentActions.Wander:
					nma.path = null;
					GetComponentInChildren<Renderer>().material.color = Color.green;											
					break;
			}
		}
    }

    // to be implemented by the developer
    public override void AgentReset() {
        
    }

	public override void AgentOnDone() {

	}

	void Update() {
		switch (curAction) {
			case AgentActions.Wander:
				Wander ();
				break;
		}
	}

	void Wander () {
		nma.SetDestination (transform.position + Quaternion.AngleAxis (Random.Range (-20f, 20f), Vector3.up) * transform.forward * nma.speed);

		if (village.jobOffers.Count > 0) {
			curAction = AgentActions.Work;
			Work = village.jobOffers.Dequeue();
			nma.SetDestination(Work.transform.position);
			GetComponentInChildren<Renderer>().material.color = Work.GetComponent<Factory>().workerColor;						
		}
	}
}
