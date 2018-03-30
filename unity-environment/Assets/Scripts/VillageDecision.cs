using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageDecision : MonoBehaviour, Decision {

	public float[] Decide (
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
		float[] action = new float[3];

		action[0] = Time.frameCount;		
		action[1] = 0f;
		action[2] = 0f;

		if (vectorObs[0] < 0)
			action[1] = 1f;
		
		if (vectorObs[1] < 0)
			action[2] = 1f;

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
