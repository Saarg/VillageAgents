using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerDecision : MonoBehaviour, Decision {

	public float[] Decide (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
		float[] action = new float[2];
		action[0] = 0;
		action[1] = 0;

		if (state[2] < 20f && state[3] > 30f) {
			action[0] = 1;
			action[1] = 0;
		}

		if (state[2] > 80f) {
			action[0] = 1;
			action[1] = 1;
		}

		if (state[3] < 30f) {
			action[0] = 1;
			action[1] = 3;
		}

        return action;
	}

	public float[] MakeMemory (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[0];
    }
}
