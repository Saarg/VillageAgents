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
					if (Work == null) {
						curAction = AgentActions.Wander;
						GetComponentInChildren<Renderer>().material.color = Color.green;						
					} else {
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

		if (Work == null) {
			village.jobOffers.ForEach(job => {
				if (!job.Contains(this)) {
					JobApplications application = new JobApplications();
					application.agent = this;
					application.interest = (strength/100f * 0.5f) + (happiness/100f + 0.25f) + (1/(job.workplace.transform.position - transform.position).sqrMagnitude);
					
					job.applications.Add(application);
				}
			});
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
	}

	void SetWork(GameObject workplace){
		Work = workplace;

		if (curAction == AgentActions.Wander || curAction == AgentActions.Work) {
			curAction = AgentActions.Work;			
			nma.SetDestination(Work.transform.position);
			GetComponentInChildren<Renderer>().material.color = Work.GetComponent<Factory>().workerColor;
		}
	}

	public bool HasWork() {
		return Work != null;
	}
}
