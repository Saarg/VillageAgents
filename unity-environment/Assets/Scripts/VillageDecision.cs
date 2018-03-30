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
		float[] action = new float[0];

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
