using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerDecision : MonoBehaviour, Decision {

	public float[] Decide (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
		float[] action = new float[1];

		action[0] = (float) AgentActions.Wander;
		if (state[(int) AgentStates.Strength] < 30f) {
			action[0] = (float) AgentActions.Rest;			
		} else if (state[(int) AgentStates.Happiness] < 30f) {
			action[0] = (float) AgentActions.Fun;			
		} else if (state[(int) AgentStates.Strength] > 80f){
			action[0] = (float) AgentActions.Work;
		} else if (state[(int) AgentStates.Happiness] > 80f){
			action[0] = (float) AgentActions.Rest;
		}

        return action;
	}

	public float[] MakeMemory (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[0];
    }
}
