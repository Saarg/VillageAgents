using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

	[SerializeField]
	VillageAcademy villageAcademy;

	[SerializeField]
	GameObject housePrefab;
	[SerializeField]
	GameObject farmPrefab;
	[SerializeField]
	GameObject factoryPrefab;
	[SerializeField]
	GameObject pubPrefab;

	[SerializeField]
	Camera camera;

	[SerializeField]
	Transform village;		

	[SerializeField]
	GameObject building;
	[SerializeField]
	LayerMask buildLayer;
	
	// Update is called once per frame
	void Update () {
		if (building != null) {
			RaycastHit hit;

			if (Physics.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), camera.transform.forward, out hit, 100f, buildLayer)) {
				building.transform.position = hit.point;
			}

			if (Input.GetMouseButton(0)) {
				if (building.name.Contains(housePrefab.name)) {
					villageAcademy.AddHouse(building.GetComponent<House>());
				} else if (building.name.Contains(farmPrefab.name)) {
					villageAcademy.AddFarm(building.GetComponent<Factory>());
				} else if (building.name.Contains(factoryPrefab.name)) {
					villageAcademy.AddCoalPlant(building.GetComponent<Factory>());
				} else if (building.name.Contains(pubPrefab.name)) {
					villageAcademy.AddBar(building.GetComponent<Bar>());
				}

				building = null;
			} else if (Input.GetKeyDown(KeyCode.Escape)) {
				Destroy(building);
			}
		}
	}

	public void StartBuildingHouse() {
		building = Instantiate(housePrefab, village);
	}

	public void StartBuildingFarm() {
		building = Instantiate(farmPrefab, village);
	}

	public void StartBuildingFactory() {
		building = Instantiate(factoryPrefab, village);
	}

	public void StartBuildingPub() {
		building = Instantiate(pubPrefab, village);
	}
}
