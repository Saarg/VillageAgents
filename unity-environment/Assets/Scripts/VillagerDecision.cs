using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerDecision : MonoBehaviour, Decision {

	public float[] Decide (
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
		float[] action = new float[1];

		action[0] = (float) AgentActions.Wander;
		if (vectorObs[(int) AgentStates.Strength] < 30f) {
			action[0] = (float) AgentActions.Rest;			
		} else if (vectorObs[(int) AgentStates.Happiness] < 30f) {
			action[0] = (float) AgentActions.Fun;			
		} else if (vectorObs[(int) AgentStates.Strength] > 80f && vectorObs[(int) AgentStates.CurAction] != (float) AgentActions.Fun || vectorObs[(int) AgentStates.CurAction] == (float) AgentActions.Work){
			action[0] = (float) AgentActions.Work;
		} else if (vectorObs[(int) AgentStates.Happiness] > 80f){
			action[0] = (float) AgentActions.Rest;
		}

        return action;
	}

	public List<float> MakeMemory(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        return new List<float>();
    }
}
