using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageAgent : Agent {

	[SerializeField]
    List<House> houses = new List<House>();
    public void AddHouse(House house) { houses.Add(house); }
    public void RemoveHouse(House house) { houses.Remove(house); }

    [SerializeField]
    List<Factory> farms;
    public void AddFarm(Factory farm) { farms.Add(farm); }
    public void RemoveFarm(Factory farm) { farms.Remove(farm); }
    [SerializeField]	
    float foodProd;
    [SerializeField]
    float food = 50;

    [SerializeField]
    List<Factory> coalPlants = new List<Factory>();
    public void AddCoalPlant(Factory coalPlant) { coalPlants.Add(coalPlant); }
    public void RemoveCoalPlant(Factory coalPlant) { coalPlants.Remove(coalPlant); }
    [SerializeField]	
    float coalProd;
    [SerializeField]
    float coal = 50;

    [SerializeField]
    List<Bar> bars = new List<Bar>();
    public void AddBar(Bar bar) { bars.Add(bar); }
    public void RemoveBar(Bar bar) { bars.Remove(bar); }
    [SerializeField]
    float happiness = 50;
    [SerializeField]
	Brain villagerBrain;	
    [SerializeField]
    GameObject villagerPrefab;
    [SerializeField]
    [Range(0, 100)]
    float babyProgress = 0f;

    [Header("UI")]
    [SerializeField]
    Image coalUI;
    [SerializeField]
    Image foodUI;
    [SerializeField]
    Image happinessUI;
    [SerializeField]
    Image reproducitonUI;
    [SerializeField]
    Text population;
    [SerializeField]
    Slider timeUI;

	public Queue<GameObject> jobOffers = new Queue<GameObject>();

	public override void InitializeAgent() {
		
	}

    public override void CollectObservations()
    {
		AddVectorObs((float) foodProd);
        AddVectorObs((float) coalProd);
    }

    public override void AgentAction(float[] act, string textAction) {
		// Update productions
        foodProd = -VillagerAgent.AgentCount * 0.2f;

        foreach (Factory f in farms) {
            foodProd += f.GetProd();
        }

        food += foodProd * Time.deltaTime;
        food = Mathf.Clamp(food, -100, 500);        

        coalProd = 0;

        foreach (Factory f in coalPlants) {
            coalProd += f.GetProd();
        }

        coalProd -= houses.Count * 0.2f;

        coal += coalProd * Time.deltaTime;
        coal = Mathf.Clamp(coal, -100, 500);

        happiness = VillagerAgent.AvgHappiness;

        foreach (House house in houses) {
            if (house.IsFull)
                continue;
            
            babyProgress += (happiness > 50 ? (happiness-40) /10 : 0) * Time.deltaTime;

            if (babyProgress >= 100f || (VillagerAgent.AgentCount == 0 && houses.Count > 0 && farms.Count > 0 && bars.Count > 0 && coalPlants.Count > 0)) {
                babyProgress = 0f;

                GameObject villager = Instantiate(villagerPrefab);
                VillagerAgent va = villager.GetComponent<VillagerAgent>();

                house.AddOwner(va);

                va.Bar = bars[Random.Range(0, bars.Count)].gameObject;
				va.village = this;

                va.GiveBrain(villagerBrain);
            }
            break;
        }

		if (act[0] == Time.frameCount) {
			if (act[1] != 0) {
				GameObject farm = farms[Random.Range(0, farms.Count)].gameObject;
				if (!jobOffers.Contains(farm)) {
					jobOffers.Enqueue(farm);
				}
			}

			if (act[2] != 0) {		
				GameObject c = coalPlants[Random.Range(0, coalPlants.Count)].gameObject;
				if (!jobOffers.Contains(c)) {				
					jobOffers.Enqueue(c);
				}		
			}
		}
    }

    public override void AgentReset() {
        
    }

	public override void AgentOnDone() {

	}

	void Update() {
        coalUI.fillAmount = Mathf.Lerp(coalUI.fillAmount, (coal + 100) / 600f, Time.deltaTime);
        coalUI.color = new Color(-coal/100, coal/500, 0);
        
        foodUI.fillAmount = Mathf.Lerp(foodUI.fillAmount, (food + 100) / 600f, Time.deltaTime);
        foodUI.color = new Color(-food/100, food/500, 0);
        
        happinessUI.fillAmount = Mathf.Lerp(happinessUI.fillAmount, happiness / 100f, Time.deltaTime);
        happinessUI.color = new Color(1, happiness / 100, 0);        
        
        reproducitonUI.fillAmount = Mathf.Lerp(reproducitonUI.fillAmount, babyProgress / 100f, Time.deltaTime);

        if (houses.Count > 0) {
            StringBuilder sb = new StringBuilder();
            sb.Append("population: ");
            sb.Append(VillagerAgent.AgentCount.ToString());
            sb.Append("/");
            sb.Append(houses.Count * houses[0].capacity);
            population.text = sb.ToString();
        }

        Time.timeScale = timeUI.value;
    }
}
